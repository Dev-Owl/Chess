using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Engine
{
   
    //TODO: FigureMove has to be included


    /// <summary>
    /// Provides any needed calculation for Figures on the board
    /// </summary>
    public class MoveGenerator
    {
        /// <summary>
        /// Contains information about the current game
        /// </summary>
        GameInfo currentGame;
        public GameInfo CurrentGame
        {
            get { return currentGame; }
            set { currentGame = value; }
        }
        
        /// <summary>
        /// The history of the game
        /// </summary>
        GameHistory history;
        public GameHistory History
        {
            get { return history; }
            set { history = value; }
        }

        /// <summary>
        /// Current board situation of the game
        /// </summary>
        BitBoard currentGameState;
        public BitBoard CurrentGameState
        {
            get { return currentGameState; }
            set { currentGameState = value; }
        }
        
        /// <summary>
        /// Attack datase offers basic attack creation and other helper function
        /// </summary>
        AttackDatabase attackDatabase;
        public AttackDatabase AttackDatabase
        {
            get { return attackDatabase; }
            set { attackDatabase = value; }
        }
       
        /// <summary>
        /// Is an active game running
        /// </summary>
        bool gameRunning = false;
        public bool GameRunning
        {
            get { return gameRunning; }
            set { gameRunning = value; }
        }

        /// <summary>
        /// Create a MoveGenerator with an empty Board
        /// </summary>
        public MoveGenerator()
        {
            initClass();
        }
       
        /// <summary>
        /// Create a MoveGenerator with a pre set Board
        /// </summary>
        /// <param name="CurrentState">The current game situation</param>
        public MoveGenerator(BitBoard CurrentState)
        {
            initClass();
            this.currentGameState = CurrentState;
        }

        /// <summary>
        /// Creates the interal used objects for the class and will be called in each constructor 
        /// </summary>
        private void initClass()
        {
            this.attackDatabase = new AttackDatabase();
            this.currentGameState = new BitBoard();
            this.history = new GameHistory();
        }

        /// <summary>
        /// Start a new game reset all values in the current Board
        /// </summary>
        public void NewGame()
        {
            this.currentGameState.WhiteKing = Defaults.WhiteKing;
            this.currentGameState.WhiteQueens = Defaults.WhiteQueens;
            this.currentGameState.WhiteRooks = Defaults.WhiteRooks;
            this.currentGameState.WhiteBishops = Defaults.WhiteBishops;
            this.currentGameState.WhiteKnights = Defaults.WhiteKnights;
            this.currentGameState.WhitePawns = Defaults.WhitePawns;
            this.currentGameState.BlackKing = Defaults.BlackKing;
            this.currentGameState.BlackQueens = Defaults.BlackQueens;
            this.currentGameState.BlackRooks = Defaults.BlackRooks;
            this.currentGameState.Blackbishops = Defaults.Blackbishops;
            this.currentGameState.BlackKnights = Defaults.BlackKnights;
            this.currentGameState.BlackPawns = Defaults.BlackPawns;
            this.GameRunning = true;
            this.UpdateHelperBoards();
        }
        
        /// <summary>
        /// Update all helper boards that are used for the calculation
        /// </summary>
        private void UpdateHelperBoards()
        {
            //Helper variable and temp. storage
            UInt64 tmpMoves = 0;
            UInt64 protectedFields=0;
            UInt64 position = 1;

            //Reset current attack boards
            this.currentGameState.AttackedByWhite = 0;
            this.currentGameState.AttackedByBlack = 0;
            //Reset the Attack and protected status of the fields
            foreach (UInt64 key in this.currentGameState.AttackedBy.Keys)
            {
                this.currentGameState.AttackedBy[key].Clear();
                this.currentGameState.ProtecteddBy[key].Clear();
            }
            //Collect the position of each white figure on the board
            this.currentGameState.WhitePieces = this.currentGameState.WhiteKing | this.currentGameState.WhiteQueens | this.currentGameState.WhiteRooks | this.currentGameState.WhiteBishops
                               | this.currentGameState.WhiteKnights | this.currentGameState.WhitePawns;
            //Collect the position of each black figure on the board
            this.currentGameState.BlackPieces = this.currentGameState.BlackKing | this.currentGameState.BlackQueens | this.currentGameState.BlackRooks | this.currentGameState.Blackbishops
                  | this.currentGameState.BlackKnights | this.currentGameState.BlackPawns;
            //All figures on the boards
            this.currentGameState.SquarsBlocked = this.currentGameState.WhitePieces | this.currentGameState.BlackPieces;
            //All squares without a figure
            this.currentGameState.EmptySquares = ~this.currentGameState.SquarsBlocked;
            
            for (UInt16 i = 0; i < 64; ++i)
            {
                tmpMoves = 0;
                //Get figure on the selected board position
                Figure fig = this.GetFigureAtPosition(position); 
                //Get the protected figures squares

                //Only go on if we found a figure
                if (fig != null)
                {

                    protectedFields = this.GetProtectedFields(fig,(short)i);
                    // Pawns moves are not the attacks
                    if (fig.Type != EFigures.Pawn)
                    {
                        //Moves for the figure
                        tmpMoves = this.GetMoveForFigure(fig, (short)i);
                    }
                    else
                    {
                        //calculate possible pawn attacks
                        UInt64 enemyAndEmpy = 0;
                        if (fig.Color == Defaults.BLACK)
                        {
                            enemyAndEmpy = this.currentGameState.WhitePieces | this.currentGameState.EmptySquares;
                        }
                        else
                        {
                            enemyAndEmpy = this.currentGameState.BlackPieces | this.currentGameState.EmptySquares;
                        }
                        // Check if the fields are not blocked by a friendly square
                        tmpMoves = enemyAndEmpy & this.attackDatabase.BuildPawnAttack((short)i, fig.Color);
                    }
                    //Update helper boards for the figure color
                    if (fig.Color == Defaults.WHITE)
                    {
                        this.currentGameState.AttackedByWhite |= tmpMoves;
                    }
                    else
                    {
                        this.currentGameState.AttackedByBlack |= tmpMoves;
                    }
                    //Update the attacked by status for the different fields 
                    foreach (UInt64 key in this.currentGameState.AttackedBy.Keys)
                    {
                        if ((key & tmpMoves) > 0)
                        {
                            //To get the figure position later faster we store it inside the figure object
                            fig.Position = position;
                            this.currentGameState.AttackedBy[key].Add(fig);      
                        }
                        if ((key & protectedFields) > 0)
                        {
                            //This field is protected by the figure
                            fig.Position = position;
                            this.currentGameState.ProtecteddBy[key].Add(fig);
                        }
                    }
                }
                //Jump to the next position
                position = (position << 1);
            }
            //Check if a king is in check
            this.currentGameState.WhiteKingCheck = (this.currentGameState.AttackedByBlack & this.currentGameState.WhiteKing) > 0;
            this.currentGameState.BlackKingCheck = (this.currentGameState.AttackedByWhite & this.currentGameState.BlackKing) > 0;
        }
        
        /// <summary>
        /// Get all legal moves for the given figure at the given position on the current board
        /// </summary>
        /// <param name="FigureToCheck">The Figure that is used for the calculation</param>
        /// <param name="Position">Current Position of the Figure on the Board</param>
        /// <returns>Moves for the selected Figure</returns>
        public UInt64 GetMoveForFigure(Figure FigureToCheck, Int16 Position)
        {
            //Get all possible moves for this figure at the givin position
            UInt64 legalMoves = attackDatabase.GetMoveMask(Position, FigureToCheck);
            UInt64 enemyOrEmpty = this.currentGameState.EmptySquares; //all enemies or empty squares
            UInt64 enemy = 0; //All figs of the current enemy color
            UInt64 enemyAttacked = 0;
            UInt64 protectedFields = 0;
            bool myKingInCheck = false;
            UInt64 myKingPosition = 0;

            if (FigureToCheck.Color == Defaults.WHITE)
            {
                enemyOrEmpty |= this.currentGameState.BlackPieces;
                enemy = this.currentGameState.BlackPieces;
                enemyAttacked = this.currentGameState.AttackedByBlack;
                myKingInCheck = this.currentGameState.WhiteKingCheck;
                myKingPosition = this.currentGameState.WhiteKing;
            }
            else
            {
                enemyOrEmpty |= this.currentGameState.WhitePieces;
                enemy = this.currentGameState.WhitePieces;
                enemyAttacked = this.currentGameState.AttackedByWhite;
                myKingInCheck = this.currentGameState.BlackKingCheck;
                myKingPosition = this.currentGameState.BlackKing;
            }
            if (FigureToCheck.Type != EFigures.Rook || FigureToCheck.Type != EFigures.Bishop || FigureToCheck.Type != EFigures.Queen)
            {
                legalMoves &= enemyOrEmpty;
            }
            switch (FigureToCheck.Type)
            {

                case EFigures.Pawn:
                    {
                        //Check if pawn is blocked by any other figure in fornt of him
                        if ((GetPositionFromPosition(Position, (Int16)(8 * FigureToCheck.Color)) & this.currentGameState.SquarsBlocked) > 0)
                        {
                            legalMoves = 0;
                        }
                        //Check if an attack is possible use attack datbase BuildPawnAttack function
                        legalMoves |= enemy & attackDatabase.BuildPawnAttack(Position, FigureToCheck.Color);
                    } break;
                case EFigures.Rook:
                    {
                        //Get all legal moves for the rook
                        legalMoves = GetRookMovesOn(Position, enemyOrEmpty);
                    }
                    break;
                case EFigures.Bishop:
                    {
                        //Get all legal moves for the bishop
                        legalMoves = GetBishopMovesOn(Position, enemyOrEmpty);
                    }
                    break;
                case EFigures.Queen:
                    {
                        //Queen is just rook + bishop
                        legalMoves = GetRookMovesOn(Position, enemyOrEmpty) | GetBishopMovesOn(Position, enemyOrEmpty);
                    }
                    break;
                case EFigures.King:
                    {
                        //Do not move on attacked fields 
                        legalMoves &= ~enemyAttacked;
                        //Do not attack protected Figures
                        legalMoves &= ~this.IsProtected(legalMoves, FigureToCheck.Color * -1);
                    }
                    break;

            }
            //TODO: TEST THIS CODE BELOW !!!
            //If current figure color king is under attack
            if (myKingInCheck)
            {
                //Get all attacking Figures for this king
                List<Figure> kingAttacks = this.currentGameState.AttackedBy[myKingPosition];
                //Temp storage for moves that are still valid
                UInt64 tmpMoves = 0;
                //Helper variable to store the figure directions
                UInt64 tmpKingDirection=0;
                UInt64 tmpMatchingDirection = 0;
                foreach (Figure fig in kingAttacks)
                {
                    //Pawns have diffrent move and attack fields so we need the if here
                    if (FigureToCheck.Type == EFigures.Pawn)
                    {
                        if ((this.attackDatabase.BuildPawnAttack(Position, FigureToCheck.Color) & fig.Position) >0)
                        {
                            tmpMoves |= fig.Position;
                        }
                    }
                    else
                    {
                        //If the "normal" moves are also reaching this figure 
                        if ((legalMoves & fig.Position) > 0)
                        {   //Add the position to the valid moves
                            tmpMoves |= fig.Position;
                        }
                    }
                    //Now we need to check if the way to the king could be blocked instead of an attack
                    if (fig.Type == (EFigures.Rook | EFigures.Queen | EFigures.Bishop))
                    { 
                        //Get the direction for the attacking figure 
                        //tmpAttackerDirection = PinPosition(fig.Position);
                        //Pin the King position on the Board
                        tmpKingDirection = PinPosition((short)Tools.BitOperations.MostSigExponent(myKingPosition));
                        //Only get the matching part of the two pined positions
                        tmpMatchingDirection = tmpKingDirection & PinPosition( (short)Tools.BitOperations.MostSigExponent(fig.Position));
                        //Add the blocking moves to the already checked attack move
                        tmpMoves = tmpMoves | (legalMoves & tmpMatchingDirection);
                    }

                }
                //Override the normal moves with the king in check calculations
                legalMoves = tmpMoves;
                
            }

            return legalMoves;
        }

        /// <summary>
        /// Sets all bits on the Left,Right,Bottom,Top and the diagonal bits of the given Position
        /// </summary>
        /// <param name="Position">Position that is used as the center</param>
        /// <returns>The resulting bitboard</returns>
        private UInt64 PinPosition(short Position)
        {
            return (this.attackDatabase.GetFieldsDown(Position) |
                    this.attackDatabase.GetFieldsDownLeft(Position) |
                    this.attackDatabase.GetFieldsDownRight(Position) |
                    this.attackDatabase.GetFieldsLeft(Position) |
                    this.attackDatabase.GetFieldsRight(Position));
        
        }

        /// <summary>
        /// Calculate all Fields protected by the given Figure on the given Position
        /// </summary>
        /// <param name="FigureToCheck">The Figure that is used for the calculation</param>
        /// <param name="Position">Current Position of the Figure on the Board</param>
        /// <returns>Protected Fields by the selected Figure</returns>
        public UInt64 GetProtectedFields(Figure FigureToCheck, Int16 Position)
        {
            UInt64 protectedSquares = 0;
            UInt64 friendlyAndEmpty = 0;
            //Get all non enemy squars
            if (FigureToCheck.Color == Defaults.WHITE)
            {
                friendlyAndEmpty = (this.currentGameState.WhitePieces |  this.currentGameState.EmptySquares);
            }
            else 
            {
                friendlyAndEmpty = (this.currentGameState.BlackPieces | this.currentGameState.EmptySquares);
            }
            //For sliding figures we need different calculation
            if (FigureToCheck.Type == EFigures.Bishop || FigureToCheck.Type == EFigures.Rook || FigureToCheck.Type == EFigures.Queen)
            {
                //NOTE: use related method for figure and pass the friendlyAndEmpty board to it instead of the enemyAndEmpty ???
                if (FigureToCheck.Type == EFigures.Bishop)
                { 
                    protectedSquares = GetBishopMovesOn(Position,friendlyAndEmpty);
                }
                else if (FigureToCheck.Type == EFigures.Rook)
                { 
                    protectedSquares = GetRookMovesOn(Position,friendlyAndEmpty);
                }
                else
                {
                    protectedSquares = (GetRookMovesOn(Position, friendlyAndEmpty) | GetBishopMovesOn(Position, friendlyAndEmpty));
                }
            }
            else 
            {
                //Get possible moves
                protectedSquares = attackDatabase.GetMoveMask(Position, FigureToCheck);
                //If pawn reset the moves to the attack fields
                if (FigureToCheck.Type == EFigures.Pawn)
                {
                   protectedSquares = attackDatabase.BuildPawnAttack(Position, FigureToCheck.Color);
                }
                //remove all enemys in the squars
                protectedSquares &= friendlyAndEmpty;
                //NOTE: Might be needed to excluded attacked fields for the king but not sure yet...
            }

            return protectedSquares;
        }
   
        /// <summary>
        /// Make a move with a figure to a position
        /// </summary>
        /// <param name="FigureToMove">The Figure that should be moved</param>
        /// <param name="TargetPosition">The new position of the figure</param>
        public void MakeAMove(Figure FigureToMove, Int16 TargetPosition)
        {
            //IDEA: Fire pre and after move events to hook them elsewhere ( Computer player, other calculations)

            //TODO: Add the current Bitboard object to a history to offer the possibility to revert a move
            
            //Change the related bitboards
            this.MoveFigure(FigureToMove, TargetPosition);
            //Refresh the helper boards
            this.UpdateHelperBoards();

            
        }

        /// <summary>
        /// Move a Figure to the provided Position. This function just sets and removes the bits in the current bitboard.
        /// </summary>
        /// <param name="FigureToMove">Figure that should be moved to a new position</param>
        /// <param name="TargetPosition">The new position of the figure</param>
        private void MoveFigure(Figure FigureToMove, Int16 TargetPosition)
        {
            //Get the figure at the target square
            Figure targetFigure = this.GetFigureAtPosition((ulong)Math.Pow(2, TargetPosition));
            //Set the target bit to 1 so we can use it in our calculation
            ulong calculationBoard =  (ulong)Math.Pow(2, TargetPosition); 
            //Depending on the figure we have to move set the matching bitboard
            switch (FigureToMove.Type)
            {
                case EFigures.Bishop:
                    {
                        if (FigureToMove.Color == Defaults.WHITE)
                        {
                            //Remove the current position from the bitboard
                            this.currentGameState.WhiteBishops ^= FigureToMove.Position;
                            //Set the new position
                            this.currentGameState.WhiteBishops |= calculationBoard; 
                        }
                        else 
                        {
                            //Remove the current position from the bitboard
                            this.currentGameState.Blackbishops ^= FigureToMove.Position;
                            //Set the new position
                            this.currentGameState.Blackbishops |= calculationBoard; 
                        }
                    }
                    break;
                case EFigures.Knight: 
                    {
                        if (FigureToMove.Color == Defaults.WHITE)
                        {
                            //Remove the current position from the bitboard
                            this.currentGameState.WhiteKnights ^= FigureToMove.Position;
                            //Set the new position
                            this.currentGameState.WhiteKnights |= calculationBoard;
                        }
                        else
                        {
                            //Remove the current position from the bitboard
                            this.currentGameState.BlackKnights ^= FigureToMove.Position;
                            //Set the new position
                            this.currentGameState.BlackKnights |= calculationBoard;
                        }
                    }
                    break;
                case EFigures.Pawn: 
                    {
                        if (FigureToMove.Color == Defaults.WHITE)
                        {
                            //Remove the current position from the bitboard
                            this.currentGameState.WhitePawns ^= FigureToMove.Position;
                            //Set the new position
                            this.currentGameState.WhitePawns |= calculationBoard;
                        }
                        else
                        {
                            //Remove the current position from the bitboard
                            this.currentGameState.BlackPawns ^= FigureToMove.Position;
                            //Set the new position
                            this.currentGameState.BlackPawns |= calculationBoard;
                        }
                    }
                    break;
                case EFigures.Queen: 
                    {
                        if (FigureToMove.Color == Defaults.WHITE)
                        {
                            //Remove the current position from the bitboard
                            this.currentGameState.WhiteQueens ^= FigureToMove.Position;
                            //Set the new position
                            this.currentGameState.WhiteQueens |= calculationBoard;
                        }
                        else
                        {
                            //Remove the current position from the bitboard
                            this.currentGameState.BlackQueens ^= FigureToMove.Position;
                            //Set the new position
                            this.currentGameState.BlackQueens |= calculationBoard;
                        }
                    }
                    break;
                case EFigures.Rook:
                    {
                        if (FigureToMove.Color == Defaults.WHITE)
                        {
                            //Remove the current position from the bitboard
                            this.currentGameState.WhiteRooks ^= FigureToMove.Position;
                            //Set the new position
                            this.currentGameState.WhiteRooks |= calculationBoard;
                        }
                        else
                        {
                            //Remove the current position from the bitboard
                            this.currentGameState.BlackRooks ^= FigureToMove.Position;
                            //Set the new position
                            this.currentGameState.BlackRooks |= calculationBoard;
                        }
                    }
                    break;
            }
        }
        
        /// <summary>
        /// Calculate all possible moves for the Rook on the given Position
        /// </summary>
        /// <param name="Position">Position of the Rook</param>
        /// <param name="EnemyAndEmpty">Representaion of all enemies and empty fields</param>
        /// <returns>Legal Possible moves</returns>
        private UInt64 GetRookMovesOn(Int16 Position, UInt64 EnemyAndEmpty)
        {
            UInt64 legalMoves = 0;
            //Get all blocking pices to the right of this figure
            UInt64 currentboard = this.attackDatabase.GetFieldsRight(Position);
            //Set all bits to 1 if a figure on the row (friendly or enemy)
            UInt64 currentmoves = currentboard & this.currentGameState.SquarsBlocked;
            //shift the bits to set all bits after the figure on the row
            currentmoves = (currentmoves >> 1) | (currentmoves >> 2) | (currentmoves >> 3) | (currentmoves >> 4) | (currentmoves >> 5) | (currentmoves >> 6);
            //remove all over overflowing or left bits
            currentmoves &= currentboard;
            //Remove the bits behind the figure
            currentmoves ^= currentboard;
            //Remove friendly figure positions form the attack board
            currentmoves &= EnemyAndEmpty;
            //Add to total moves board
            legalMoves |= currentmoves;

            currentboard = this.attackDatabase.GetFieldsLeft(Position);
            //Set all bits to 1 if a figure on the row (friendly or enemy)
            currentmoves = currentboard & this.currentGameState.SquarsBlocked;
            //shift the bits to set all bits after the figure on the row
            currentmoves = (currentmoves << 1) | (currentmoves << 2) | (currentmoves << 3) | (currentmoves << 4) | (currentmoves << 5) | (currentmoves << 6);
            //remove all over overflowing or left bits
            currentmoves &= currentboard;
            //Remove the bits behind the figure
            currentmoves ^= currentboard;
            //Remove friendly figure positions form the attack board
            currentmoves &= EnemyAndEmpty;
            //Add to total moves board
            legalMoves |= currentmoves;

            currentboard = this.attackDatabase.GetFieldsUP(Position);
            //Set all bits to 1 if a figure on the row (friendly or enemy)
            currentmoves = currentboard & this.currentGameState.SquarsBlocked;
            //shift the bits to set all bits after the figure on the row
            currentmoves = (currentmoves << 8) | (currentmoves << 16) | (currentmoves << 24) | (currentmoves << 32) | (currentmoves << 40) | (currentmoves << 48);
            //remove all over overflowing or left bits
            currentmoves &= currentboard;
            //Remove the bits behind the figure
            currentmoves ^= currentboard;
            //Remove friendly figure positions form the attack board
            currentmoves &= EnemyAndEmpty;
            //Add to total moves board
            legalMoves |= currentmoves;

            currentboard = this.attackDatabase.GetFieldsDown(Position);
            //Set all bits to 1 if a figure on the row (friendly or enemy)
            currentmoves = currentboard & this.currentGameState.SquarsBlocked;
            //shift the bits to set all bits after the figure on the row
            currentmoves = (currentmoves >> 8) | (currentmoves >> 16) | (currentmoves >> 24) | (currentmoves >> 32) | (currentmoves >> 40) | (currentmoves >> 48);
            //remove all over overflowing or left bits
            currentmoves &= currentboard;
            //Remove the bits behind the figure
            currentmoves ^= currentboard;
            //Remove friendly figure positions form the attack board
            currentmoves &= EnemyAndEmpty;
            //Add to total moves board
            legalMoves |= currentmoves;

            return legalMoves;

        }
      
        /// <summary>
        /// Calculate all possible moves for the Bishop on the given Position
        /// </summary>
        /// <param name="Position">Position of the Bishop</param>
        /// <param name="EnemyAndEmpty">Representaion of all enemies and empty fields</param>
        /// <returns>Legal Possible moves</returns>
        private UInt64 GetBishopMovesOn(Int16 Position, UInt64 EnemyAndEmpty)
        {
            UInt64 legalMoves = 0;
            //Get all blocking pices to the right of this figure
            UInt64 currentboard = this.attackDatabase.GetFieldsUpRight(Position);
            //Set all bits to 1 if a figure on the row (friendly or enemy)
            UInt64 currentmoves = currentboard & this.currentGameState.SquarsBlocked;
            //shift the bits to set all bits after the figure on the row
            currentmoves = (currentmoves << 7) | (currentmoves << 14) | (currentmoves << 21) | (currentmoves << 28) | (currentmoves << 35) | (currentmoves << 42);
            //remove all over overflowing or left bits
            currentmoves &= currentboard;
            //Remove the bits behind the figure
            currentmoves ^= currentboard;
            //Remove friendly figure positions form the attack board
            currentmoves &= EnemyAndEmpty;
            //Add to total moves board
            legalMoves |= currentmoves;

            //Get all blocking pices to the right of this figure
            currentboard = this.attackDatabase.GetFieldsDownRight(Position);
            //Set all bits to 1 if a figure on the row (friendly or enemy)
            currentmoves = currentboard & this.currentGameState.SquarsBlocked;
            //shift the bits to set all bits after the figure on the row
            currentmoves = (currentmoves >> 9) | (currentmoves >> 18) | (currentmoves >> 27) | (currentmoves >> 36) | (currentmoves >> 45) | (currentmoves >> 54);
            //remove all over overflowing or left bits
            currentmoves &= currentboard;
            //Remove the bits behind the figure
            currentmoves ^= currentboard;
            //Remove friendly figure positions form the attack board
            currentmoves &= EnemyAndEmpty;
            //Add to total moves board
            legalMoves |= currentmoves;

            //Get all blocking pices to the right of this figure
            currentboard = this.attackDatabase.GetFieldsDownLeft(Position);
            //Set all bits to 1 if a figure on the row (friendly or enemy)
            currentmoves = currentboard & this.currentGameState.SquarsBlocked;
            //shift the bits to set all bits after the figure on the row
            currentmoves = (currentmoves >> 7) | (currentmoves >> 14) | (currentmoves >> 21) | (currentmoves >> 28) | (currentmoves >> 35) | (currentmoves >> 42);//(currentmoves << 9) | (currentmoves << 18) | (currentmoves << 27) | (currentmoves << 36) | (currentmoves << 45) | (currentmoves << 54);
            //remove all over overflowing or left bits
            currentmoves &= currentboard;
            //Remove the bits behind the figure
            currentmoves ^= currentboard;
            //Remove friendly figure positions form the attack board
            currentmoves &= EnemyAndEmpty;
            //Add to total moves board
            legalMoves |= currentmoves;

            //Get all blocking pices to the right of this figure
            currentboard = this.attackDatabase.GetFieldsUpLeft(Position);
            //Set all bits to 1 if a figure on the row (friendly or enemy)
            currentmoves = currentboard & this.currentGameState.SquarsBlocked;
            //shift the bits to set all bits after the figure on the row
            currentmoves = (currentmoves << 9) | (currentmoves << 18) | (currentmoves << 27) | (currentmoves << 36) | (currentmoves << 45) | (currentmoves << 54);
            //remove all over overflowing or left bits
            currentmoves &= currentboard;
            //Remove the bits behind the figure
            currentmoves ^= currentboard;
            //Remove friendly figure positions form the attack board
            currentmoves &= EnemyAndEmpty;
            //Add to total moves board
            legalMoves |= currentmoves;
            return legalMoves;
        }
       
        /// <summary>
        /// Creates based on the given Postion and the MoveRange the target Position
        /// </summary>
        /// <param name="SourcePosition">Start position for the calculation</param>
        /// <param name="MoveRange">Fields to move on the board. Can be positive or negative.</param>
        /// <returns>New Position</returns>
        public UInt64 GetPositionFromPosition(Int16 SourcePosition, Int16 MoveRange)
        {
            UInt64 result = 0;
            //Prevent out of board moves ( should never happen but ....)
            if (SourcePosition + MoveRange > 0 & SourcePosition + MoveRange < 64)
            {
                result |= (UInt64)Math.Pow(2, SourcePosition + MoveRange);
            }
            return result;
        }
      
        /// <summary>
        /// Get the Figure at the given Position on the current Board
        /// </summary>
        /// <param name="Position">The Position of the Figure</param>
        /// <returns>Figure object if Figure was found if not null.</returns>
        public Figure GetFigureAtPosition(UInt64 Position)
        {
            Figure returnValue = null;
            if (gameRunning)
            {
                UInt64 result = Position & this.currentGameState.SquarsBlocked;
                if (result != 0)
                {
                    result = Position & this.currentGameState.BlackPieces;
                    if (result != 0)
                    {
                        //Black Figure
                        if ((this.currentGameState.BlackKing & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.BLACK, EFigures.King);
                        }
                        if ((this.currentGameState.BlackQueens & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.BLACK, EFigures.Queen);
                        }
                        if ((this.currentGameState.BlackRooks & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.BLACK, EFigures.Rook);
                        }
                        if ((this.currentGameState.Blackbishops & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.BLACK, EFigures.Bishop);
                        }
                        if ((this.currentGameState.BlackKnights & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.BLACK, EFigures.Knight);
                        }
                        if ((this.currentGameState.BlackPawns & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.BLACK, EFigures.Pawn);
                        }
                    }
                    else
                    {
                        //White Figure
                        if ((this.currentGameState.WhiteKing & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.WHITE, EFigures.King);
                        }
                        if ((this.currentGameState.WhiteQueens & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.WHITE, EFigures.Queen);
                        }
                        if ((this.currentGameState.WhiteRooks & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.WHITE, EFigures.Rook);
                        }
                        if ((this.currentGameState.WhiteBishops & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.WHITE, EFigures.Bishop);
                        }
                        if ((this.currentGameState.WhiteKnights & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.WHITE, EFigures.Knight);
                        }
                        if ((this.currentGameState.WhitePawns & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.WHITE, EFigures.Pawn);
                        }
                    }
                }
            }
            //Set the current position of the figure to speed up calculations later
            returnValue.Position = Position;
            //Return the result 
            return returnValue;
        }
      
        /// <summary>
        /// Returns an Array of Figures that are Protecting the Provided Fields for the given Color
        /// </summary>
        /// <param name="SearchValues">Fields to be checked</param>
        /// <param name="Color">Only this Color will be checked</param>
        /// <returns>Array of Figures that are matching the search creteria</returns>
        public Figure[] GetProtectingFigures(UInt64 SearchValues, int Color)
        {
            //list with figures that protected the passed search mask
            List<Figure> protectors = new List<Figure>();
            //Loop to the protection list
            foreach (UInt64 Key in this.currentGameState.ProtecteddBy.Keys)
            {
                if ((Key & SearchValues) > 0)
                { 
                    //Add the searched figures to our temp. storage
                    foreach (Figure fig in this.currentGameState.ProtecteddBy[Key].Where(F=>F.Color == Color))
                    {
                        protectors.Add(fig);
                    }
                }
            }
            return protectors.ToArray();
        }
      
        /// <summary>
        /// Calculates a Value that contains all Bits that are Protected based on the given Color and SearchVlaue
        /// </summary>
        /// <param name="SearchValues">Bits to be checked for protection</param>
        /// <param name="Color">Only check this color</param>
        /// <returns>Protected fields</returns>
        public UInt64 IsProtected(UInt64 SearchValues, int Color)
        {
            UInt64 result = 0;
            //Loop to the protection list
            foreach (UInt64 Key in this.currentGameState.ProtecteddBy.Keys)
            {
                if ((Key & SearchValues) > 0)
                {
                    //Set bits if we find figures for the given color
                    foreach (Figure fig in this.currentGameState.ProtecteddBy[Key].Where(F => F.Color == Color))
                    {
                        result |= Key;
                        break;
                    }
                }
            }
            
            return result;
        }
    }
}

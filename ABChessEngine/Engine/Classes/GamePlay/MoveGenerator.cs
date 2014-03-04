using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
 * TODO: Replace all currentGameState to a local copy to support virtual moves from the AI
 * 
 */


namespace ABChess.Engine
{
   
    /// <summary>
    /// Provides any needed calculation for Figures on the board
    /// </summary>
    public class MoveGenerator
    {
        /// <summary>
        /// Event handler that is fired when the game ends because a player has won the game 
        /// </summary>
        public event EventHandler<GameEndedEventArgs> GameEnded;
        
        /// <summary>
        /// In case of a Pawn promotion the engine will request the decision of the player with this interface
        /// </summary>
        IPromotion promotionHandlerWhite;
        public IPromotion PromotionHandlerWhite
        {
            get { return this.promotionHandlerWhite; }
            set {
                if (value != null)
                {
                    this.promotionHandlerWhite = value;
                }
            }
        }

        /// <summary>
        /// In case of a Pawn promotion the engine will request the decision of the player with this interface
        /// </summary>
        IPromotion promotionHandlerBlack;
        public IPromotion PromotionHandlerBlack
        {
            get { return this.promotionHandlerBlack; }
            set
            {
                if (value != null)
                {
                    this.promotionHandlerBlack = value;
                }
            }
        }

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
            set { 
                history = value; 
                //When a new Histor is set , set the Movegenerator to enable reverting of moves
                history.MoveGenerator = this;
            }
        }

        /// <summary>
        /// Current board situation of the game
        /// </summary>
        BitBoard currentGameState;
        public BitBoard CurrentGameState
        {
            get { return currentGameState; }
            set {
                currentGameState.Dispose();
                currentGameState = null;
                currentGameState = value;
                //When the gamestate was changed reload the helper boards if the game is running
                if (gameRunning)
                {
                    this.UpdateHelperBoards(this.currentGameState);
                }
            }
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
            this.attackDatabase = new AttackDatabase(null);
            this.currentGameState = new BitBoard();
            this.history = new GameHistory();
            this.history.MoveGenerator = this;
        }

        /// <summary>
        /// Start a new game reset all values in the current Board
        /// </summary>
        public void NewGame(GameInfo NewGame)
        {
            this.currentGame = null;
            this.currentGame = NewGame;
            this.currentGameState.Dispose();
            this.currentGameState = null;
            this.currentGameState = new BitBoard();
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
            this.history.AddGame(NewGame);
            this.UpdateHelperBoards(this.currentGameState);
        }
       
        /// <summary>
        /// Update all helper boards that are used for the calculation
        /// </summary>
        private void UpdateHelperBoards(BitBoard WorkingBoard)
        {
            //Helper variable and temp. storage
            UInt64 tmpMoves = 0;
            UInt64 protectedFields=0;
            UInt64 position = 1;

            //Reset current attack boards
            WorkingBoard.AttackedByWhite = 0;
            WorkingBoard.AttackedByBlack = 0;
            //Reset the Attack and protected status of the fields
            foreach (UInt64 key in WorkingBoard.AttackedBy.Keys)
            {
                WorkingBoard.AttackedBy[key].Clear();
                WorkingBoard.ProtecteddBy[key].Clear();
            }
            //Collect the position of each white figure on the board
            WorkingBoard.WhitePieces = WorkingBoard.WhiteKing | WorkingBoard.WhiteQueens | WorkingBoard.WhiteRooks | WorkingBoard.WhiteBishops
                               | WorkingBoard.WhiteKnights | WorkingBoard.WhitePawns;
            //Collect the position of each black figure on the board
            WorkingBoard.BlackPieces = WorkingBoard.BlackKing | WorkingBoard.BlackQueens | WorkingBoard.BlackRooks | WorkingBoard.Blackbishops
                  | WorkingBoard.BlackKnights | WorkingBoard.BlackPawns;
            //All figures on the boards
            WorkingBoard.SquarsBlocked = WorkingBoard.WhitePieces | WorkingBoard.BlackPieces;
            //All squares without a figure
            WorkingBoard.EmptySquares = ~WorkingBoard.SquarsBlocked;
            
            for (UInt16 i = 0; i < 64; ++i)
            {
                tmpMoves = 0;
                //Get figure on the selected board position
                Figure fig = this.GetFigureAtPosition(position,WorkingBoard); 
                //Get the protected figures squares

                //Only go on if we found a figure
                if (fig != null)
                {

                    protectedFields = this.GetProtectedFields(fig,(short)i,WorkingBoard);
                    // Pawns moves are not the attacks
                    if (fig.Type != EFigures.Pawn)
                    {
                        //Moves for the figure
                        tmpMoves = this.GetMoveForFigure(fig, (short)i,WorkingBoard);
                    }
                    else
                    {
                        //calculate possible pawn attacks
                        UInt64 enemyAndEmpy = 0;
                        if (fig.Color == Defaults.BLACK)
                        {
                            enemyAndEmpy = WorkingBoard.WhitePieces | WorkingBoard.EmptySquares;
                        }
                        else
                        {
                            enemyAndEmpy = WorkingBoard.BlackPieces | WorkingBoard.EmptySquares;
                        }
                        // Check if the fields are not blocked by a friendly square
                        tmpMoves = enemyAndEmpy & this.attackDatabase.BuildPawnAttack((short)i, fig.Color);
                    }
                    //Update helper boards for the figure color
                    if (fig.Color == Defaults.WHITE)
                    {
                        WorkingBoard.AttackedByWhite |= tmpMoves;
                    }
                    else
                    {
                        WorkingBoard.AttackedByBlack |= tmpMoves;
                    }
                    //Update the attacked by status for the different fields 
                    foreach (UInt64 key in WorkingBoard.AttackedBy.Keys)
                    {
                        if ((key & tmpMoves) > 0)
                        {
                            //To get the figure position later faster we store it inside the figure object
                            fig.Position = position;
                            WorkingBoard.AttackedBy[key].Add(fig);      
                        }
                        if ((key & protectedFields) > 0)
                        {
                            //This field is protected by the figure
                            fig.Position = position;
                            WorkingBoard.ProtecteddBy[key].Add(fig);
                        }
                    }
                }
                //Jump to the next position
                position = (position << 1);
            }
            //Check if a king is in check
            WorkingBoard.WhiteKingCheck = (WorkingBoard.AttackedByBlack & WorkingBoard.WhiteKing) > 0;
            WorkingBoard.BlackKingCheck = (WorkingBoard.AttackedByWhite & WorkingBoard.BlackKing) > 0;
        }
        
        /// <summary>
        /// Get all legal moves for the given figure at the given position on the current board
        /// </summary>
        /// <param name="FigureToCheck">The Figure that is used for the calculation</param>
        /// <param name="Position">Current Position of the Figure on the Board</param>
        /// <returns>Moves for the selected Figure</returns>
        public UInt64 GetMoveForFigure(Figure FigureToCheck, Int16 Position,BitBoard WorkingBoard)
        {
            
            //Get all possible moves for this figure at the givin position
            UInt64 legalMoves = attackDatabase.GetMoveMask(Position, FigureToCheck);
            UInt64 enemyOrEmpty = WorkingBoard.EmptySquares; //all enemies or empty squares
            UInt64 enemy = 0; //All figs of the current enemy color
            UInt64 enemyAttacked = 0;
            UInt64 protectedFields = 0;
            UInt64 enemyEnPassant = 0;
            bool myKingInCheck = false;
            UInt64 myKingPosition = 0;
            short myKingPositionShort = 0;
            bool myKingMoved = false;
            bool myLeftRookMoved = false;
            bool myRightRookMoved = false;
            UInt64 myLeftRook =0;
            UInt64 myRightRook =0;
            UInt64 CastlingLeft = 0;
            UInt64 CastlingRight = 0;
            UInt64 CastlingKingFieldsLeft =0;
            UInt64 CastlingKingFieldsRight =0;
            UInt64 myFigures =0;
            if (FigureToCheck.Color == Defaults.WHITE)
            {
                enemyOrEmpty |= WorkingBoard.BlackPieces;
                enemy = WorkingBoard.BlackPieces;
                enemyAttacked = WorkingBoard.AttackedByBlack;
                myKingInCheck = WorkingBoard.WhiteKingCheck;
                myKingPosition = WorkingBoard.WhiteKing; 
                myKingPositionShort = (short)Tools.BitOperations.MostSigExponent(myKingPosition);
                enemyEnPassant = WorkingBoard.EnPassantBlack;
                myKingMoved = WorkingBoard.WhiteKingMoved;
                myLeftRookMoved = WorkingBoard.WhiteLeftRookMoved;
                myRightRookMoved = WorkingBoard.WhiteRightRookMoved;
                myLeftRook = Defaults.WhiteLeftRookStartPosition;
                myRightRook = Defaults.WhiteRightRookStartPosition;
                CastlingLeft = Defaults.CastlingWhiteLeft;
                CastlingRight = Defaults.CastlingWhiteRight;
                CastlingKingFieldsLeft = Defaults.CastlingWhiteKingFieldsLeft;
                CastlingKingFieldsRight = Defaults.CastlingWhiteKingFieldsRight;
                myFigures = WorkingBoard.WhitePieces;
            }
            else
            {
                enemyOrEmpty |= WorkingBoard.WhitePieces;
                enemy = WorkingBoard.WhitePieces;
                enemyAttacked = WorkingBoard.AttackedByWhite;
                myKingInCheck = WorkingBoard.BlackKingCheck;
                myKingPosition = WorkingBoard.BlackKing;
                myKingPositionShort = (short)Tools.BitOperations.MostSigExponent(myKingPosition);
                enemyEnPassant = WorkingBoard.EnPassantWhite;
                myKingMoved = WorkingBoard.BalckKingMoved;
                myLeftRookMoved = WorkingBoard.BlackLeftRookMoved;
                myRightRookMoved = WorkingBoard.BlackRightRookMoved;
                myLeftRook = Defaults.BlackLeftRookStartPosition;
                myRightRook = Defaults.BlackRightRookStartPosition;
                CastlingLeft = Defaults.CastlingBlackLeft;
                CastlingRight = Defaults.CastlingBlackRight;
                CastlingKingFieldsLeft = Defaults.CastlingBlackKingFieldsLeft;
                CastlingKingFieldsRight = Defaults.CastlingBlackKingFieldsRight;
                myFigures = WorkingBoard.BlackPieces;
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
                        legalMoves &= WorkingBoard.EmptySquares;
                        //Check if an attack is possible use attack datbase BuildPawnAttack function
                        legalMoves |= (enemy & attackDatabase.BuildPawnAttack(Position, FigureToCheck.Color)) | (attackDatabase.BuildPawnAttack(Position, FigureToCheck.Color) & enemyEnPassant);


                    } break;
                case EFigures.Rook:
                    {
                        //Get all legal moves for the rook
                        legalMoves = GetRookMovesOn(Position, enemyOrEmpty,WorkingBoard);
                    }
                    break;
                case EFigures.Bishop:
                    {
                        //Get all legal moves for the bishop
                        legalMoves = GetBishopMovesOn(Position, enemyOrEmpty,WorkingBoard);
                    }
                    break;
                case EFigures.Queen:
                    {
                        //Queen is just rook + bishop
                        legalMoves = GetRookMovesOn(Position, enemyOrEmpty,WorkingBoard) | GetBishopMovesOn(Position, enemyOrEmpty,WorkingBoard);
                    }
                    break;
                case EFigures.King:
                    {
                        //Do not move on attacked fields 
                        legalMoves &= ~enemyAttacked;
                        //Do not attack protected Figures
                        legalMoves &= ~this.IsProtected(legalMoves, FigureToCheck.Color * -1,WorkingBoard);
                        //If we are in check we can also not walk on a field that is attacked behind us
                        if (myKingInCheck)
                        {
                            foreach (Figure fig in WorkingBoard.AttackedBy[myKingPosition].Where<Figure>(f => f.Color != FigureToCheck.Color).ToList<Figure>())
                            {
                                legalMoves &= ~attackDatabase.GetMoveMask((short)Tools.BitOperations.MostSigExponent(fig.Position), fig);
                            }
                        }
                        else {
                            //If the king was not moved and at least one rook too we can castel
                            if (!myKingMoved && (!myLeftRookMoved || !myRightRookMoved))
                            {
                               
                                if (!myLeftRookMoved)
                                {
                                    legalMoves |= CastlingCheck(myLeftRook, Position, enemyAttacked, true, CastlingKingFieldsLeft, CastlingLeft,WorkingBoard);
                                }
                                if(!myRightRookMoved) 
                                {
                                    legalMoves |= CastlingCheck(myRightRook, Position, enemyAttacked, false, CastlingKingFieldsRight, CastlingRight,WorkingBoard);
                                }
                            }
                        }
                    }
                    break;

            }
            //If current figure color king is under attack
            if (myKingInCheck && FigureToCheck.Type != EFigures.King)
            {
                //Get all attacking Figures for this king
                List<Figure> kingAttacks = WorkingBoard.AttackedBy[myKingPosition].Where<Figure>(f => f.Color != FigureToCheck.Color).ToList<Figure>();
                //Temp storage for moves that are still valid
                UInt64 tmpMoves = 0;
                //Helper variable to store the figure directions
                UInt64 tmpKingDirection = 0;
                UInt64 tmpMatchingDirection = 0;
                foreach (Figure fig in kingAttacks)
                {
                    //Pawns have diffrent move and attack fields so we need the if here
                    if (FigureToCheck.Type == EFigures.Pawn)
                    {
                        if ((this.attackDatabase.BuildPawnAttack(Position, FigureToCheck.Color) & fig.Position) > 0)
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
                    if (fig.Type == EFigures.Rook || fig.Type == EFigures.Queen || fig.Type == EFigures.Bishop)
                    {
                        //The mappend short positions 
                        short attackingPosition = (short)Tools.BitOperations.MostSigExponent(fig.Position);
                        //Pin the King position on the Board
                        tmpKingDirection = PinPosition(myKingPositionShort, fig.Type);
                        //Only get the matching part of the two pined positions
                        tmpMatchingDirection = tmpKingDirection & PinPosition(attackingPosition, fig.Type);
                        //If the king is blow the attacking figure remove the fields behind the king and above the figure
                        if (myKingPositionShort < attackingPosition)
                        {
                            tmpMatchingDirection &= (~this.FillToPositionFromBottom(myKingPositionShort));
                            tmpMatchingDirection &= (~this.FillToPositionFromTop(attackingPosition));
                        }
                        else if (myKingPositionShort > attackingPosition)
                        {   //If it is the other way remove the fields above the king and below the attacking figure
                            tmpMatchingDirection &= (~this.FillToPositionFromBottom(attackingPosition));
                            tmpMatchingDirection &= (~this.FillToPositionFromTop(myKingPositionShort));
                        }
                        //Add the blocking moves to the already checked attack move
                        tmpMoves = tmpMoves | (legalMoves & tmpMatchingDirection);
                    }

                }
                //Override the normal moves with the king in check calculations
                legalMoves = tmpMoves;

            }
            //We also have to check if this figure is blocking another figure and protecting the king
            if ((PinPosition(myKingPositionShort) & FigureToCheck.Position) > 0 && FigureToCheck.Type != EFigures.King) //if we are on the same line as our king
            { 
                //Only check the complete part below if no figure is between me and the king
                bool needToCheck = true;
                //used to store the check if another figure is  in the way to the king so we can move normaly 
                UInt64 otherFigureCheck =0;

                //King is above me
                if (myKingPositionShort > Position)
                {
                    //Check if the kin is on our left side 
                    if ((this.attackDatabase.GetFieldsLeft(Position) & myKingPosition) > 0)
                    { 
                        //The king is on our left side get all filds between us and the king
                        otherFigureCheck = this.attackDatabase.GetFieldsLeft(Position) & (~this.FillToPositionFromTop(myKingPositionShort,true));
                        //If it is more then the current figure we have to stop the check
                        if ((otherFigureCheck & myFigures) != 0)
                        {
                            needToCheck = false;
                        }
                    }
                    else if ((this.attackDatabase.GetFieldsUpLeft(Position) & myKingPosition) > 0)
                    {
                        //The king is on our left side get all filds between us and the king
                        otherFigureCheck = this.attackDatabase.GetFieldsUpLeft(Position) & (~this.FillToPositionFromTop(myKingPositionShort, true));
                        //If it is more then the current figure we have to stop the check
                        if ((otherFigureCheck & myFigures) !=0)
                        {
                            needToCheck = false;
                        }
                    }
                    else if ((this.attackDatabase.GetFieldsUpRight(Position) & myKingPosition) > 0)
                    {
                        //The king is on our left side get all filds between us and the king
                        otherFigureCheck = this.attackDatabase.GetFieldsUpRight(Position) & (~this.FillToPositionFromTop(myKingPositionShort, true));
                        //If it is more then the current figure we have to stop the check
                        if ((otherFigureCheck & myFigures) != 0)
                        {
                            needToCheck = false;
                        }
                    }
                    else if ((this.attackDatabase.GetFieldsUP(Position) & myKingPosition) > 0)
                    {
                        //The king is on our left side get all filds between us and the king
                        otherFigureCheck = this.attackDatabase.GetFieldsUP(Position) & (~this.FillToPositionFromTop(myKingPositionShort, true));
                        //If it is more then the current figure we have to stop the check
                        if ((otherFigureCheck & myFigures) != 0)
                        {
                            needToCheck = false;
                        }
                    }
                }
                else
                { //King is below me
                    //Check if the kin is on our left side 
                    if ((this.attackDatabase.GetFieldsRight(Position) & myKingPosition) > 0)
                    {
                        //The king is on our left side get all filds between us and the king
                        otherFigureCheck = this.attackDatabase.GetFieldsRight(Position) & (~this.FillToPositionFromBottom(myKingPositionShort, true));
                        //If it is more then the current figure we have to stop the check
                        if ((otherFigureCheck & myFigures) != 0)
                        {
                            needToCheck = false;
                        }
                    }
                    else if ((this.attackDatabase.GetFieldsDownLeft(Position) & myKingPosition) > 0)
                    {
                        //The king is on our left side get all filds between us and the king
                        otherFigureCheck = this.attackDatabase.GetFieldsDownLeft(Position) & (~this.FillToPositionFromBottom(myKingPositionShort, true));
                        //If it is more then the current figure we have to stop the check
                        if ((otherFigureCheck & myFigures) != 0)
                        {
                            needToCheck = false;
                        }
                    }
                    else if ((this.attackDatabase.GetFieldsDownRight(Position) & myKingPosition) > 0)
                    {
                        //The king is on our left side get all filds between us and the king
                        otherFigureCheck = this.attackDatabase.GetFieldsDownRight(Position) & (~this.FillToPositionFromBottom(myKingPositionShort, true));
                        //If it is more then the current figure we have to stop the check
                        if ((otherFigureCheck & myFigures) != 0)
                        {
                            needToCheck = false;
                        }
                    }
                    else if ((this.attackDatabase.GetFieldsDown(Position) & myKingPosition) > 0)
                    {
                        //The king is on our left side get all filds between us and the king
                        otherFigureCheck = this.attackDatabase.GetFieldsDown(Position) & (~this.FillToPositionFromBottom(myKingPositionShort, true));
                        //If it is more then the current figure we have to stop the check
                        if ((otherFigureCheck & myFigures) != 0)
                        {
                            needToCheck = false;
                        }
                    }
                }
                if (needToCheck)
                {
                    //If the king is not in check make sure if you would move that the king gets in check
                    //if this figure is attacked by queen,rook or bishop we have to check it
                    List<Figure> attackers = WorkingBoard.AttackedBy[FigureToCheck.Position].Where<Figure>(f => f.Color != FigureToCheck.Color
                                             && (f.Type == EFigures.Rook || f.Type == EFigures.Queen || f.Type == EFigures.Bishop)).ToList<Figure>();
                    //If we have attackers go on 
                    if (attackers.Count > 0)
                    {
                        //Helper for storing the matching fields between the attacking figure and the king
                        UInt64 tmpMatchingDirection = 0;
                        short attackingPosition = 0;
                        bool kingInDanger = false;
                        foreach (Figure fig in attackers)
                        {
                            //Get the attacking figure position in the 1 to 63 code
                            attackingPosition = (short)Tools.BitOperations.MostSigExponent(fig.Position);
                            //Based on the figure type check if we found matching fields
                            tmpMatchingDirection = PinPosition(attackingPosition, fig.Type) & PinPosition(myKingPositionShort, fig.Type);
                            //If any thing was found go on 
                            if (tmpMatchingDirection > 0)
                            {
                                //Make sure that the figure will put the king in check without a move


                                if (myKingPositionShort > attackingPosition)
                                {
                                    if (fig.Type == EFigures.Rook && ((attackDatabase.GetFieldsDown(myKingPositionShort) | attackDatabase.GetFieldsRight(myKingPositionShort)) & fig.Position) > 0)
                                    {
                                        //King would be in direct contact with this figure 
                                        kingInDanger = true;
                                    }
                                    else if (fig.Type == EFigures.Bishop)
                                    {

                                        if (((attackDatabase.GetFieldsDownLeft(myKingPositionShort) | attackDatabase.GetFieldsDownRight(myKingPositionShort)) & fig.Position) > 0)
                                        {
                                            //King is in direct contact with this figure 
                                            kingInDanger = true;
                                        }
                                    }
                                    else
                                    {
                                        //queen is attacker we have to build the movment 
                                        UInt64 queenLower = (attackDatabase.GetFieldsDownLeft(myKingPositionShort) | attackDatabase.GetFieldsDownRight(myKingPositionShort) |
                                                             attackDatabase.GetFieldsDown(myKingPositionShort) | attackDatabase.GetFieldsRight(myKingPositionShort));
                                        if ((queenLower & fig.Position) > 0)
                                        {
                                            //King is in direct contact with this figure 
                                            kingInDanger = true;
                                        }
                                    }
                                    if (kingInDanger)
                                    {
                                        //Remove the rubbish filds behind or in front of the figures (king and attacker)
                                        tmpMatchingDirection &= (~this.FillToPositionFromBottom(myKingPositionShort));
                                        tmpMatchingDirection &= (~this.FillToPositionFromTop(attackingPosition));
                                    }



                                }
                                else if (myKingPositionShort < attackingPosition)
                                {
                                    if (fig.Type == EFigures.Rook && ((attackDatabase.GetFieldsUP(myKingPositionShort) | attackDatabase.GetFieldsLeft(myKingPositionShort)) & myKingPosition) > 0)
                                    {
                                        //Kind would be in direct contact with this figure 
                                        kingInDanger = true;
                                    }
                                    else if (fig.Type == EFigures.Bishop)
                                    {
                                        //Build the move for a bishop
                                        if (((attackDatabase.GetFieldsUpLeft(myKingPositionShort) | attackDatabase.GetFieldsUpRight(myKingPositionShort)) & fig.Position) > 0)
                                        {
                                            //King is in direct contact with this figure 
                                            kingInDanger = true;
                                        }
                                    }
                                    else
                                    {
                                        //queen is attacker we have to build the movment 
                                        UInt64 queenLower = (attackDatabase.GetFieldsUpLeft(myKingPositionShort) | attackDatabase.GetFieldsUpRight(myKingPositionShort) |
                                                             attackDatabase.GetFieldsUP(myKingPositionShort) | attackDatabase.GetFieldsLeft(myKingPositionShort));
                                        if ((queenLower & fig.Position) > 0)
                                        {
                                            //King is in direct contact with this figure 
                                            kingInDanger = true;
                                        }
                                    }
                                    if (kingInDanger)
                                    {
                                        //If it is the other way remove the fields above the king and below the attacking figure
                                        tmpMatchingDirection &= (~this.FillToPositionFromBottom(attackingPosition));
                                        tmpMatchingDirection &= (~this.FillToPositionFromTop(myKingPositionShort));
                                    }



                                }


                                if (kingInDanger)
                                {
                                    //Only allow moves that match the fields we found ( between the king and the attacker)
                                    legalMoves = (legalMoves & tmpMatchingDirection);
                                }
                            }
                        }
                    }
                }
            }
            //Return the final moves for the figure at the given position
            return legalMoves;
        }
        
        /// <summary>
        /// Checks if the Castling move is valid for the provided King
        /// </summary>
        /// <param name="RookPosition">Rook that should be used for Castling</param>
        /// <param name="KingPosition">The Position of the King</param>
        /// <param name="AttackedByEnemy">All Fields attacked by the enemy player</param>
        /// <param name="Left">Is it the left or the right Rook</param>
        /// <param name="KingFields">The fields the king has to move to castel</param>
        /// <param name="CastlingTarget">The Target field for the king</param>
        /// <returns>Move mask in 64bit for the king or 0</returns>
        private UInt64 CastlingCheck(UInt64 RookPosition,short KingPosition,UInt64 AttackedByEnemy ,bool Left,UInt64 KingFields,UInt64 CastlingTarget, BitBoard CurrentBitBoard)
        {
            UInt64 result = 0;
            //check for the right rook and at the moves to the legal moves if the fields are not under attack
            //and not blockecd by any figure

            //Get all fields on the left of the king
            UInt64 workingBoard = Left ? this.attackDatabase.GetFieldsLeft(KingPosition) : this.attackDatabase.GetFieldsRight(KingPosition);
            //No figures except the rook on my left
            if ((workingBoard & CurrentBitBoard.SquarsBlocked) == RookPosition)
            {
                //Now we have to check if the king squares are under attack
                if ((AttackedByEnemy & KingFields) == 0)
                {
                    result |= CastlingTarget;
                }
            }
            return result;
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
                    this.attackDatabase.GetFieldsRight(Position) |
                    this.attackDatabase.GetFieldsUpLeft(Position) |
                    this.attackDatabase.GetFieldsUpRight(Position) |
                    this.attackDatabase.GetFieldsUP(Position));       
        }

        /// <summary>
        /// Sets all bits on the depending figure type
        /// </summary>
        /// <param name="Position">Position that is used as the center</param>
        /// <param name="Type">FigureType for movment</param>
        /// <returns>The resulting bitboard</returns>
        private UInt64 PinPosition(short Position,EFigures Type)
        {
            switch (Type)
            {
                case EFigures.Rook:
                    {
                        return (this.attackDatabase.GetFieldsDown(Position) |
                                this.attackDatabase.GetFieldsLeft(Position) |
                                this.attackDatabase.GetFieldsRight(Position) |
                                this.attackDatabase.GetFieldsUP(Position));
                    }
                    break;
                case EFigures.Bishop:
                    {
                        return (this.attackDatabase.GetFieldsDownLeft(Position) |
                                    this.attackDatabase.GetFieldsDownRight(Position) |
                                    this.attackDatabase.GetFieldsUpLeft(Position) |
                                    this.attackDatabase.GetFieldsUpRight(Position));

                    }
                    break;
                default:
                    { 
                       return (this.attackDatabase.GetFieldsDown(Position) |
                               this.attackDatabase.GetFieldsDownLeft(Position) |
                               this.attackDatabase.GetFieldsDownRight(Position) |
                               this.attackDatabase.GetFieldsLeft(Position) |
                               this.attackDatabase.GetFieldsRight(Position) |
                               this.attackDatabase.GetFieldsUpLeft(Position) |
                               this.attackDatabase.GetFieldsUpRight(Position) |
                               this.attackDatabase.GetFieldsUP(Position));       
                    
                    }
                    break;
            }
            
            

        }

        /// <summary>
        /// Calculate all Fields protected by the given Figure on the given Position
        /// </summary>
        /// <param name="FigureToCheck">The Figure that is used for the calculation</param>
        /// <param name="Position">Current Position of the Figure on the Board</param>
        /// <returns>Protected Fields by the selected Figure</returns>
        public UInt64 GetProtectedFields(Figure FigureToCheck, Int16 Position, BitBoard CurrentBoard)
        {
            UInt64 protectedSquares = 0;
            UInt64 friendlyAndEmpty = 0;
            //Get all non enemy squars
            if (FigureToCheck.Color == Defaults.WHITE)
            {
                friendlyAndEmpty = (CurrentBoard.WhitePieces | CurrentBoard.EmptySquares);
            }
            else 
            {
                friendlyAndEmpty = (CurrentBoard.BlackPieces | CurrentBoard.EmptySquares);
            }
            //For sliding figures we need different calculation
            if (FigureToCheck.Type == EFigures.Bishop || FigureToCheck.Type == EFigures.Rook || FigureToCheck.Type == EFigures.Queen)
            {
                //NOTE: use related method for figure and pass the friendlyAndEmpty board to it instead of the enemyAndEmpty ???
                if (FigureToCheck.Type == EFigures.Bishop)
                { 
                    protectedSquares = GetBishopMovesOn(Position,friendlyAndEmpty,CurrentBoard);
                }
                else if (FigureToCheck.Type == EFigures.Rook)
                { 
                    protectedSquares = GetRookMovesOn(Position,friendlyAndEmpty,CurrentBoard);
                }
                else
                {
                    protectedSquares = (GetRookMovesOn(Position, friendlyAndEmpty,CurrentBoard) | GetBishopMovesOn(Position, friendlyAndEmpty,CurrentBoard));
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
        public void MakeAMove(Figure FigureToMove, Int16 TargetPosition,BitBoard CurrentBoard)
        {
            //IDEA: Fire pre and after move events to hook them elsewhere ( Computer player, other calculations)

            //Add the current Bitboard object to a history to offer the possibility to revert a move
            this.history.AddHistory(this.currentGame, CurrentBoard);
            //If the bitboard is update inside the special move function no update is needed 
            if (HandleSpecialMoves(FigureToMove, TargetPosition, CurrentBoard))
            {
                //Change the related bitboards
                this.MoveFigure(FigureToMove, TargetPosition, CurrentBoard);
            }
            //Refresh the helper boards
            this.UpdateHelperBoards(CurrentBoard);
            //Check if a king is checkmate 
            if (CheckmateCheck(CurrentBoard))
            {
                this.GameRunning = false;
            }
        }


        /// <summary>
        /// Simulate a move with a figure without history or any changes on the game state
        /// </summary>
        /// <param name="FigureToMove">The figure that should be moved</param>
        /// <param name="TargetPosition">The target position for this figure</param>
        /// <param name="TargetBoard">If needed all calculation will be done on the provided board</param>
        /// <returns>The resulting Bitboard after the move</returns>
        public BitBoard SimulateAMove(Figure FigureToMove, Int16 TargetPosition,BitBoard TargetBoard=null)
        {
            //If nothing was provided copy over the current board
            if (TargetBoard == null)
            {
                TargetBoard = BitBoard.CopyFigureValues(this.currentGameState);
            }
            
            //If the bitboard is update inside the special move function no update is needed 
            if (HandleSpecialMoves(FigureToMove, TargetPosition,TargetBoard))
            {
                //Change the related bitboards
                this.MoveFigure(FigureToMove, TargetPosition,TargetBoard);
            }
            //Refresh the helper boards
            this.UpdateHelperBoards(TargetBoard);
            //Check if a king is checkmate 
            if (CheckmateCheck(TargetBoard))
            {
                this.GameRunning = false;
            }
            return TargetBoard;
        }

        /// <summary>
        /// This functions checks if a king is in checkmate and ends the game
        /// </summary>
        /// <returns>True if the game is over</returns>
        private bool CheckmateCheck(BitBoard CurrentBoard)
        {
            bool result = false;
            UInt64 kingMoves = this.attackDatabase.GetMoveMask((short)Tools.BitOperations.MostSigExponent(CurrentBoard.WhiteKing),
                                                               this.GetFigureAtPosition(CurrentBoard.WhiteKing,CurrentBoard));
            if ((kingMoves & CurrentBoard.AttackedByBlack) == kingMoves)
            {
                result = true;
                if (GameEnded != null)
                {
                    GameEnded.Invoke(this, new GameEndedEventArgs(Defaults.BLACK));
                }
                return result;
            }
            kingMoves = this.attackDatabase.GetMoveMask((short)Tools.BitOperations.MostSigExponent(CurrentBoard.BlackKing),
                                                               this.GetFigureAtPosition(CurrentBoard.BlackKing,CurrentBoard));
            if ((kingMoves & CurrentBoard.AttackedByWhite) == kingMoves)
            {
                result = true;
                if (GameEnded != null)
                {
                    GameEnded.Invoke(this, new GameEndedEventArgs(Defaults.WHITE));
                }
                return result;
            }
            return result;
        }


        /// <summary>
        /// This function takes care of pawn promotion , Castling and En passant
        /// </summary>
        /// <param name="FigureToMove">The figure that should be moved</param>
        /// <param name="TargetPosition">The target field in the short form 0-63</param>
        /// <returns>True if the bitboard needs to be updates, false if everything is done</returns>
        private bool HandleSpecialMoves(Figure FigureToMove, Int16 TargetPosition,BitBoard WorkingBoard)
        {
            //True means no calculation was done the normal update can do his work
            bool updateBitboard = true;
            //The target position of the moving figure in the "bitboard style"
            UInt64 targetPositionLong = (ulong)Math.Pow(2, TargetPosition);
            //Get current pawn position in short style
            short currentPos=(short)Tools.BitOperations.MostSigExponent(FigureToMove.Position);
            //Special moves are only realted to kings and pawns
            switch (FigureToMove.Type)
            {
                case EFigures.Pawn: {
                    //Pawns can use promotion or en passant
                    #region En Passant
                    //Check if en passant was used by a pawn
                    if ((FigureToMove.Color == Defaults.WHITE ? WorkingBoard.EnPassantBlack & targetPositionLong : WorkingBoard.EnPassantWhite & targetPositionLong) > 0)
                    { 
                        //The Pawn moved on one of the fields block the normal update
                        updateBitboard = false;
                        //Set the pawn to the moved field
                        if (FigureToMove.Color == Defaults.WHITE)
                        {
                            //Remove the current position from the bitboard
                            WorkingBoard.WhitePawns ^= FigureToMove.Position;
                            //Set the new position
                            WorkingBoard.WhitePawns |= targetPositionLong;
                            //Remove the enemy pawn
                            WorkingBoard.BlackPawns ^= (UInt64)Math.Pow(2, TargetPosition - 8);

                        }
                        else
                        {
                            //Remove the current position from the bitboard
                            WorkingBoard.BlackPawns ^= FigureToMove.Position;
                            //Set the new position
                            WorkingBoard.BlackPawns |= targetPositionLong;
                            //Remove the enemy pawn
                            WorkingBoard.WhitePawns ^= (UInt64)Math.Pow(2, TargetPosition + 8);
                        }

                    }
                    
                    //Check if pawn moved 2 fields from his starting position
                    if (Math.Abs(TargetPosition - currentPos) == 16)
                    {
                        //Pawn moved two fields so set the en passant bit for it
                        if (FigureToMove.Color == Defaults.WHITE)
                        {
                            //set the field that could be attacked in the next turn by a pawn
                            WorkingBoard.EnPassantWhite = (UInt64)Math.Pow(2, TargetPosition - 8);
                        }
                        else
                        {
                            //set the field that could be attacked in the next turn by a pawn
                            WorkingBoard.EnPassantBlack = (UInt64)Math.Pow(2, TargetPosition + 8);
                        }
                    }
                    else {
                        //Reset the field of the other color
                        if (FigureToMove.Color == Defaults.WHITE)
                        {
                            //set the field that could be attacked in the next turn by a pawn
                            WorkingBoard.EnPassantBlack = 0;
                        }
                        else
                        {
                            //set the field that could be attacked in the next turn by a pawn
                            WorkingBoard.EnPassantWhite = 0;
                        }
                    }
                    #endregion

                    #region Promotion
                    //Check the promotion case
                    if ((FigureToMove.Color == Defaults.BLACK && (targetPositionLong & Defaults.WhitePromotionRank) > 0) |
                        (FigureToMove.Color == Defaults.WHITE && (targetPositionLong & Defaults.BlackPromotionRank) > 0))
                    {
                        EFigures newFigureType = EFigures.NAN;
                        //The selected pawn will be promoted check if we have an handler 
                        if (FigureToMove.Color == Defaults.WHITE && this.promotionHandlerWhite != null)
                        {
                            newFigureType = this.promotionHandlerWhite.GetDecision();
                        }
                        else
                        { 
                            //No handler menas we take the queen
                            newFigureType = EFigures.Queen;
                        }
                        if (FigureToMove.Color == Defaults.BLACK && this.promotionHandlerBlack != null)
                        {
                            newFigureType = this.promotionHandlerBlack.GetDecision();
                        }
                        else
                        {
                            //No handler menas we take the queen
                            newFigureType = EFigures.Queen;
                        }
                        
                        //Remove the pawn and set the selected figure
                        if (FigureToMove.Color == Defaults.WHITE)
                        {
                            //Remove the current position from the bitboard
                            WorkingBoard.WhitePawns ^= FigureToMove.Position;
                        }
                        else
                        {
                            //Remove the current position from the bitboard
                            WorkingBoard.BlackPawns ^= FigureToMove.Position;
                        }
                    
                        //Check if we have to remove a figure at this position
                        if ((WorkingBoard.BlackPieces & targetPositionLong) > 0 || (WorkingBoard.WhitePieces & targetPositionLong) > 0)
                        {
                            //Remove the figure that is located at the new position
                            this.MakeAMove(this.GetFigureAtPosition((ulong)Math.Pow(2, TargetPosition),WorkingBoard), -1,WorkingBoard);
                        }

                        switch (newFigureType)
                        {
                            case EFigures.Rook:
                                {
                                    if (FigureToMove.Color == Defaults.WHITE)
                                    {
                                        //Set/Create the new figure
                                        WorkingBoard.WhiteRooks |= targetPositionLong;
                                    }
                                    else
                                    {
                                        //Remove the current position from the bitboard
                                        WorkingBoard.BlackRooks |= targetPositionLong;
                                    }
                                }break;
                            case EFigures.Bishop:
                                {
                                    if (FigureToMove.Color == Defaults.WHITE)
                                    {
                                        //Set/Create the new figure
                                        WorkingBoard.WhiteBishops |= targetPositionLong;
                                    }
                                    else
                                    {
                                        //Set/Create the new figure
                                        WorkingBoard.Blackbishops |= targetPositionLong;
                                    }
                                } break;
                            case EFigures.Knight:
                                {
                                    if (FigureToMove.Color == Defaults.WHITE)
                                    {
                                        //Set/Create the new figure
                                        WorkingBoard.WhiteKnights |= targetPositionLong;
                                    }
                                    else
                                    {
                                        //Set/Create the new figure
                                        WorkingBoard.BlackKnights |= targetPositionLong;
                                    }
                                } break;
                            case EFigures.Queen:
                                {
                                    if (FigureToMove.Color == Defaults.WHITE)
                                    {
                                        //Set/Create the new figure
                                        WorkingBoard.WhiteQueens |= targetPositionLong;
                                    }
                                    else
                                    {
                                        //Set/Create the new figure
                                        WorkingBoard.BlackQueens |= targetPositionLong;
                                    }
                                } break;
                            

                        }
                        updateBitboard = false;
                    }
#endregion

                }
                break;
                case EFigures.King:
                {
                    UInt64 targetPosition = (UInt64)Math.Pow(2, TargetPosition);
                    if (FigureToMove.Color == Defaults.WHITE)
                    {
                        if (!WorkingBoard.WhiteKingMoved)
                        {
                            WorkingBoard.WhiteKingMoved = true;

                            if ((targetPosition & Defaults.CastlingWhiteLeft) > 0 || (Defaults.CastlingWhiteRight & targetPosition) > 0)
                            {
                                //The king moved on a casteling field also move the related rook
                                //Check for the right rook to move 
                                if ((targetPosition & Defaults.CastlingWhiteLeft) > 0)
                                {
                                    if (!WorkingBoard.WhiteLeftRookMoved)
                                    {
                                        MoveFigure(this.GetFigureAtPosition(Defaults.WhiteLeftRookStartPosition,WorkingBoard), 4,WorkingBoard);
                                        WorkingBoard.WhiteLeftRookMoved = true;  
                                    }
                                }
                                else
                                {
                                    if (!WorkingBoard.WhiteRightRookMoved)
                                    {
                                        MoveFigure(this.GetFigureAtPosition(Defaults.WhiteRightRookStartPosition,WorkingBoard), 2,WorkingBoard);
                                        WorkingBoard.WhiteRightRookMoved = true;  
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!WorkingBoard.BalckKingMoved)
                        {
                            WorkingBoard.BalckKingMoved = true;
                            if ((targetPosition & Defaults.CastlingBlackLeft) > 0 || (Defaults.CastlingBlackRight & targetPosition) > 0)
                            {
                                //The king moved on a casteling field also move the related rook
                                if ((targetPosition & Defaults.CastlingBlackLeft) > 0)
                                {
                                    //Move the rook to the position
                                    if (!WorkingBoard.BlackLeftRookMoved)
                                    {
                                        WorkingBoard.BlackLeftRookMoved = true;
                                        MoveFigure(this.GetFigureAtPosition(Defaults.BlackLeftRookStartPosition,WorkingBoard), 60,WorkingBoard);
                                    }
                                }
                                else
                                {
                                    if (!WorkingBoard.BlackRightRookMoved)
                                    {
                                        WorkingBoard.BlackRightRookMoved = true;
                                        MoveFigure(this.GetFigureAtPosition(Defaults.BlackRightRookStartPosition,WorkingBoard), 58,WorkingBoard);
                                    }
                                }

                            }
                        }
                    }
                    

                }
                break;
                case EFigures.Rook:
                {
                    if (FigureToMove.Color == Defaults.WHITE)
                    {
                        if ((FigureToMove.Position & Defaults.WhiteRightRookStartPosition) > 0)
                        {
                            WorkingBoard.WhiteRightRookMoved = true;
                        }
                        else {
                            WorkingBoard.WhiteLeftRookMoved = true;    
                        }
                    }
                    else
                    {
                        if ((FigureToMove.Position & Defaults.BlackRightRookStartPosition) > 0)
                        {
                            WorkingBoard.BlackRightRookMoved = true;
                        }
                        else
                        {
                            WorkingBoard.BlackLeftRookMoved = true;
                        }

                    }                                        
                }
                break;
            }

            return updateBitboard;       
        }

        /// <summary>
        /// Move a Figure to the provided Position. This function just sets and removes the bits in the current bitboard.
        /// </summary>
        /// <param name="FigureToMove">Figure that should be moved to a new position</param>
        /// <param name="TargetPosition">The new position of the figure</param>
        private void MoveFigure(Figure FigureToMove, Int16 TargetPosition,BitBoard WorkingBoard)
        {
            // The target figure is only important if this is not a "remove move" ( TargetPosition > 0)
            Figure targetFigure = null;
            //By default we do not set the figure on a new position currentBoard |= 0 does not change a bit ;)
            ulong calculationBoard =0; 
            if (TargetPosition >= 0)
            {
                //Set the target bit to 1 so we can use it in our calculation
                calculationBoard = (ulong)Math.Pow(2, TargetPosition);
                //Get the figure at the target square 
                targetFigure = this.GetFigureAtPosition((ulong)Math.Pow(2, TargetPosition),WorkingBoard); 
            }
            //Depending on the figure we have to move set the matching bitboard
            switch (FigureToMove.Type)
            {
                case EFigures.Bishop:
                    {
                        if (FigureToMove.Color == Defaults.WHITE)
                        {
                            //Remove the current position from the bitboard
                            WorkingBoard.WhiteBishops ^= FigureToMove.Position;
                            //Set the new position
                            WorkingBoard.WhiteBishops |= calculationBoard; 
                        }
                        else 
                        {
                            //Remove the current position from the bitboard
                            WorkingBoard.Blackbishops ^= FigureToMove.Position;
                            //Set the new position
                            WorkingBoard.Blackbishops |= calculationBoard; 
                        }
                    }
                    break;
                case EFigures.Knight: 
                    {
                        if (FigureToMove.Color == Defaults.WHITE)
                        {
                            //Remove the current position from the bitboard
                            WorkingBoard.WhiteKnights ^= FigureToMove.Position;
                            //Set the new position
                            WorkingBoard.WhiteKnights |= calculationBoard;
                        }
                        else
                        {
                            //Remove the current position from the bitboard
                            WorkingBoard.BlackKnights ^= FigureToMove.Position;
                            //Set the new position
                            WorkingBoard.BlackKnights |= calculationBoard;
                        }
                    }
                    break;
                case EFigures.Pawn: 
                    {
                        if (FigureToMove.Color == Defaults.WHITE)
                        {
                            //Remove the current position from the bitboard
                            WorkingBoard.WhitePawns ^= FigureToMove.Position;
                            //Set the new position
                            WorkingBoard.WhitePawns |= calculationBoard;
                        }
                        else
                        {
                            //Remove the current position from the bitboard
                            WorkingBoard.BlackPawns ^= FigureToMove.Position;
                            //Set the new position
                            WorkingBoard.BlackPawns |= calculationBoard;
                        }
                    }
                    break;
                case EFigures.Queen: 
                    {
                        if (FigureToMove.Color == Defaults.WHITE)
                        {
                            //Remove the current position from the bitboard
                            WorkingBoard.WhiteQueens ^= FigureToMove.Position;
                            //Set the new position
                            WorkingBoard.WhiteQueens |= calculationBoard;
                        }
                        else
                        {
                            //Remove the current position from the bitboard
                            WorkingBoard.BlackQueens ^= FigureToMove.Position;
                            //Set the new position
                            WorkingBoard.BlackQueens |= calculationBoard;
                        }
                    }
                    break;
                case EFigures.Rook:
                    {
                        if (FigureToMove.Color == Defaults.WHITE)
                        {
                            //Remove the current position from the bitboard
                            WorkingBoard.WhiteRooks ^= FigureToMove.Position;
                            //Set the new position
                            WorkingBoard.WhiteRooks |= calculationBoard;
                        }
                        else
                        {
                            //Remove the current position from the bitboard
                            WorkingBoard.BlackRooks ^= FigureToMove.Position;
                            //Set the new position
                            WorkingBoard.BlackRooks |= calculationBoard;
                        }
                    }
                    break;
                case EFigures.King:
                    {
                        if (FigureToMove.Color == Defaults.WHITE)
                        {
                            //Remove the current position from the bitboard
                            WorkingBoard.WhiteKing ^= FigureToMove.Position;
                            //Set the new position
                            WorkingBoard.WhiteKing |= calculationBoard;
                        }
                        else
                        {
                            //Remove the current position from the bitboard
                            WorkingBoard.BlackKing ^= FigureToMove.Position;
                            //Set the new position
                            WorkingBoard.BlackKing |= calculationBoard;
                        }
                    }
                    break;
            }
            
            //If the move attacks a figure remove it from the board
            if (targetFigure != null)
            {
                //Call the function again with the target figure to remove it from the board
                this.MoveFigure(targetFigure, -1,WorkingBoard);
            }
        }
        
        /// <summary>
        /// Calculate all possible moves for the Rook on the given Position
        /// </summary>
        /// <param name="Position">Position of the Rook</param>
        /// <param name="EnemyAndEmpty">Representaion of all enemies and empty fields</param>
        /// <returns>Legal Possible moves</returns>
        private UInt64 GetRookMovesOn(Int16 Position, UInt64 EnemyAndEmpty,BitBoard CurrentBoard)
        {
            UInt64 legalMoves = 0;
            //Get all blocking pices to the right of this figure
            UInt64 currentboard = this.attackDatabase.GetFieldsRight(Position);
            //Set all bits to 1 if a figure on the row (friendly or enemy)
            UInt64 currentmoves = currentboard & CurrentBoard.SquarsBlocked;
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
            currentmoves = currentboard & CurrentBoard.SquarsBlocked;
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
            currentmoves = currentboard & CurrentBoard.SquarsBlocked;
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
            currentmoves = currentboard & CurrentBoard.SquarsBlocked;
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
        private UInt64 GetBishopMovesOn(Int16 Position, UInt64 EnemyAndEmpty, BitBoard CurrentBoard)
        {
            UInt64 legalMoves = 0;
            //Get all blocking pices to the right of this figure
            UInt64 currentboard = this.attackDatabase.GetFieldsUpRight(Position);
            //Set all bits to 1 if a figure on the row (friendly or enemy)
            UInt64 currentmoves = currentboard & CurrentBoard.SquarsBlocked;
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
            currentmoves = currentboard & CurrentBoard.SquarsBlocked;
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
            currentmoves = currentboard & CurrentBoard.SquarsBlocked;
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
            currentmoves = currentboard & CurrentBoard.SquarsBlocked;
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
        public Figure GetFigureAtPosition(UInt64 Position, BitBoard CurrentBoard)
        {
            Figure returnValue = null;
            if (gameRunning)
            {
                UInt64 result = Position & CurrentBoard.SquarsBlocked;
                if (result != 0)
                {
                    result = Position & CurrentBoard.BlackPieces;
                    if (result != 0)
                    {
                        //Black Figure
                        if ((CurrentBoard.BlackKing & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.BLACK, EFigures.King);
                        }
                        if ((CurrentBoard.BlackQueens & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.BLACK, EFigures.Queen);
                        }
                        if ((CurrentBoard.BlackRooks & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.BLACK, EFigures.Rook);
                        }
                        if ((CurrentBoard.Blackbishops & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.BLACK, EFigures.Bishop);
                        }
                        if ((CurrentBoard.BlackKnights & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.BLACK, EFigures.Knight);
                        }
                        if ((CurrentBoard.BlackPawns & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.BLACK, EFigures.Pawn);
                        }
                    }
                    else
                    {
                        //White Figure
                        if ((CurrentBoard.WhiteKing & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.WHITE, EFigures.King);
                        }
                        if ((CurrentBoard.WhiteQueens & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.WHITE, EFigures.Queen);
                        }
                        if ((CurrentBoard.WhiteRooks & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.WHITE, EFigures.Rook);
                        }
                        if ((CurrentBoard.WhiteBishops & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.WHITE, EFigures.Bishop);
                        }
                        if ((CurrentBoard.WhiteKnights & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.WHITE, EFigures.Knight);
                        }
                        if ((CurrentBoard.WhitePawns & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.WHITE, EFigures.Pawn);
                        }
                    }
                }
                if (returnValue != null)
                {
                    //Set the current position of the figure to speed up calculations later
                    returnValue.Position = Position;
                }
            }
            //Return the result 
            return returnValue;
        }
      
        /// <summary>
        /// Returns an Array of Figures that are Protecting the Provided Fields for the given Color
        /// </summary>
        /// <param name="SearchValues">Fields to be checked</param>
        /// <param name="Color">Only this Color will be checked</param>
        /// <returns>Array of Figures that are matching the search creteria</returns>
        public Figure[] GetProtectingFigures(UInt64 SearchValues, int Color,BitBoard CurrentBoard)
        {
            //list with figures that protected the passed search mask
            List<Figure> protectors = new List<Figure>();
            //Loop to the protection list
            foreach (UInt64 Key in CurrentBoard.ProtecteddBy.Keys)
            {
                if ((Key & SearchValues) > 0)
                { 
                    //Add the searched figures to our temp. storage
                    foreach (Figure fig in CurrentBoard.ProtecteddBy[Key].Where(F => F.Color == Color))
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
        public UInt64 IsProtected(UInt64 SearchValues, int Color,BitBoard CurrentBoard)
        {
            UInt64 result = 0;
            //Loop to the protection list
            foreach (UInt64 Key in CurrentBoard.ProtecteddBy.Keys)
            {
                if ((Key & SearchValues) > 0)
                {
                    //Set bits if we find figures for the given color
                    foreach (Figure fig in CurrentBoard.ProtecteddBy[Key].Where(F => F.Color == Color))
                    {
                        result |= Key;
                        break;
                    }
                }
            }
            
            return result;
        }

        /// <summary>
        /// Sets all bits to 1 until it reachs the stop position starting at 0
        /// </summary>
        /// <param name="StopPosition">Position to stop 1 to 64</param>
        /// <param name="includePosition">If true the stopposition will be set 0 also</param>
        /// <returns>Filled bitboard based on the provided Position</returns>
        public UInt64 FillToPositionFromBottom(Int16 StopPosition, bool excludePosition = false)
        {
            UInt64 returnValue = 0;
            StopPosition += (short)(excludePosition ? 1 : 0);
            for (Int16 index = 0; index < StopPosition; ++index)
            {
                returnValue |= (UInt64)Math.Pow(2, index);
            }
            return returnValue;
        }


        /// <summary>
        /// Sets all bits to 1 until it reachs the stop position starting at 63
        /// </summary>
        /// <param name="StopPosition">Position to stop 1 to 64</param>
        /// <param name="includePosition">If true the stopposition will be set 0 also</param>
        /// <returns>Filled bitboard based on the provided Position</returns>
        public UInt64 FillToPositionFromTop(Int16 StopPosition, bool excludePosition = false)
        {
            UInt64 returnValue = 0;
            StopPosition -= (short)(excludePosition ? 1 : 0);
            for (Int16 index = 63; index > StopPosition; --index)
            {
                returnValue |= (UInt64)Math.Pow(2, index);
            }
            return returnValue;
        }
    }
}

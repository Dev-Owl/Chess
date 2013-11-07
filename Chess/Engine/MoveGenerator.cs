using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Engine
{
    public class MoveGenerator
    {
        BitBoard currentGameState;
        public BitBoard CurrentGameState
        {
            get { return currentGameState; }
            set { currentGameState = value; }
        }

        AttackDatabase attackDatabase;

        public AttackDatabase AttackDatabase
        {
            get { return attackDatabase; }
            set { attackDatabase = value; }
        }

        bool gameRunning = false;

        public bool GameRunning
        {
            get { return gameRunning; }
            set { gameRunning = value; }
        }




        public MoveGenerator()
        {
            this.attackDatabase = new AttackDatabase();
            this.currentGameState = new BitBoard();
        }

        public MoveGenerator(BitBoard CurrentState)
        {
            this.attackDatabase = new AttackDatabase();
            this.currentGameState = CurrentState;
        }

        public void NewGame()
        {
            this.currentGameState.WhiteKing = Defaults.WhiteKing;
            this.currentGameState.WhiteQueens = Defaults.WhiteQueens;
            this.currentGameState.WhiteRooks = Defaults.WhiteRooks;
            this.currentGameState.WhiteBishops = Defaults.WhiteBishops;
            this.currentGameState.WhiteKnights = Defaults.WhiteKnights;
            this.currentGameState.WhitePawns = Defaults.WhitePawns;
            this.currentGameState.WhitePieces = this.currentGameState.WhiteKing | this.currentGameState.WhiteQueens | this.currentGameState.WhiteRooks | this.currentGameState.WhiteBishops
                               | this.currentGameState.WhiteKnights | this.currentGameState.WhitePawns;
            this.currentGameState.BlackKing = Defaults.BlackKing;
            this.currentGameState.BlackQueens = Defaults.BlackQueens;
            this.currentGameState.BlackRooks = Defaults.BlackRooks;
            this.currentGameState.Blackbishops = Defaults.Blackbishops;
            this.currentGameState.BlackKnights = Defaults.BlackKnights;
            this.currentGameState.BlackPawns = Defaults.BlackPawns;
            this.currentGameState.BlackPieces = this.currentGameState.BlackKing | this.currentGameState.BlackQueens | this.currentGameState.BlackRooks | this.currentGameState.Blackbishops
                              | this.currentGameState.BlackKnights | this.currentGameState.BlackPawns;
            this.currentGameState.SquarsBlocked = this.currentGameState.WhitePieces | this.currentGameState.BlackPieces;
            this.currentGameState.EmptySquares = ~this.currentGameState.SquarsBlocked;
            this.UpdateAttackedFields();
            this.GameRunning = true;
        }

        private void UpdateAttackedFields()
        {
            //Helper variable and temp. storage
            UInt64 tmpMoves = 0;
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
            this.currentGameState.AttackedBy
            for (UInt16 i = 0; i < 64; ++i)
            {
                tmpMoves = 0;
                //Get figure on the selected board position
                Figure fig = this.GetFigureAtPosition(position); ?????
                //Get the protected figures squares

                //Only go on if we found a figure
                if (fig != null)
                {
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
                    }
                }
                //Jump to the next position
                position = (position << 1);
            }
        }

        public UInt64 GetMoveForFigure(Figure FigureToCheck, Int16 Position)
        {
            //Get all possible moves for this figure at the givin position
            UInt64 legalMoves = attackDatabase.GetMoveMask(Position, FigureToCheck);
            UInt64 enemyOrEmpty = this.currentGameState.EmptySquares; //all enemies or empty squares
            UInt64 enemy = 0; //All figs of the current enemy color
            UInt64 enemyAttacked = 0;
           

            if (FigureToCheck.Color == Defaults.WHITE)
            {
                enemyOrEmpty |= this.currentGameState.BlackPieces;
                enemy = this.currentGameState.BlackPieces;
                enemyAttacked = this.currentGameState.AttackedByBlack;
            }
            else
            {
                enemyOrEmpty |= this.currentGameState.WhitePieces;
                enemy = this.currentGameState.WhitePieces;
                enemyAttacked = this.currentGameState.AttackedByWhite;
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
                    }
                    break;

            }


            //IDEA: For king check detection
            //1. Check if an enemy figure is atticking current figure
            //2. Check if king is on row,column or 45 °degres angle the first figure from this fig
            //3. Get the figures that are attacking this figure
            //4. ?????
            return legalMoves;
        }

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
            if (FigureToCheck.Type != EFigures.Bishop || FigureToCheck.Type != EFigures.Rook || FigureToCheck.Type != EFigures.Queen)
            {
                //NOTE: use related method for figure and pass the friendlyAndEmpty board to it instead of the enemyAndEmpty ???
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

        public UInt64 GetPositionFromPosition(Int16 SourcePosition, Int16 MoveRange)
        {
            UInt64 result = 0;
            result |= (UInt64)Math.Pow(2, SourcePosition + MoveRange);
            return result;
        }

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
            return returnValue;
        }
    }
}

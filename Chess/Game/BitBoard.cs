using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chess.Tools;
using System.Drawing;

namespace Chess.Game
{
   public class BitBoard :IDisposable
   {

        #region Figures
        private UInt64 whiteKing;
        public UInt64 WhiteKing
        {
            get { return whiteKing; }
            set { whiteKing = value; }
        }
        private UInt64 whiteQueens;
        public UInt64 WhiteQueens
        {
            get { return whiteQueens; }
            set { whiteQueens = value; }
        }
        private UInt64 whiteRooks;
        public UInt64 WhiteRooks
        {
            get { return whiteRooks; }
            set { whiteRooks = value; }
        }
        private UInt64 whiteBishops;
        public UInt64 WhiteBishops
        {
            get { return whiteBishops; }
            set { whiteBishops = value; }
        }
        private UInt64 whiteKnights;
        public UInt64 WhiteKnights
        {
            get { return whiteKnights; }
            set { whiteKnights = value; }
        }
        private UInt64 whitePawns;
        public UInt64 WhitePawns
        {
            get { return whitePawns; }
            set { whitePawns = value; }
        }
        private UInt64 whitePieces;
        public UInt64 WhitePieces
        {
            get { return whitePieces; }
            set { whitePieces = value; }
        }
        private UInt64 blackKing;
        public UInt64 BlackKing
        {
            get { return blackKing; }
            set { blackKing = value; }
        }
        private UInt64 blackQueens;
        public UInt64 BlackQueens
        {
            get { return blackQueens; }
            set { blackQueens = value; }
        }
        private UInt64 blackRooks;
        public UInt64 BlackRooks
        {
            get { return blackRooks; }
            set { blackRooks = value; }
        }
        private UInt64 blackbishops;
        public UInt64 Blackbishops
        {
            get { return blackbishops; }
            set { blackbishops = value; }
        }
        private UInt64 blackKnights;
        public UInt64 BlackKnights
        {
            get { return blackKnights; }
            set { blackKnights = value; }
        }
        private UInt64 blackPawns;
        public UInt64 BlackPawns
        {
            get { return blackPawns; }
            set { blackPawns = value; }
        }
        private UInt64 blackPieces;
        public UInt64 BlackPieces
        {
            get { return blackPieces; }
            set { blackPieces = value; }
        }
        #endregion

        #region Helping Boards
        private UInt64 squarsBlocked;
        public UInt64 SquarsBlocked
        {
            get { return squarsBlocked; }
            set { squarsBlocked = value; }
        }
        private UInt64 emptySquares;
        public UInt64 EmptySquares
        {
            get { return emptySquares; }
            set { emptySquares = value; }
        }
        private UInt64 attackedByWhite;
        public UInt64 AttackedByWhite
        {
            get { return this.attackedByWhite; }
            set { this.attackedByWhite = value; }
        }
        private UInt64 attackedByBlack;
        public UInt64 AttackedByBlack
        {
            get { return this.attackedByBlack; }
            set { this.attackedByBlack = value; }
        }
        private Dictionary<UInt64, List<Figure>> attackedBy;
        public Dictionary<UInt64, List<Figure>> AttackedBy
        {
            get { return attackedBy; }
            set { attackedBy = value; }
        }
        #endregion

        bool gameRunning = false;
        public bool GameRunning
        {
            get;
            set;
        }

        AttackDatabase db;
       
       public BitBoard()
       {
           db = new AttackDatabase();
           this.attackedBy = new Dictionary<ulong, List<Figure>>();
           UInt64 position = 1;
           for (int index = 0; index < 64; ++index)
           {
               this.attackedBy.Add(position, new List<Figure>());
               position = (position << 1);    
           }
       }

       ~BitBoard()
       {
           this.Dispose();
       }

       public void NewGame()
       {
           this.WhiteKing = Defaults.WhiteKing;
           this.WhiteQueens = Defaults.WhiteQueens; 
           this.WhiteRooks = Defaults.WhiteRooks;
           this.WhiteBishops = Defaults.WhiteBishops;
           this.WhiteKnights = Defaults.WhiteKnights;
           this.WhitePawns = Defaults.WhitePawns;
           this.WhitePieces = this.WhiteKing | this.WhiteQueens | this.WhiteRooks | this.WhiteBishops
                              | this.WhiteKnights | this.WhitePawns;
           this.BlackKing = Defaults.BlackKing;
           this.BlackQueens = Defaults.BlackQueens;
           this.BlackRooks = Defaults.BlackRooks;
           this.Blackbishops = Defaults.Blackbishops;
           this.BlackKnights = Defaults.BlackKnights;
           this.BlackPawns = Defaults.BlackPawns;
           this.BlackPieces = this.BlackKing | this.BlackQueens | this.BlackRooks | this.Blackbishops
                             | this.BlackKnights | this.BlackPawns;
           this.SquarsBlocked = whitePieces | BlackPieces; 
           this.emptySquares = ~this.squarsBlocked;
           this.gameRunning = true;
           UpdateAttackedFields();
       }

       private void UpdateAttackedFields()
       {
           UInt64 tmpMoves = 0;
           UInt64 position = 1;
           for (UInt16 i = 0; i < 64; ++i)
           {
               tmpMoves = 0;
               Figure fig = GetFigureAtPosition(position);
               if (fig != null)
               {   
                   // Pawns moves are not the attacks
                   if (fig.Type != EFigures.Pawn)
                   {   
                       //Moves for the figure
                       tmpMoves = GetMoveForFigure(fig, (short)i);
                   }
                   else {
                       //calculate possible pawn attacks
                       UInt64 enemyAndEmpy =0;
                       if(fig.Color == Defaults.BLACK)
                       {
                            enemyAndEmpy = this.whitePieces | this.emptySquares;
                       }
                       else
                       {
                           enemyAndEmpy = this.blackPieces | this.emptySquares;
                       }
                       tmpMoves = enemyAndEmpy & this.db.BuildPawnAttack((short)i, fig.Color);
                   }
                   //Update helper boards for the figure color
                   if (fig.Color == Defaults.WHITE)
                   {
                       this.attackedByWhite |= tmpMoves;
                   }
                   else
                   {
                       this.attackedByBlack |= tmpMoves;
                   }
                   //Update the attacked by status for the different fields 
                   foreach (UInt64 key in this.attackedBy.Keys)
                   {
                       if ( (key & tmpMoves) > 0)
                       {
                           //To get the figure position later faster we store it inside the figure object
                           fig.Position = position;
                           this.attackedBy[key].Add(fig);
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
           UInt64 legalMoves =  db.GetMoveMask(Position, FigureToCheck);
           UInt64 enemyOrEmpty = this.emptySquares; //all enemies or empty squares
           UInt64 enemy = 0; //All figs of the current enemy color
           UInt64 enemyAttacked =0;
           //Get current x,y position
           Point CurrentPoistion = Chess.GUI.DrawHelper.PositionMatrix(Position);
           
           if (FigureToCheck.Color == Defaults.WHITE)
           {
               enemyOrEmpty |= this.blackPieces;
               enemy = this.blackPieces;
               enemyAttacked = this.attackedByBlack;
           }
           else
           {
               enemyOrEmpty |= this.whitePieces;
               enemy = this.whitePieces;
               enemyAttacked = this.attackedByWhite;
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
                        if ((GetPositionFromPosition(Position, (Int16)(8 * FigureToCheck.Color)) & squarsBlocked) > 0)
                        {
                            legalMoves = 0;
                        }
                        //Check if an attack is possible use attack datbase BuildPawnAttack function
                        legalMoves |= enemy & db.BuildPawnAttack(Position, FigureToCheck.Color);
                    }break;
               case EFigures.Rook:
                   {
                       //Get all legal moves for the rook
                       legalMoves = GetRookMovesOn(Position,enemyOrEmpty);
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

       //Check if the own King is in check and if yes check how this figure can move to solve it
       private UInt64 CheckKingStatus(Figure CurrentFig, Int16 Position)
       {
           return 0;
       }

       private UInt64 GetRookMovesOn(Int16 Position, UInt64 EnemyAndEmpty)
       {
           UInt64 legalMoves = 0;
           //Get all blocking pices to the right of this figure
           UInt64 currentboard = this.db.GetFieldsRight(Position);
           //Set all bits to 1 if a figure on the row (friendly or enemy)
           UInt64 currentmoves = currentboard & this.squarsBlocked;
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

           currentboard = this.db.GetFieldsLeft(Position);
           //Set all bits to 1 if a figure on the row (friendly or enemy)
           currentmoves = currentboard & this.squarsBlocked;
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

           currentboard = this.db.GetFieldsUP(Position);
           //Set all bits to 1 if a figure on the row (friendly or enemy)
           currentmoves = currentboard & this.squarsBlocked;
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

           currentboard = this.db.GetFieldsDown(Position);
           //Set all bits to 1 if a figure on the row (friendly or enemy)
           currentmoves = currentboard & this.squarsBlocked;
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
           UInt64 currentboard = this.db.GetFieldsUpRight(Position);
           //Set all bits to 1 if a figure on the row (friendly or enemy)
           UInt64 currentmoves = currentboard & this.squarsBlocked;
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
           currentboard = this.db.GetFieldsDownRight(Position);
           //Set all bits to 1 if a figure on the row (friendly or enemy)
           currentmoves = currentboard & this.squarsBlocked;
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
           currentboard = this.db.GetFieldsDownLeft(Position);
           //Set all bits to 1 if a figure on the row (friendly or enemy)
           currentmoves = currentboard & this.squarsBlocked;
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
           currentboard = this.db.GetFieldsUpLeft(Position);
           //Set all bits to 1 if a figure on the row (friendly or enemy)
           currentmoves = currentboard & this.squarsBlocked;
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
           result |= (UInt64)Math.Pow(2, SourcePosition+MoveRange);
           return result;
       }
       
       public Figure GetFigureAtPosition(UInt64 Position)
        {
            Figure returnValue = null;
            if (gameRunning)
            {
                UInt64 result = Position & this.squarsBlocked;
                if (result != 0)
                {
                    result = Position & this.blackPieces;
                    if (result != 0)
                    {
                        //Black Figure
                        if ((this.blackKing & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.BLACK, EFigures.King);
                        }
                        if ((this.BlackQueens & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.BLACK, EFigures.Queen);
                        }
                        if ((this.BlackRooks & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.BLACK, EFigures.Rook);
                        }
                        if ((this.Blackbishops & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.BLACK, EFigures.Bishop);
                        }
                        if ((this.BlackKnights & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.BLACK, EFigures.Knight);
                        }
                        if ((this.BlackPawns & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.BLACK, EFigures.Pawn);
                        }
                    }
                    else
                    {
                        //White Figure
                        if ((this.whiteKing & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.WHITE, EFigures.King);
                        }
                        if ((this.whiteQueens & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.WHITE, EFigures.Queen);
                        }
                        if ((this.whiteRooks & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.WHITE, EFigures.Rook);
                        }
                        if ((this.whiteBishops & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.WHITE, EFigures.Bishop);
                        }
                        if ((this.whiteKnights & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.WHITE, EFigures.Knight);
                        }
                        if ((this.whitePawns & Position) > 0)
                        {
                            returnValue = new Figure(Defaults.WHITE, EFigures.Pawn);
                        }
                    }
                }
            }
            return returnValue;
        }

       public void Dispose()
       {
           this.db.Dispose();
           this.db = null;
           GC.SuppressFinalize(this);
       }
   }
}

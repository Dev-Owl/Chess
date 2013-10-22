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
        #endregion
       /// <summary>
       /// Dictionary<FigurColog,Dictionary<FigureType,List<Bitboards>>>
       /// </summary>
        private Dictionary<int, Dictionary<int, List<UInt64>>> rotatedBoards;


       AttackDatabase db;
       
       public BitBoard()
       {
           db = new AttackDatabase();
           rotatedBoards = new Dictionary<int, Dictionary<int, List<ulong>>>();
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
           this.BlackPawns = 0xFE000000020000;//Defaults.BlackPawns;
           this.BlackPieces = this.BlackKing | this.BlackQueens | this.BlackRooks | this.Blackbishops
                             | this.BlackKnights | this.BlackPawns;
           this.SquarsBlocked = whitePieces | BlackPieces; 
           this.emptySquares = ~this.squarsBlocked;
           //Create lists for each color
           this.rotatedBoards.Add(Defaults.BLACK, new Dictionary<int, List<ulong>>());
           this.rotatedBoards.Add(Defaults.WHITE, new Dictionary<int, List<ulong>>());

           this.CreateNewRotatedEntry(Defaults.BLACK, Defaults.ROTATED_KING);
           this.CreateNewRotatedEntry(Defaults.BLACK, Defaults.ROTATED_QUEEN);
           this.CreateNewRotatedEntry(Defaults.BLACK, Defaults.ROTATED_ROOKS);
           this.CreateNewRotatedEntry(Defaults.BLACK, Defaults.ROTATED_PAWN);
           this.CreateNewRotatedEntry(Defaults.BLACK, Defaults.ROTATED_BISHOPS);
           this.CreateNewRotatedEntry(Defaults.BLACK, Defaults.ROTATED_KNIGHT);
           this.CreateNewRotatedEntry(Defaults.BLACK, Defaults.ROTATED_ALL);

           this.CreateNewRotatedEntry(Defaults.WHITE, Defaults.ROTATED_KING);
           this.CreateNewRotatedEntry(Defaults.WHITE, Defaults.ROTATED_QUEEN);
           this.CreateNewRotatedEntry(Defaults.WHITE, Defaults.ROTATED_ROOKS);
           this.CreateNewRotatedEntry(Defaults.WHITE, Defaults.ROTATED_PAWN);
           this.CreateNewRotatedEntry(Defaults.WHITE, Defaults.ROTATED_BISHOPS);
           this.CreateNewRotatedEntry(Defaults.WHITE, Defaults.ROTATED_KNIGHT);
           this.CreateNewRotatedEntry(Defaults.WHITE, Defaults.ROTATED_ALL);
           
           
           this.rotatedBoards[Defaults.BLACK][Defaults.ROTATED_KING].Add(Defaults.Rotated90ClockWise_BlackKing);
           this.rotatedBoards[Defaults.BLACK][Defaults.ROTATED_QUEEN].Add(Defaults.Rotated90ClockWise_BlackQueens);
           this.rotatedBoards[Defaults.BLACK][Defaults.ROTATED_ROOKS].Add(Defaults.Rotated90ClockWise_BlackRooks);
           this.rotatedBoards[Defaults.BLACK][Defaults.ROTATED_PAWN].Add(Defaults.Rotated90ClockWise_BlackPawn);
           this.rotatedBoards[Defaults.BLACK][Defaults.ROTATED_BISHOPS].Add(Defaults.Rotated90ClockWise_BlackBishops);
           this.rotatedBoards[Defaults.BLACK][Defaults.ROTATED_KNIGHT].Add(Defaults.Rotated90ClockWise_BlackKnights);
           this.rotatedBoards[Defaults.BLACK][Defaults.ROTATED_ALL].Add(Defaults.Rotated90ClockWise_BlackPieces);

           this.rotatedBoards[Defaults.WHITE][Defaults.ROTATED_KING].Add(Defaults.Rotated90ClockWise_WhiteKing);
           this.rotatedBoards[Defaults.WHITE][Defaults.ROTATED_QUEEN].Add(Defaults.Rotated90ClockWise_WhiteQueens);
           this.rotatedBoards[Defaults.WHITE][Defaults.ROTATED_ROOKS].Add(Defaults.Rotated90ClockWise_WhiteRooks);
           this.rotatedBoards[Defaults.WHITE][Defaults.ROTATED_PAWN].Add(Defaults.Rotated90ClockWise_WhitePawn);
           this.rotatedBoards[Defaults.WHITE][Defaults.ROTATED_BISHOPS].Add(Defaults.Rotated90ClockWise_WhiteBishops);
           this.rotatedBoards[Defaults.WHITE][Defaults.ROTATED_KNIGHT].Add(Defaults.Rotated90ClockWise_WhiteKnights);
           this.rotatedBoards[Defaults.WHITE][Defaults.ROTATED_ALL].Add(Defaults.Rotated90ClockWise_WhitePieces);

       }

       private void CreateNewRotatedEntry(int Color, int FigureType)
       {
           this.rotatedBoards[Color].Add(FigureType, new List<ulong>());
       }

       public UInt64 GetBitBoardRotated(int Color, int FigureType, int Rotation)
       {
           UInt64 resultvalue = 0;
           if( this.rotatedBoards[Color].Keys.Contains(FigureType))
           {
               resultvalue = this.rotatedBoards[Color][FigureType][Rotation];
           }
           return resultvalue;
       }

       public UInt64 GetMoveForFigure(Figure FigureToCheck, Int16 Position)
       {
           //Get all possible moves for this figure at the givin position
           UInt64 legalMoves =  db.GetMoveMask(Position, FigureToCheck);
           UInt64 enemyOrEmpty = this.emptySquares; //all enemies or empty squares
           UInt64 enemy = 0; //All figs of the current enemy color
           UInt64 tempResult = 0; //calculation or temp values
           Point CurrentPoistion = Chess.GUI.DrawHelper.PositionMatrix(Position);

           if (FigureToCheck.Color == Defaults.WHITE)
           {
               enemyOrEmpty |= this.blackPieces;
               enemy = this.blackPieces;
           }
           else
           {
               enemyOrEmpty |= this.whitePieces;
               enemy = this.whitePieces;
           }
           legalMoves &= enemyOrEmpty;
           
           switch (FigureToCheck.Type)
           { 
               case EFigures.Pawn:
                    {
                         //Check if pawn is blocked by any other figure in fornt of him
                        if ((GetPositionFromPosition(Position, (Int16)(8 * FigureToCheck.Color)) & enemy) > 0)
                        {
                            legalMoves = 0;
                        }
                        //Check if an attack is possible use attack datbase BuildPawnAttack function
                        legalMoves |= enemy & db.BuildPawnAttack(Position, FigureToCheck.Color);
                    }break;
               case EFigures.Rook:
                   {
                      
                   }
                   break;
                   
           }
           
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
            UInt64 result = Position & this.squarsBlocked;
            Figure returnValue = null;
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
                else { 
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chess.Tools;
using System.Drawing;

namespace Chess.Engine
{
   public class BitBoard :IDisposable
   {

        #region Figures
       private UInt64 whiteKing = 0ul;
        public UInt64 WhiteKing
        {
            get { return whiteKing; }
            set { whiteKing = value; }
        }
        private UInt64 whiteQueens = 0ul;
        public UInt64 WhiteQueens
        {
            get { return whiteQueens; }
            set { whiteQueens = value; }
        }
        private UInt64 whiteRooks = 0ul;
        public UInt64 WhiteRooks
        {
            get { return whiteRooks; }
            set { whiteRooks = value; }
        }
        private UInt64 whiteBishops = 0ul;
        public UInt64 WhiteBishops
        {
            get { return whiteBishops; }
            set { whiteBishops = value; }
        }
        private UInt64 whiteKnights = 0ul;
        public UInt64 WhiteKnights
        {
            get { return whiteKnights; }
            set { whiteKnights = value; }
        }
        private UInt64 whitePawns = 0ul;
        public UInt64 WhitePawns
        {
            get { return whitePawns; }
            set { whitePawns = value; }
        }
        private UInt64 whitePieces = 0ul;
        public UInt64 WhitePieces
        {
            get { return whitePieces; }
            set { whitePieces = value; }
        }
        private UInt64 blackKing = 0ul;
        public UInt64 BlackKing
        {
            get { return blackKing; }
            set { blackKing = value; }
        }
        private UInt64 blackQueens = 0ul;
        public UInt64 BlackQueens
        {
            get { return blackQueens; }
            set { blackQueens = value; }
        }
        private UInt64 blackRooks = 0ul;
        public UInt64 BlackRooks
        {
            get { return blackRooks; }
            set { blackRooks = value; }
        }
        private UInt64 blackbishops = 0ul;
        public UInt64 Blackbishops
        {
            get { return blackbishops; }
            set { blackbishops = value; }
        }
        private UInt64 blackKnights = 0ul;
        public UInt64 BlackKnights
        {
            get { return blackKnights; }
            set { blackKnights = value; }
        }
        private UInt64 blackPawns = 0ul;
        public UInt64 BlackPawns
        {
            get { return blackPawns; }
            set { blackPawns = value; }
        }
        private UInt64 blackPieces = 0ul;
        public UInt64 BlackPieces
        {
            get { return blackPieces; }
            set { blackPieces = value; }
        }
        #endregion

        #region Helping Boards
        private UInt64 squarsBlocked = 0ul;
        public UInt64 SquarsBlocked
        {
            get { return squarsBlocked; }
            set { squarsBlocked = value; }
        }
        private UInt64 emptySquares = 0ul;
        public UInt64 EmptySquares
        {
            get { return emptySquares; }
            set { emptySquares = value; }
        }
        private UInt64 attackedByWhite = 0ul;
        public UInt64 AttackedByWhite
        {
            get { return this.attackedByWhite; }
            set { this.attackedByWhite = value; }
        }
        private UInt64 attackedByBlack = 0ul;
        public UInt64 AttackedByBlack
        {
            get { return this.attackedByBlack; }
            set { this.attackedByBlack = value; }
        }
        private Dictionary<UInt64, List<Figure>> attackedBy = null;
        public Dictionary<UInt64, List<Figure>> AttackedBy
        {
            get { return attackedBy; }
            set { attackedBy = value; }
        }
        #endregion

       public BitBoard()
       {
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

       public void Dispose()
       {
           GC.SuppressFinalize(this);
       }
   }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Game
{
    class Defaults
    {
        //Default value for each starting bitboard

        public const int  BLACK = -1;
        public const int  WHITE = 1;
        public const int  ANY = 2;
        public const UInt64 WhiteKing = 0x8;
        public const UInt64 WhiteQueens = 0x10; 
        public const UInt64 WhiteRooks = 0x81;
        public const UInt64 WhiteBishops = 0x24;
        public const UInt64 WhiteKnights = 0x42;
        public const UInt64 WhitePawns = 0xFF00;
        public const UInt64 WhitePieces = 0xFFFF;

        public const UInt64 BlackKing = 0x800000000000000;
        public const UInt64 BlackQueens = 0x1000000000000000;
        public const UInt64 BlackRooks = 0x8100000000000000;
        public const UInt64 Blackbishops = 0x2400000000000000;
        public const UInt64 BlackKnights = 0x4200000000000000;
        public const UInt64 BlackPawns = 0xFF000000000000;
        public const UInt64 BlackPieces = 0xFFFF000000000000;

    }
}

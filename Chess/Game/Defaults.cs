using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Game
{   //TODO: Revert values back to orginal !!!
    class Defaults
    {
        //Default value for each starting bitboard

        public const int  BLACK = -1;
        public const int  WHITE = 1;
        public const int  ANY = 2;
        public const UInt64 WhiteKing = 0x8;
        public const UInt64 WhiteQueens = 0x10;
        public const UInt64 WhiteRooks = 0x100080;//0x81;
        public const UInt64 WhiteBishops = 0x420;//0x24;
        public const UInt64 WhiteKnights = 0x42;
        public const UInt64 WhitePawns = 0x0;//x4FB00;//0xFF00;
        public const UInt64 WhitePieces = 0xFFFF;

        public const UInt64 BlackKing = 0x800000000000000;
        public const UInt64 BlackQueens = 0x1000000000000000;
        public const UInt64 BlackRooks = 0x8100000000000000;
        public const UInt64 Blackbishops = 0x2400000000000000;
        public const UInt64 BlackKnights = 0x4200000000000000;
        public const UInt64 BlackPawns = 0xFC0000000A0000;//0xFF000000000000;
        public const UInt64 BlackPieces = 0xFFFF000000000000;

        public const UInt64 Rotated90ClockWise_BlackKing = 0x1000000;
        public const UInt64 Rotated90ClockWise_BlackQueens = 0x100000000;
        public const UInt64 Rotated90ClockWise_BlackRooks = 0x100000000000001;
        public const UInt64 Rotated90ClockWise_BlackBishops = 0x10000010000;
        public const UInt64 Rotated90ClockWise_BlackKnights = 0x1000000000100;
        public const UInt64 Rotated90ClockWise_BlackPawn = 0x202020202020202;
        public const UInt64 Rotated90ClockWise_BlackPieces = 0x303030303030303;

        public const UInt64 Rotated90ClockWise_WhiteKing =   0x80000000;
        public const UInt64 Rotated90ClockWise_WhiteQueens = 0x8000000000;
        public const UInt64 Rotated90ClockWise_WhiteRooks = 0x8000000000000080;
        public const UInt64 Rotated90ClockWise_WhiteBishops = 0x800000800000;
        public const UInt64 Rotated90ClockWise_WhiteKnights = 0x80000000008000;
        public const UInt64 Rotated90ClockWise_WhitePawn = 0x4040404040404040;
        public const UInt64 Rotated90ClockWise_WhitePieces = 0xC0C0C0C0C0C0C0C0;


        public const int ROTATED_KING = 0;
        public const int ROTATED_QUEEN = 1;
        public const int ROTATED_BISHOPS = 2;
        public const int ROTATED_KNIGHT = 3;
        public const int ROTATED_ROOKS = 4;
        public const int ROTATED_PAWN = 5;
        public const int ROTATED_ALL = 6;

        public const int ROTATED90CLOCKWISE = 0;
        public const int ROTATED45CLOCKWISE = 1;
        public const int ROTATED45ANTICLOCKWISE = 2;
    }
}


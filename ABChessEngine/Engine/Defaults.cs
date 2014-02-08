using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABChess.Engine
{   
    public class Defaults
    {
        //Default value for each starting bitboard

        public const int  BLACK = -1;
        public const int  WHITE = 1;
        public const int  ANY = 2;
        public const UInt64 WhiteKing = 0x8;
        public const UInt64 WhiteQueens = 0x10;
        public const UInt64 WhiteRooks =  0x81;
        public const UInt64 WhiteBishops =  0x24;
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
        //Startpostions for the rooks used in casteling moves
        public const UInt64 WhiteLeftRookStartPosition = 0x80;
        public const UInt64 WhiteRightRookStartPosition = 0x1;
        
        public const UInt64 BlackLeftRookStartPosition = 0x8000000000000000;
        public const UInt64 BlackRightRookStartPosition = 0x100000000000000;
        
        //Castling positions
        public const UInt64 CastlingWhiteRight = 0x2;
        public const UInt64 CastlingWhiteLeft = 0x20;
        public const UInt64 CastlingBlackLeft = 0x2000000000000000;
        public const UInt64 CastlingBlackRight = 0x200000000000000;
        public const UInt64 CastlingWhiteKingFieldsRight = 0x6;
        public const UInt64 CastlingWhiteKingFieldsLeft = 0x30;
        public const UInt64 CastlingBlackKingFieldsRight = 0x600000000000000;
        public const UInt64 CastlingBlackKingFieldsLeft = 0x3000000000000000;
        //0x100000000000000
        //First column of the board ( from the view of the white player)
        public const UInt64 WhitePromotionRank = 0xFF;
        //Last column of the board ( from the view of the white player)
        public const UInt64 BlackPromotionRank = 0xFF00000000000000;

    }
}


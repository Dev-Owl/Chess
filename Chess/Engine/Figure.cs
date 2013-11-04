using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Engine
{
   public class Figure
    {

        private int color;
        public int Color
        {
            get { return color; }
            set { color = value; }
        }
      
        private EFigures type;
        public EFigures Type
        {
            get { return type; }
            set { type = value; }
        }

       //Used to store the information only in the attack by helper boards (see Bitboard class)
        private UInt64 position;
        public UInt64 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Figure(int Color, EFigures Type)
        {
            this.color = Color;
            this.type = Type;
        }

    }
}

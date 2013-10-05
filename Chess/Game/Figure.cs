using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Game
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

        public Figure(int Color, EFigures Type)
        {
            this.color = Color;
            this.type = Type;
        }

    }
}

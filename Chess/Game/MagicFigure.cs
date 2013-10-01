using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Game
{
   public class MagicFigure
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

        public MagicFigure(int Color, EFigures Type)
        {
            this.color = Color;
            this.type = Type;
        }

    }
}

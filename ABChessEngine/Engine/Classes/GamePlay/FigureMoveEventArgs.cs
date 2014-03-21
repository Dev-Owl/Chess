using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABChess.Engine
{
    public class FigureMoveEventArgs : EventArgs
    {
        public Figure MovedFigure { get; set; }
        public Int16 TargetPosition { get; set; }

        public FigureMoveEventArgs(Figure MovedFigure, Int16 TargetPosition)
            : base()
        {
            this.MovedFigure = MovedFigure;
            this.TargetPosition = TargetPosition;
        }
    }
}

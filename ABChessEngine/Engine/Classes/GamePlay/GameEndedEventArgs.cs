using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABChess.Engine
{
    public class GameEndedEventArgs : EventArgs
    {
        public int Winner { get; set; }


        public GameEndedEventArgs(int Winner )
            : base()
        {
            this.Winner = Winner;
        }
    }
}

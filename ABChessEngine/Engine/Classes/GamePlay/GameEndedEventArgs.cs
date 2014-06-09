using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABChess.Engine
{
    public class GameEndedEventArgs : EventArgs
    {
		/// <summary>
		/// Color of the player that is the winner of the current match
		/// </summary>
		/// <value>The winner.</value>
        public int Winner { get; set; }


        public GameEndedEventArgs(int Winner )
            : base()
        {
            this.Winner = Winner;
        }
    }
}

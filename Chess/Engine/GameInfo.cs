using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Engine
{
    public class GameInfo
    {
        /// <summary>
        /// Name of the Player that owns the white figures
        /// </summary>
        public string Player1 { get; set; }
        
        /// <summary>
        /// Name of the Player that owns the black figures
        /// </summary>
        public string Player2 { get; set; }
        
        /// <summary>
        /// The game time in seconds
        /// </summary>
        public int GameTime { get; set; }

        /// <summary>
        /// Starting time for the game (UNIX-Timestamp)
        /// </summary>
        public int StartingTime { get; set; }
        
    }
}

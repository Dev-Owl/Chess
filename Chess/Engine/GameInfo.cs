using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Chess.Engine
{
    [Serializable]
    public class GameInfo
    {
        /// <summary>
        /// Name of the Player that owns the white figures
        /// </summary>
        public string Player1 { get; set; }

        /// <summary>
        /// If true this player is owned by the AI
        /// </summary>
        public bool Player1IsAI { get; set; }

        /// <summary>
        /// Name of the Player that owns the black figures
        /// </summary>
        public string Player2 { get; set; }

        /// <summary>
        /// If true this player is owned by the AI
        /// </summary>
        public bool Player2IsAI { get; set; }

        /// <summary>
        /// The game time in seconds
        /// </summary>
        public UInt64 GameTime { get; set; }

        /// <summary>
        /// Starting time for the game (UNIX-Timestamp)
        /// </summary>
        public UInt64 StartingTime { get; set; }

     
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Engine
{
    public class GameHistory : List<BitBoard>
    {
        //TODO: Add meta information to saved data to identify more games (class needed player name color time)

        public GameHistory()
            : base()
        { }

        public void SaveHistoryToDisk()
        { 
            //TODO: Add a way to save objects on disk
        }

        public void SaveHistoryToDB()
        {
            //TODO: Add a way to save objects on disk
        }

       
    }
}

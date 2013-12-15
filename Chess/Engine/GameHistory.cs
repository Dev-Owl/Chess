using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Engine
{

    //TODO:Add documentation below


    public class GameHistory
    {
      
        private Dictionary<GameInfo,List<BitBoard>> history;
        private MoveGenerator moveGenerator;

        public MoveGenerator MoveGenerator
        {
            get { return moveGenerator; }
            set { moveGenerator = value; }
        }

        public GameHistory()
        {
            history = new Dictionary<GameInfo,List<BitBoard>>();
        }

        public void AddGame( GameInfo Info,List<BitBoard> Moves=null)
        {
            this.history.Add(Info,new List<BitBoard>());
            if( Moves !=null)
            {
                this.history[Info].AddRange( Moves);
            }
        }

        public void AddHistory(GameInfo Info, BitBoard Move)
        {
            this.history[Info].Add(BitBoard.CopyFigureValues(Move));
        }

        public void SaveHistoryToDisk()
        { 
            //TODO: Add a way to save objects on disk
        }

        public bool RevertLastMove()
        {
            bool reverted = false;
            //Is a moveGenerator set if not we can not revert something
            if (this.moveGenerator != null)
            {
                //Do we have a step that can be removed
                if (this.history[this.moveGenerator.CurrentGame].Count > 0)
                { 
                    //Get history of the game
                    List<BitBoard> moves = this.history[this.moveGenerator.CurrentGame];
                    //Set the new state to the moveGenerator
                    this.moveGenerator.CurrentGameState = moves[moves.Count-1];
                    //Remove the last entry from the history
                    moves.RemoveAt(moves.Count - 1);
                    reverted = true;
                }
            }
            return reverted;
        }
    }
}

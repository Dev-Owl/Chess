using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ABChess.Engine
{

    //TODO:Add documentation below


    public class GameHistory
    {

        private Dictionary<GameInfo, List<BitBoard>> history;
        public Dictionary<GameInfo, List<BitBoard>> History
        {
            get { return history; }
            set {
                //Prevent passing null values issue related to the designer
                if (value != null)
                {
                    history = value;
                }
            }
        }

        private int activeColor;
        public int ActiveColor
        {
            get { return activeColor; }
            set { activeColor = value; }
        }

        private MoveGenerator moveGenerator;

        public MoveGenerator MoveGenerator
        {
            get { return moveGenerator; }
            set { moveGenerator = value; }
        }

        public GameHistory()
        {
            history = new Dictionary<GameInfo,List<BitBoard>>();
            activeColor = Defaults.WHITE;
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
            this.activeColor *= -1;
        }

        public void SaveHistoryToDisk(GameInfo Info,string PathToSave)
        { 
            if (!string.IsNullOrEmpty(PathToSave))
            {
                string fileName = Path.GetFileName(PathToSave);
                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = string.Format("{0}vs{1}_{2}.chess", Info.Player1, Info.Player2, DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                    PathToSave = Path.Combine(PathToSave, fileName);
                }
                using (StreamWriter sw = new StreamWriter(PathToSave, false, Encoding.UTF8))
                {
                    sw.WriteLine( SerializeObject(Info));
                    sw.WriteLine(history[Info].Count);
                    foreach (BitBoard board in history[Info])
                    { 
                        sw.WriteLine(board.WhiteKing.ToString());
                        sw.WriteLine(board.WhiteQueens.ToString());
                        sw.WriteLine(board.WhiteRooks.ToString());
                        sw.WriteLine(board.WhiteBishops.ToString());
                        sw.WriteLine(board.WhiteKnights.ToString());
                        sw.WriteLine(board.WhitePawns.ToString());
                        sw.WriteLine(board.EnPassantWhite.ToString());
                        sw.WriteLine(board.BlackKing.ToString());
                        sw.WriteLine(board.BlackQueens.ToString());
                        sw.WriteLine(board.BlackRooks.ToString());
                        sw.WriteLine(board.Blackbishops.ToString());
                        sw.WriteLine(board.BlackKnights.ToString());
                        sw.WriteLine(board.BlackPawns.ToString());
                        sw.WriteLine(board.EnPassantBlack.ToString());
                        sw.WriteLine(board.BlackKingCheck ? "1" : "0");
                        sw.WriteLine(board.WhiteKingMoved ? "1" : "0");
                        sw.WriteLine(board.BlackLeftRookMoved ? "1" : "0");
                        sw.WriteLine(board.BlackRightRookMoved ? "1" : "0");
                        sw.WriteLine(board.WhiteLeftRookMoved ? "1" : "0");
                        sw.WriteLine(board.WhiteRightRookMoved ? "1" : "0");
                    }
                    sw.WriteLine(this.activeColor);
                    //Save current state also
                    sw.WriteLine(this.moveGenerator.CurrentGameState.WhiteKing.ToString());
                    sw.WriteLine(this.moveGenerator.CurrentGameState.WhiteQueens.ToString());
                    sw.WriteLine(this.moveGenerator.CurrentGameState.WhiteRooks.ToString());
                    sw.WriteLine(this.moveGenerator.CurrentGameState.WhiteBishops.ToString());
                    sw.WriteLine(this.moveGenerator.CurrentGameState.WhiteKnights.ToString());
                    sw.WriteLine(this.moveGenerator.CurrentGameState.WhitePawns.ToString());
                    sw.WriteLine(this.moveGenerator.CurrentGameState.EnPassantWhite.ToString());
                    sw.WriteLine(this.moveGenerator.CurrentGameState.BlackKing.ToString());
                    sw.WriteLine(this.moveGenerator.CurrentGameState.BlackQueens.ToString());
                    sw.WriteLine(this.moveGenerator.CurrentGameState.BlackRooks.ToString());
                    sw.WriteLine(this.moveGenerator.CurrentGameState.Blackbishops.ToString());
                    sw.WriteLine(this.moveGenerator.CurrentGameState.BlackKnights.ToString());
                    sw.WriteLine(this.moveGenerator.CurrentGameState.BlackPawns.ToString());
                    sw.WriteLine(this.moveGenerator.CurrentGameState.EnPassantBlack.ToString());
                    sw.WriteLine(this.moveGenerator.CurrentGameState.BlackKingCheck ? "1" : "0");
                    sw.WriteLine(this.moveGenerator.CurrentGameState.WhiteKingMoved ? "1" : "0");
                    sw.WriteLine(this.moveGenerator.CurrentGameState.BlackLeftRookMoved ? "1" : "0");
                    sw.WriteLine(this.moveGenerator.CurrentGameState.BlackRightRookMoved ? "1" : "0");
                    sw.WriteLine(this.moveGenerator.CurrentGameState.WhiteLeftRookMoved ? "1" : "0");
                    sw.WriteLine(this.moveGenerator.CurrentGameState.WhiteRightRookMoved ? "1" : "0");
                    
                }
            }
        }

        public bool LoadHistoryFromDisk(string PathToLoad)
        {

            bool result = false;
            if (!string.IsNullOrEmpty(PathToLoad))
            {
                if (File.Exists(PathToLoad))
                {
                    this.history.Clear();
                    using (StreamReader sr = new StreamReader(PathToLoad, Encoding.UTF8))
                    {
                        GameInfo info = (GameInfo)DeserializeObject(sr.ReadLine());
                        BitBoard board = new BitBoard();
                        this.history.Add(info, new List<BitBoard>());
                        int amount = 0;
                        if (int.TryParse(sr.ReadLine(), out amount))
                        {
                            for (int step = 0; step < amount; ++step)
                            {
                               
                                board.WhiteKing = UInt64.Parse(sr.ReadLine());
                                board.WhiteQueens = UInt64.Parse(sr.ReadLine());
                                board.WhiteRooks = UInt64.Parse(sr.ReadLine());
                                board.WhiteBishops = UInt64.Parse(sr.ReadLine());
                                board.WhiteKnights = UInt64.Parse(sr.ReadLine());
                                board.WhitePawns = UInt64.Parse(sr.ReadLine());
                                board.EnPassantWhite = UInt64.Parse(sr.ReadLine());
                                board.BlackKing = UInt64.Parse(sr.ReadLine());
                                board.BlackQueens = UInt64.Parse(sr.ReadLine());
                                board.BlackRooks = UInt64.Parse(sr.ReadLine());
                                board.Blackbishops = UInt64.Parse(sr.ReadLine());
                                board.BlackKnights = UInt64.Parse(sr.ReadLine());
                                board.BlackPawns = UInt64.Parse(sr.ReadLine());
                                board.EnPassantBlack = UInt64.Parse(sr.ReadLine());
                                board.BalckKingMoved = sr.ReadLine() == "1" ? true : false;
                                board.WhiteKingMoved = sr.ReadLine() == "1" ? true : false;
                                board.BlackLeftRookMoved = sr.ReadLine() == "1" ? true : false;
                                board.BlackRightRookMoved= sr.ReadLine() == "1" ? true : false;
                                board.WhiteLeftRookMoved = sr.ReadLine() == "1" ? true : false;
                                board.WhiteRightRookMoved = sr.ReadLine() == "1" ? true : false;
                                this.history[info].Add(BitBoard.CopyFigureValues(board));
                            }
                            this.activeColor = int.Parse(sr.ReadLine());
                            //Current state
                            board.WhiteKing = UInt64.Parse(sr.ReadLine());
                            board.WhiteQueens = UInt64.Parse(sr.ReadLine());
                            board.WhiteRooks = UInt64.Parse(sr.ReadLine());
                            board.WhiteBishops = UInt64.Parse(sr.ReadLine());
                            board.WhiteKnights = UInt64.Parse(sr.ReadLine());
                            board.WhitePawns = UInt64.Parse(sr.ReadLine());
                            board.EnPassantWhite = UInt64.Parse(sr.ReadLine());
                            board.BlackKing = UInt64.Parse(sr.ReadLine());
                            board.BlackQueens = UInt64.Parse(sr.ReadLine());
                            board.BlackRooks = UInt64.Parse(sr.ReadLine());
                            board.Blackbishops = UInt64.Parse(sr.ReadLine());
                            board.BlackKnights = UInt64.Parse(sr.ReadLine());
                            board.BlackPawns = UInt64.Parse(sr.ReadLine());
                            board.EnPassantBlack = UInt64.Parse(sr.ReadLine());
                            board.BalckKingMoved = sr.ReadLine() == "1" ? true : false;
                            board.WhiteKingMoved = sr.ReadLine() == "1" ? true : false;
                            board.BlackLeftRookMoved = sr.ReadLine() == "1" ? true : false;
                            board.BlackRightRookMoved = sr.ReadLine() == "1" ? true : false;
                            board.WhiteLeftRookMoved = sr.ReadLine() == "1" ? true : false;
                            board.WhiteRightRookMoved = sr.ReadLine() == "1" ? true : false;
                            this.history[info].Add(BitBoard.CopyFigureValues(board));

                            result = true;
                        }
                        else
                        {
                            throw new Exception("Invalid save file");
                        }
                    }
                }
            }
            return result;
        }

        private string SerializeObject(object o)
        {
            if (!o.GetType().IsSerializable)
            {
                return null;
            }

            using (MemoryStream stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, o);
                return Convert.ToBase64String(stream.ToArray());
            }
        }

        private object DeserializeObject(string str)
        {
            byte[] bytes = Convert.FromBase64String(str);

            using (MemoryStream stream = new MemoryStream(bytes))
            {
                return new BinaryFormatter().Deserialize(stream);
            }
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

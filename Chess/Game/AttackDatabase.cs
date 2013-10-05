using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;
using System.IO;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.Builders;

namespace Chess.Game
{
    /// <summary>
    /// This class is able to provide accses to a attack database or build a new database
    /// </summary>
    public class AttackDatabase : IDisposable
    {
        /*
         * TODO:  
         */
        Thinking thinking;
        Thread createThread;
        string mongoConnection = @"mongodb://localhost/?safe=true";
        MongoDatabase attackBoard;
        MongoCollection<AttackDocument> attacks;

        public AttackDatabase()
        {
            thinking = new Thinking();
            var server = new MongoClient(this.mongoConnection).GetServer();
            attackBoard = server.GetDatabase("attackBoard");
            attacks = attackBoard.GetCollection<AttackDocument>("attacks");
        }

        public void BuildAttackboard()
        {
            createThread = new Thread(BuildAttackDatabase);
            createThread.IsBackground = true;
            thinking.Show();
            createThread.Start();
          
        }

        private void BuildAttackDatabase()
        {
            attacks.RemoveAll();
            //StreamWriter sw = new StreamWriter("resultCurrentFig.txt", false);
            thinking.SetMessage("Starting with database, take a seat and drink a coffee");
            UInt64 currentIndex = 1;
           
                //For each possible figure on this square
                foreach (EFigures figtype in Enum.GetValues(typeof(EFigures)))
                {
                    switch (figtype)
                    {
                        //Depending on the current active index create the moving mask of this figure and store it in the database
                        case EFigures.King:
                            {
                                thinking.SetMessage("Working on the king figure at the moment");
                                #region King
                                UInt64 movingMask=0;
                                //For each square on the board 
                                for (UInt64 i = 0; i < 64; i++)
                                {
                                    // i == 0 means the first bit on the bitboard right lower corner

                                    // when i mod 8 is zero the position is on the left right side of the board so we can not move right (0 % N = 0)
                                    if (i % 8 != 0)
                                    {
                                        //we can move to the right so set bit i -1 to true
                                        movingMask |= (UInt64)Math.Pow(2, (i - 1));
                                        if (i + 7 <= 63)
                                        {
                                            movingMask |= (UInt64)Math.Pow(2, (i + 7));
                                        }
                                        if (i - 7 >= 0)
                                        {
                                            movingMask |= (UInt64)Math.Pow(2, (i - 7));
                                        }
                                    }
                                    // when i ist at least 8 we can go down a row
                                    if (i >= 8)
                                    { 
                                        // we can go down set the bit i-8 (prevouise byte) to true
                                        movingMask |= (UInt64)Math.Pow(2, (i - 8));
                                    }

                                    // when i is higher or equal 57 it is in the last byte which is the top row so we can not move up
                                    if (i < 57)
                                    {
                                        // we can go up set the bit i+8 (next byte) to true
                                        movingMask |= (UInt64)Math.Pow(2, (i + 8)); 
                                    }

                                    //If the next bit is divided by 8 without a carryover it is the next byte which means the next row so we are on the left side of a row
                                    if ((i + 1) % 8 != 0)
                                    { 
                                        //we can move left set i + 1 to true
                                        movingMask |= (UInt64)Math.Pow(2, (i + 1));
                                        if (i + 9 <= 63)
                                        {
                                            movingMask |= (UInt64)Math.Pow(2, (i + 9));
                                        }
                                        if (i - 9 >= 0)
                                        {
                                            movingMask |= (UInt64)Math.Pow(2, (i - 9));
                                        }
                                    }

                                    

                                    SaveInMongo((Int32)i, figtype, movingMask);    
                                    //Jump to the next index
                                    currentIndex = currentIndex << 1;
                                    //Reset movingMask for the next square
                                    movingMask = 0;
                                }
                                #endregion
                            }
                            break;
                        case EFigures.Rook:
                            {
                                 thinking.SetMessage("Working on the Rooks now");
                                 UInt64 movingMask=0;
                                 #region Rook
                                 //For each square on the board 
                                for (UInt64 i = 0; i < 64; i++)
                                {
                                    // i == 0 means the first bit on the bitboard right lower corner

                                    //when i is at least 8 we are in the second row so we can move down
                                    if (i > 7)
                                    {
                                        //Move down until the edge of the board move a row down means jump 8 bits to the next byte
                                        int tmp = (int)i - 8;
                                        while (tmp >= 0)
                                        {
                                            movingMask |= (UInt64)Math.Pow(2, tmp);
                                            tmp -= 8;
                                        }
                                    }

                                    // when i is not in the last line the rook can go up which means adding 8 bits to get to the next byte
                                    if (i < 57)
                                    {
                                        //move up untile the edge of the board
                                        int tmp = (int)i + 8;
                                        while (tmp <= 63)
                                        {
                                            movingMask |= (UInt64)Math.Pow(2, tmp);
                                            tmp += 8;
                                        }
                                    }

                                    //if i +1 %8 == 0 means we are at the last bit of a byte in this case we can not go to the left
                                    if ((i + 1) % 8 != 0)
                                    {
                                        // move left until the byte is full 
                                        int tmp = (int)i + 1;
                                        while (tmp  % 8 != 0)
                                        {
                                            movingMask |= (UInt64)Math.Pow(2, tmp);
                                            tmp += 1;
                                        }
                                    }

                                    // when i modulo 8 results in no carryover we are not at the right edge of the board
                                    if (i % 8 != 0)
                                    {
                                        //move right until the next byte is reached
                                        int tmp = (int)i - 1;
                                        int counter = 0;
                                        //find solution here !!!
                                        while ((int)i - (int)(( i/8) * 8) > counter)
                                        {
                                            movingMask |= (UInt64)Math.Pow(2, tmp);
                                            tmp -= 1;
                                            counter += 1;
                                        }
                                    }



                                     //sw.WriteLine(movingMask.ToString());

                                     //Save 
                                     SaveInMongo((Int32)i, figtype, movingMask);    
                                     //Jump to the next index
                                     currentIndex = currentIndex << 1;
                                     //Reset movingMask for the next square
                                     movingMask = 0;
                                }
                                 #endregion
                            }
                            break;
                        case EFigures.Knight:
                            {
                                thinking.SetMessage("Now taking care of the Knight");
                                UInt64 movingMask = 0;
                                #region Knight
                                //For each square on the board 
                                for (UInt64 i = 0; i < 64; i++)
                                {
                                    // see moves below
                                    //Up menas set bit i +15 for left and i + 17 for right
                                    //Down means set bit i -15 for left and i -17 for right
                                    //left means set bit i + 6 for up and i - 6 for down
                                    //right means set bit i + 10 for up and i - 10 for down
                                    UInt64 tmpResult = i;

                                    #region Up
                                    tmpResult =tmpResult+ 15;
                                    if (tmpResult <= 63 && i % 8 != 0)
                                    {
                                        movingMask |= (UInt64)Math.Pow(2, tmpResult);

                                    }

                                    tmpResult = i;
                                    tmpResult =tmpResult+ 17;
                                    if (tmpResult <= 63 && (i+1) % 8 !=0)
                                    {
                                        movingMask |= (UInt64)Math.Pow(2, tmpResult);
                                    }
                                    #endregion
                                    tmpResult = 0;
                                    #region down
                                    tmpResult = i;
                                    tmpResult =tmpResult- 15;
                                    if (tmpResult >= 0 && i % 8 != 0 && i >=15)
                                    {
                                        movingMask |= (UInt64)Math.Pow(2, tmpResult);
                                       
                                    }
                                    tmpResult = i;
                                    tmpResult = tmpResult -17;
                                    if (tmpResult >= 0 && (i + 1) % 8 != 0 && i >= 17)
                                    {
                                        movingMask |= (UInt64)Math.Pow(2, tmpResult);
                                    }
                                    #endregion
                                    tmpResult = 0;
                                    #region left
                                    //we can not move left we are already on the left edge 
                                    if ((i + 2) % 8 != 0)
                                    {

                                        tmpResult = i;
                                        tmpResult = tmpResult + 10;
                                        if (tmpResult <= 63)
                                        {
                                            movingMask |= (UInt64)Math.Pow(2, tmpResult);

                                        }
                                        tmpResult = i;
                                        tmpResult = tmpResult - 6;
                                        if (tmpResult >= 0 && i >=6)
                                        {
                                            movingMask |= (UInt64)Math.Pow(2, tmpResult);

                                        }
                                    }
                                    #endregion
                                    tmpResult = 0;
                                    #region right
                                    
                                    if ((i-1) % 8 != 0 && i %8 !=0)
                                    {
                                        tmpResult = i;
                                        tmpResult = tmpResult + 6;
                                        if (tmpResult <= 63)
                                        {
                                            movingMask |= (UInt64)Math.Pow(2, tmpResult);

                                        }
                                        tmpResult = i;
                                        tmpResult = tmpResult - 10;
                                        if (tmpResult >= 0 && i >=10)
                                        {
                                            movingMask |= (UInt64)Math.Pow(2, tmpResult);

                                        }
                                    }
                                    #endregion
                                    tmpResult = 0;
                                    //sw.WriteLine(movingMask.ToString());
                                    //Save 
                                    SaveInMongo((Int32)i, figtype, movingMask);
                                    //Jump to the next index
                                    currentIndex = currentIndex << 1;
                                    //Reset movingMask for the next square
                                    movingMask = 0;
                                }
                                #endregion
                            } break;
                        case EFigures.Bishop:
                            {
                                thinking.SetMessage("Taking care of the Bishop");
                                UInt64 movingMask = 0;
                                //Up left is plus 9 and up right 7
                                //Down left is -7 and right -9
                                //check if value is still in the range and 
                                //if we can jump to this not at the ege of a board
                                #region Bishop
                                for (UInt64 i = 0; i < 64; i++)
                                {
                                    // We can go up
                                    if (i <= 57)
                                    {
                                        UInt64 tmpPosition = i;
                                        //Go up left if we are not at the leeft side of the board
                                        if ((tmpPosition + 1) % 8 != 0)
                                        {
                                            tmpPosition += 9;
                                            //until the top row is reached
                                            while (tmpPosition <= 63)
                                            {
                                                movingMask |= (UInt64)Math.Pow(2, tmpPosition);
                                                //Are we already at the left side of the board
                                                if ((tmpPosition + 1) % 8 == 0)
                                                {
                                                    //Reached left side of the board escape the loop
                                                    break;
                                                }
                                                tmpPosition += 9;

                                            }
                                        }
                                        //Reset start position for UP right run
                                        tmpPosition = i;
                                        if (tmpPosition % 8 != 0)
                                        {
                                            //until the top row is reached
                                            tmpPosition += 7;
                                            while (tmpPosition <= 63)
                                            {
                                                                                                
                                                movingMask |= (UInt64)Math.Pow(2, tmpPosition);
                                                //Are we already at the left side of the board
                                                if (tmpPosition % 8 == 0)
                                                {
                                                    //Reached left side of the board escape the loop
                                                    break;
                                                }
                                                tmpPosition += 7;
                                            }
                                        }
                                    }
                                    if (i >= 8)
                                    {
                                        UInt64 tmpPosition = i;
                                        //Go down left if we are not at the leeft side of the board
                                        if ((tmpPosition + 1) % 8 != 0)
                                        {
                                            tmpPosition -= 7;
                                            //until the top row is reached
                                            while (tmpPosition >= 0)
                                            {
                                                
                                                
                                                movingMask |= (UInt64)Math.Pow(2, tmpPosition);
                                                //Are we already at the left side of the board
                                                if ((tmpPosition + 1) % 8 == 0)
                                                {                                                                                                
                                                    //Reached left side of the board escape the loop
                                                    break;
                                                }
                                                tmpPosition -= 7;
                                            }
                                        }
                                        //Reset start position for UP right run
                                        tmpPosition = i;
                                        if (tmpPosition % 8 != 0)
                                        {
                                            tmpPosition -= 9;
                                            //until the top row is reached
                                            while (tmpPosition >= 0)
                                            {
                                                
                                                movingMask |= (UInt64)Math.Pow(2, tmpPosition);
                                                //Are we already at the left side of the board
                                                if (tmpPosition % 8 == 0)
                                                {
                                                    //Reached left side of the board escape the loop
                                                    break;
                                                }
                                                tmpPosition -= 9;
                                            }
                                         
                                        }
                                    }

                                    //sw.WriteLine(movingMask.ToString());
                                    //Save 
                                    SaveInMongo((Int32)i, figtype, movingMask);
                                    //Jump to the next index
                                    currentIndex = currentIndex << 1;
                                    //Reset movingMask for the next square
                                    movingMask = 0;
                                #endregion


                                }

                            }break;
                        case EFigures.Queen:
                            {
                                thinking.SetMessage("Taking care of the queens");
                                UInt64 movingMask = 0;
                                #region Queen
                                for (UInt64 i = 0; i < 64; i++)
                                {
                                    // We can go up
                                    if (i <= 57)
                                    {
                                        UInt64 tmpPosition = i;
                                        //Go up left if we are not at the leeft side of the board
                                        if ((tmpPosition + 1) % 8 != 0)
                                        {
                                            tmpPosition += 9;
                                            //until the top row is reached
                                            while (tmpPosition <= 63)
                                            {
                                                movingMask |= (UInt64)Math.Pow(2, tmpPosition);
                                                //Are we already at the left side of the board
                                                if ((tmpPosition + 1) % 8 == 0)
                                                {
                                                    //Reached left side of the board escape the loop
                                                    break;
                                                }
                                                tmpPosition += 9;

                                            }
                                        }
                                        //Reset start position for UP right run
                                        tmpPosition = i;
                                        if (tmpPosition % 8 != 0)
                                        {
                                            //until the top row is reached
                                            tmpPosition += 7;
                                            while (tmpPosition <= 63)
                                            {
                                                                                                
                                                movingMask |= (UInt64)Math.Pow(2, tmpPosition);
                                                //Are we already at the left side of the board
                                                if (tmpPosition % 8 == 0)
                                                {
                                                    //Reached left side of the board escape the loop
                                                    break;
                                                }
                                                tmpPosition += 7;
                                            }
                                        }
                                    }
                                    if (i >= 8)
                                    {
                                        UInt64 tmpPosition = i;
                                        //Go down left if we are not at the leeft side of the board
                                        if ((tmpPosition + 1) % 8 != 0)
                                        {
                                            tmpPosition -= 7;
                                            //until the top row is reached
                                            while (tmpPosition >= 0)
                                            {


                                                movingMask |= (UInt64)Math.Pow(2, tmpPosition);
                                                //Are we already at the left side of the board
                                                if ((tmpPosition + 1) % 8 == 0)
                                                {
                                                    //Reached left side of the board escape the loop
                                                    break;
                                                }
                                                tmpPosition -= 7;
                                            }
                                        }
                                        //Reset start position for UP right run
                                        tmpPosition = i;
                                        if (tmpPosition % 8 != 0)
                                        {
                                            tmpPosition -= 9;
                                            //until the top row is reached
                                            while (tmpPosition >= 0)
                                            {

                                                movingMask |= (UInt64)Math.Pow(2, tmpPosition);
                                                //Are we already at the left side of the board
                                                if (tmpPosition % 8 == 0)
                                                {
                                                    //Reached left side of the board escape the loop
                                                    break;
                                                }
                                                tmpPosition -= 9;
                                            }

                                        }
                                        // i == 0 means the first bit on the bitboard right lower corner
                                    }
                                    //when i is at least 8 we are in the second row so we can move down
                                    if (i > 7)
                                    {
                                        //Move down until the edge of the board move a row down means jump 8 bits to the next byte
                                        int tmp = (int)i - 8;
                                        while (tmp >= 0)
                                        {
                                            movingMask |= (UInt64)Math.Pow(2, tmp);
                                            tmp -= 8;
                                        }
                                    }

                                    // when i is not in the last line the rook can go up which means adding 8 bits to get to the next byte
                                    if (i < 57)
                                    {
                                        //move up untile the edge of the board
                                        int tmp = (int)i + 8;
                                        while (tmp <= 63)
                                        {
                                            movingMask |= (UInt64)Math.Pow(2, tmp);
                                            tmp += 8;
                                        }
                                    }

                                    //if i +1 %8 == 0 means we are at the last bit of a byte in this case we can not go to the left
                                    if ((i + 1) % 8 != 0)
                                    {
                                        // move left until the byte is full 
                                        int tmp = (int)i + 1;
                                        while (tmp  % 8 != 0)
                                        {
                                            movingMask |= (UInt64)Math.Pow(2, tmp);
                                            tmp += 1;
                                        }
                                    }

                                    // when i modulo 8 results in no carryover we are not at the right edge of the board
                                    if (i % 8 != 0)
                                    {
                                        //move right until the next byte is reached
                                        int tmp = (int)i - 1;
                                        int counter = 0;
                                        //find solution here !!!
                                        while ((int)i - (int)(( i/8) * 8) > counter)
                                        {
                                            movingMask |= (UInt64)Math.Pow(2, tmp);
                                            tmp -= 1;
                                            counter += 1;
                                        }
                                    }
                                    

                                    //sw.WriteLine(movingMask.ToString());
                                    //Save 
                                    SaveInMongo((Int32)i, figtype, movingMask);
                                    //Jump to the next index
                                    currentIndex = currentIndex << 1;
                                    //Reset movingMask for the next square
                                    movingMask = 0;
                                 }
                                #endregion
                            }break;
                        case EFigures.PawnBlack:
                            {
                                thinking.SetMessage("Working on the Pawns starting with the black ones");
                                UInt64 movingMask = 0;
                                //Set position Index to the start
                                currentIndex |= (UInt64)Math.Pow(2, 8);
                                for (UInt64 i = 8; i <= 55; ++i)
                                {

                                    movingMask |= (UInt64)Math.Pow(2, (i-8));
                                    if (i > 47 && i <= 55)
                                    {
                                        movingMask |= (UInt64)Math.Pow(2, (i - 16));
                                    }
                                    //sw.WriteLine(movingMask.ToString());
                                    //Save 
                                    SaveInMongo((Int32)i, figtype, movingMask);
                                    //Jump to the next index
                                    currentIndex = currentIndex << 1;
                                    //Reset movingMask for the next square
                                    movingMask = 0;
                                   
                                }
  
                            }break;
                        case EFigures.PawnWhite:
                            {
                                thinking.SetMessage("Now taking care of the white pawns ");
                                UInt64 movingMask = 0;
                                //Set position Index to the start
                                currentIndex |= (UInt64)Math.Pow(2, 8);
                                for (UInt64 i = 8; i <= 55; ++i)
                                {

                                    movingMask |= (UInt64)Math.Pow(2, (i + 8));
                                    if (i > 7 && i <= 15)
                                    {
                                        movingMask |= (UInt64)Math.Pow(2, (i + 16));
                                    }
                                    //sw.WriteLine(movingMask.ToString());
                                    //Save 
                                    SaveInMongo((Int32)i, figtype, movingMask);
                                    //Jump to the next index
                                    currentIndex = currentIndex << 1;
                                    //Reset movingMask for the next square
                                    movingMask = 0;

                                }
                            }break;

                    }
                    currentIndex = 1;
                
              
            }
                //sw.Close();
            thinking.Close();
        }

        private void SaveInMongo(Int32 Position, EFigures Type, UInt64 MoveMask)
        {
            //Insert the new position in the datbase 
            attacks.Insert(new AttackDocument() { F = (int)Type, M = MoveMask, P = Position });
        }

        public UInt64 GetMoveMask(Int16 Position, MagicFigure Figure)
        {
            UInt64 returnValue = 0;
            EFigures searchType = Figure.Type;
            if (Figure.Type == EFigures.Pawn && Figure.Color == Defaults.BLACK)
            {
                searchType = EFigures.PawnBlack;
            }
            else if(Figure.Type == EFigures.Pawn)  {
                searchType = EFigures.PawnWhite;
            }
            QueryDocument doc = new QueryDocument();
            doc.Add("F",(int)searchType);
            doc.Add("P", Position);
            var result = attacks.Find(doc);
            if (result.Count() > 0)
            {
                returnValue = result.ElementAt(0).M;
            }
            return returnValue;
        }

        public UInt64 BuildPawnAttack(Int16 Position, int Color)
        {
            UInt64 result = 0;
            if (Position % 8 != 0)
            {
                int newPos = Position + (7 * Color);
                if (newPos >= 0 && newPos <= 63)
                {
                    result |= (UInt64)Math.Pow(2, Position + (7 * Color));    
                }
            }
            if ((Position+1) % 8 != 0)
            {
                int newPos = Position + (9 * Color);
                if (newPos >= 0 && newPos <= 63)
                {
                    result |= (UInt64)Math.Pow(2, Position + (7 * Color));
                }
            }
            result |= (UInt64)Math.Pow(2, Position+(9*Color));
            
            return result;
        }

        public bool BackgroundWorkInprogress()
        {
            try
            {
                return createThread.IsAlive;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Single attack position in the database
        /// </summary>
        private class AttackDocument
        {
            /// <summary>
            /// This is needed because mongodb ( or the .net driver) do not support uint64
            /// </summary>
            //[BsonRepresentation(BsonType.Int64, AllowOverflow = true)] 
            public Int32 P;
           
            public Int32 F;

            /// <summary>
            /// This is needed because mongodb ( or the .net driver) do not support uint64
            /// </summary>
            [BsonRepresentation(BsonType.Int64, AllowOverflow = true)] 
            public UInt64 M;
            [BsonId]
            ObjectId _id { get; set; }
        }


        public void Dispose()
        {
            if (thinking != null)
            {
                thinking.Dispose();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Chess.GUI
{
    public class FigurePosition
    {

        public const char MAX_X = 'h';
        public const int MAX_Y = 8;
        public const char MIN_X = 'a';
        public const int MIN_Y = 1;


        
        char positionX;
        public char PositionX
        {
            get { return positionX; }
            set 
            {
                if (Regex.IsMatch(value.ToString(), @"^[a-hA-H]$"))
                    positionX = Char.ToLower( value);
                else
                    throw new Exception("Invalid X Position for Figure");
            }
        }
        int positionY;
        public int PositionY
        {
            get { return positionY; }
            set 
            {
                if (Regex.IsMatch(value.ToString(), @"^[1-8]$"))
                    positionY = value;
                else
                    throw new Exception("Invalid Y Position for Figure");
            }
        }

        public FigurePosition()
        { }

        public FigurePosition(char PositionX, int PositionY)
        {
            set(PositionX, PositionY);
        }

        public FigurePosition(int PositionX, int PositionY)
        {
            set((char)(96 + PositionX), PositionY);
        }

        private void set(char PositionX, int PositionY)
        {
            this.PositionX = PositionX;
            this.PositionY = PositionY;
        }

        /// <summary>
        /// Gets the 0 based position of the Figure (a = 0)
        /// </summary>
        /// <returns></returns>
        public int[] ToInt()
        { 
            return new int[] {(int)positionX-97,positionY-1};
        }

        public FigurePosition Left()
        {
            try
            {
                return new FigurePosition((char)((int)this.positionX - 1), this.positionY);
            }
            catch (Exception ex)
            { 
                
            }
            return null;
        }

        public FigurePosition Right()
        {
            try
            {
                return new FigurePosition((char)((int)this.positionX + 1), this.positionY);
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public FigurePosition Forward()
        {
            try
            {
                return new FigurePosition(this.positionX, this.positionY +1);
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public bool Same(FigurePosition Position)
        {
            if (Position.PositionY == this.PositionY && Position.PositionX == this.PositionX)
            {
                return true;
            }
            else {
                return false;
            }
        }

        public FigurePosition Backward()
        {
            try
            {
                return new FigurePosition(this.positionX, this.positionY - 1);
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public FigurePosition TopLeft()
        {
            try
            {
                return new FigurePosition((char)((int)this.positionX -1), this.positionY+1);
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public FigurePosition TopRight()
        {
            try
            {
                return new FigurePosition((char)((int)this.positionX + 1), this.positionY + 1);
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public FigurePosition LowerLeft()
        {
            try
            {
                return new FigurePosition((char)((int)this.positionX - 1), this.positionY - 1);
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public FigurePosition LowerRight()
        {
            try
            {
                return new FigurePosition((char)((int)this.positionX + 1), this.positionY - 1);
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public bool Below(FigurePosition Compare)
        {
            return Compare.PositionY > this.PositionY;
        }

        public bool Above(FigurePosition Compare)
        {
            return Compare.PositionY < this.PositionY;
        }
    }
}

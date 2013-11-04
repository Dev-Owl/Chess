using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using Chess.Tools;

namespace Chess.GUI
{
    class DrawHelper
    {

        public static Point ToDrawingPoint(int Position,int Width,int Height, int OffsetX=0,int OffsetY = 0)
        { 
         
            System.Drawing.Point pFigure = new System.Drawing.Point();
            pFigure.X = (831 - Width - 7)-(Position - ((Position / 8) * 8)) * Width + OffsetX;
            pFigure.Y = (7*Height)-((Position / 8) * Height) + OffsetY;
            return pFigure;
        }

        public static Point PositionMatrix(Int16 Position)
        {
            System.Drawing.Point pFigure = new System.Drawing.Point();
            pFigure.X =  (Position - ((Position / 8) * 8));
            pFigure.Y = (Position / 8);
            return pFigure;
        }
       
        public static Rectangle ToDrawingRectangle(int Position, int Width, int Height, int OffsetX = 0, int OffsetY = 0)
        {
            System.Drawing.Rectangle rec = new System.Drawing.Rectangle();

            rec.X =(831-Width-7) - (Position - ((Position / 8) * 8)) * Width + OffsetX;
            rec.Y = (7 * Height) - ((Position / 8) * Height) + OffsetY;
            rec.Width = Width;
            rec.Height = Height;
            return rec;
        }

        public static UInt64 FromDrawingPoint(int X, int Y)
        {            
            return BitOperations.SetBit(X+(Y*8));
        }


    }
}

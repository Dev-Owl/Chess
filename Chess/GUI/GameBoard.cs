﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using Chess.Tools;
using Chess.Game;


namespace Chess.GUI
{
    public class GameBoard  : Panel
    {
        //Magic
        MagicBitBoard magicboard;

        public MagicBitBoard Magicboard
        {
            get { return magicboard; }
            set { magicboard = value; }
        }
       



        #region Draw and Colors
        int selectedX = -1;
        public int SelectedX
        {
            get { return selectedX; }
            set { selectedX = value; }
        }
       
        int selectedY = -1;
        public int SelectedY
        {
            get { return selectedY; }
            set { selectedY = value; }
        }

        //List<FigurePosition> highlightFields;

        bool selected = false;
        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }
        
        Brush black = new SolidBrush(Color.FromArgb(140, 70, 0));
        Brush white = new SolidBrush(Color.FromArgb(255, 191, 128));
        Brush activeField = new SolidBrush(Color.FromArgb(70, 0, 0, 255));
        
        public int FieldSizeX
        {
            get { return this.Width/8; }
        } 
        public int FieldSizeY
        {
            get { return this.Height/8; }
        }

        private int offsetX = 5;
        public int OffsetX
        {
            get { return offsetX; }
            set { offsetX = value; }
        }
        private int offsetY = 5;
        public int OffsetY
        {
            get { return offsetY; }
            set { offsetY = value; }
        }

        public UInt64 SelectedMask=0;

        #endregion

        private Dictionary<EFigures, Image> whiteFigureFiles;
        private Dictionary<EFigures, Image> blackFigureFiles;

        //List<Figure> figures;
        //public List<Figure> Figures
        //{
        //    get { return figures; }
        //    set { figures = value; }
        //}

        bool gameRunning = false;
        public bool GameRunning
        {
            get { return gameRunning; }
            set { gameRunning = value; }
        }

        public delegate void PropertyChangeHandler(string Event, object data);
        // The event
        public event PropertyChangeHandler PropertyChange;



        public GameBoard() :base()
        {
            //figures = new List<Figure>();
            
       //     highlightFields = new List<FigurePosition>();
            whiteFigureFiles = new Dictionary<EFigures, Image>();
            blackFigureFiles = new Dictionary<EFigures, Image>();
            base.DoubleBuffered = true;
            magicboard = new MagicBitBoard();

        }      

        #region Drawing
        private Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics gra = e.Graphics;
            drawBoard(gra);
            drawFigures(gra);
        }

        private void drawBoard(Graphics gra)
        {
            int width = this.FieldSizeX, height = this.FieldSizeY;

            for (int x = 0; x < 8; ++x)
            {
                for (int y = 0; y < 8; ++y)
                {
                    if (y % 2 == 0)
                    {
                        gra.FillRectangle(x % 2 == 0 ? white : black, x * width, y * height, width, height);
                    }
                    else
                    {
                        gra.FillRectangle(x % 2 == 0 ? black : white, x * width, y * height, width, height);
                    }
                }
            }
            if (selected)
            {
                gra.FillRectangle(activeField, selectedX * width, selectedY * height, width, height);
                if (this.SelectedMask > 0)
                {
                    for (int index = 0; index < 64; ++index)
                    {
                        if (((SelectedMask >> index) & 1) != 0)
                        {
                            gra.FillRectangle(activeField, DrawHelper.ToDrawingRectangle(index, width, height));
                        }
                    }
                }
                
                //foreach (FigurePosition selectedPos in this.highlightFields)
                //{
                //    gra.FillRectangle(activeField, selectedPos.ToDrawingRectangle(width, height));
                //    //gra.FillRectangle(activeField, selectedPos.ToInt()[0] * width, selectedPos.ToInt()[1] * height, width, height);
                //}
            }
        }

        private void drawFigures(Graphics gra)
        {
            int width = this.FieldSizeX, height = this.FieldSizeY;
            UInt64 position = 1;
            for (Int16 i = 0; i < 64; i++)
            {
                
                MagicFigure tmp = this.magicboard.GetFigureAtPosition(position  );
                if (tmp != null)
                {
                    if (tmp.Color == Defaults.BLACK)
                    {
                        //Draw image here 
                        gra.DrawImage(this.blackFigureFiles[tmp.Type], DrawHelper.ToDrawingPoint(i, width, height, offsetX, OffsetY));
                    }
                    else
                    {
                        gra.DrawImage(this.whiteFigureFiles[tmp.Type], DrawHelper.ToDrawingPoint(i, width, height, offsetX, OffsetY));
                    }
                }
                position = (position << 1);
            }
            //foreach (Figure fig in this.figures.Where(k => k.Ingame))
            //{
            //    if (fig.Color == Figure.BLACK)
            //    {
            //        gra.DrawImage(this.blackFigureFiles[fig.Figuretype], fig.Position.ToDrawingPoint(width,height,offsetX,offsetY));
            //    }
            //    else {
            //        gra.DrawImage(this.whiteFigureFiles[fig.Figuretype], fig.Position.ToDrawingPoint(width, height, offsetX, offsetY));
            //    }
            //}
        }

        public void LoadResources()
        {
            Size imageSize = new System.Drawing.Size(this.FieldSizeX, this.FieldSizeY);
            whiteFigureFiles.Add(EFigures.Pawn, resizeImage(Image.FromFile(@"Graphics\PawnWhite.png"), imageSize));
            whiteFigureFiles.Add(EFigures.Queen, resizeImage(Image.FromFile(@"Graphics\QueenWhite.png"), imageSize));
            whiteFigureFiles.Add(EFigures.Rook, resizeImage(Image.FromFile(@"Graphics\RookWhite.png"), imageSize));
            whiteFigureFiles.Add(EFigures.Knight, resizeImage(Image.FromFile(@"Graphics\KnightWhite.png"), imageSize));
            whiteFigureFiles.Add(EFigures.Bishop, resizeImage(Image.FromFile(@"Graphics\BishopWhite.png"), imageSize));
            whiteFigureFiles.Add(EFigures.King, resizeImage(Image.FromFile(@"Graphics\KingWhite.png"), imageSize));
            blackFigureFiles.Add(EFigures.Pawn, resizeImage(Image.FromFile(@"Graphics\PawnBlack.png"), imageSize));
            blackFigureFiles.Add(EFigures.Queen, resizeImage(Image.FromFile(@"Graphics\QueenBlack.png"), imageSize));
            blackFigureFiles.Add(EFigures.Rook, resizeImage(Image.FromFile(@"Graphics\RookBlack.png"), imageSize));
            blackFigureFiles.Add(EFigures.Knight, resizeImage(Image.FromFile(@"Graphics\KnightBlack.png"), imageSize));
            blackFigureFiles.Add(EFigures.Bishop, resizeImage(Image.FromFile(@"Graphics\BishopBlack.png"), imageSize));
            blackFigureFiles.Add(EFigures.King, resizeImage(Image.FromFile(@"Graphics\KingBlack.png"), imageSize));
        }
        #endregion

        #region EventAndCallbacks

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (gameRunning)
            {
                SelectedMask = 0;
                selected = true;
                //highlightFields.Clear();
                selectedX = (int)(e.X / FieldSizeX);
                selectedY = (int)(e.Y / FieldSizeY);
                UInt64 bitBoardPosition = DrawHelper.FromDrawingPoint(7-selectedX, 7 - selectedY);
                MagicFigure fig = this.magicboard.GetFigureAtPosition(bitBoardPosition);
                if(fig != null)
                {
                  //Get valid moves for the selected figures
                    SelectedMask = this.magicboard.GetMoveForFigure(fig, (Int16)((7 - selectedX) + ((7 - selectedY) * 8)));
                    FireChangeEvent("Figure selected", SelectedMask);
                }
                this.Invalidate();
            }
        }

        public void StartNewGame()
        {
            LoadResources();
            this.gameRunning = true;
            magicboard.NewGame();
            this.Invalidate();
            FireChangeEvent("New Game");

            #region OLD
            //this.figures.Clear();
            //this.figures.Add(new Figure(EFigures.King, Figure.WHITE, this));
            //this.figures.Add(new Figure(EFigures.Queen, Figure.WHITE, this));
            //this.figures.Add(new Figure(EFigures.Knight, Figure.WHITE, this));
            //this.figures.Add(new Figure(EFigures.Bishop, Figure.WHITE, this));
            //this.figures.Add(new Figure(EFigures.Rook, Figure.WHITE, this));
            //this.figures.Add(new Figure(EFigures.Knight, Figure.WHITE, this));
            //this.figures.Add(new Figure(EFigures.Bishop, Figure.WHITE, this));
            //this.figures.Add(new Figure(EFigures.Rook, Figure.WHITE, this));


            //this.figures.Add(new Figure(EFigures.King, Figure.BLACK, this));
            //this.figures.Add(new Figure(EFigures.Queen, Figure.BLACK, this));
            //this.figures.Add(new Figure(EFigures.Knight, Figure.BLACK, this));
            //this.figures.Add(new Figure(EFigures.Bishop, Figure.BLACK, this));
            //this.figures.Add(new Figure(EFigures.Rook, Figure.BLACK, this));
            //this.figures.Add(new Figure(EFigures.Knight, Figure.BLACK, this));
            //this.figures.Add(new Figure(EFigures.Bishop, Figure.BLACK, this));
            //this.figures.Add(new Figure(EFigures.Rook, Figure.BLACK, this));

            //for (int i = 0; i < 16; ++i)
            //{
            //    this.figures.Add(new Figure(EFigures.Pawn, i > 7 ? Figure.BLACK : Figure.WHITE, this));
            //}

            ////TESTING !!!!!
            //this.figures.Find(f => f.Figuretype == EFigures.Knight && f.Color == Figure.BLACK).Position = new FigurePosition('d', 3);
            //this.figures.Find(f => f.Figuretype == EFigures.Rook && f.Color == Figure.BLACK).Position = new FigurePosition('e', 3);
            ////TESTING !!!!!

            //for (int x = 0; x < 8; ++x)
            //{
            //    for (int y = 0; y < 8; ++y)
            //    {
            //        this.gameBoard[x, y] = 0;
            //    }
            //}
            //this.figures.ForEach(k => this.gameBoard[k.Position.PositionX - 97, k.Position.PositionY-1] = (int)k.Figuretype);
            //this.figures.ForEach(k => k.Ingame = true);
            #endregion
        }

        private void FireChangeEvent(string Event ="",object data=null)
        {
               if (PropertyChange != null)
            {
                PropertyChange(Event, data);
            }
        }

        #endregion

        //#region Figure and Position
        //public bool PositionFree(FigurePosition SinglePosition)
        //{
        //    return this.figures.Count<Figure>(f => f.Position.Same(SinglePosition) && f.Ingame) == 0;
        //}

        //public bool PositionFree(FigurePosition[] MultiPositions)
        //{
        //    return this.figures.FindAll(f => MultiPositions.Contains<FigurePosition>(f.Position) && f.Ingame).Count == 0;
        //}

        //public Figure GetFigureAt(FigurePosition SinglePosition)
        //{
        //    return this.figures.Find(f => f.Position.Same(SinglePosition) && f.Ingame);
        //}

        //public List<Figure> GetFigureAt(FigurePosition[] Positions)
        //{
        //    return this.figures.FindAll(f => Positions.FirstOrDefault<FigurePosition>( p=> p.Same( f.Position)) != null && f.Ingame);
        //}

        //public List<Figure> TestMove(Figure MovingFigure, FigurePosition Destination)
        //{
        //    return null;
        //}

        //public void PreformMove(Figure MovingFigure, FigurePosition Destination)
        //{
            
        //}

        //public bool TestCheckForColor(int COLOR)
        //{
        //    //Get all figures that are not the same color as requested 
        //    //Get all position where the fig can go
        //    //If one of this positions match the kings position you are in check
        //    //If all positions are matched it is checkmate

        //    return false;
        //}

        //#endregion

    }
}

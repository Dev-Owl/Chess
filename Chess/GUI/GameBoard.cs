using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace Chess.GUI
{
    public class GameBoard  : Panel
    {

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

        List<FigurePosition> highlightFields;

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
        #endregion


        List<Figure> figures;
        public List<Figure> Figures
        {
            get { return figures; }
            set { figures = value; }
        }

        bool gameRunning = false;
        public bool GameRunning
        {
            get { return gameRunning; }
            set { gameRunning = value; }
        }

        private int[,] gameBoard;

        public GameBoard() :base()
        {
            figures = new List<Figure>();
            gameBoard = new int[8, 8];
            highlightFields = new List<FigurePosition>();
        }

        #region Drawing
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
                foreach (FigurePosition selectedPos in this.highlightFields)
                {
                    gra.FillRectangle(activeField, selectedPos.ToInt()[0] * width, selectedPos.ToInt()[1] * height, width, height);
                }
            }
        }

        private void drawFigures(Graphics gra)
        { 
        
        }
        #endregion

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (gameRunning)
            {
                selected = true;
                highlightFields.Clear();
                selectedX = (int)(e.X / FieldSizeX);
                selectedY = (int)(e.Y / FieldSizeY);
                FigurePosition selectedPos = new FigurePosition(selectedX+1, selectedY+1);
                Figure tmp = this.figures.Find(k => k.Position.Same(selectedPos));
                if(tmp != null)
                {
                    highlightFields = tmp.GetLegalMoves();
                }
                this.Invalidate();
            }
        }

        public void StartNewGame()
        {
            this.figures.Clear();
            this.gameRunning = true;
            
            this.figures.Add(new Figure(EFigures.King, Figure.WHITE, this));
            this.figures.Add(new Figure(EFigures.Queen, Figure.WHITE, this));
            this.figures.Add(new Figure(EFigures.Knight, Figure.WHITE, this));
            this.figures.Add(new Figure(EFigures.Bishop, Figure.WHITE, this));
            this.figures.Add(new Figure(EFigures.Rook, Figure.WHITE, this));
            this.figures.Add(new Figure(EFigures.Knight, Figure.WHITE, this));
            this.figures.Add(new Figure(EFigures.Bishop, Figure.WHITE, this));
            this.figures.Add(new Figure(EFigures.Rook, Figure.WHITE, this));


            this.figures.Add(new Figure(EFigures.King, Figure.BLACK, this));
            this.figures.Add(new Figure(EFigures.Queen, Figure.BLACK, this));
            this.figures.Add(new Figure(EFigures.Knight, Figure.BLACK, this));
            this.figures.Add(new Figure(EFigures.Bishop, Figure.BLACK, this));
            this.figures.Add(new Figure(EFigures.Rook, Figure.BLACK, this));
            this.figures.Add(new Figure(EFigures.Knight, Figure.BLACK, this));
            this.figures.Add(new Figure(EFigures.Bishop, Figure.BLACK, this));
            this.figures.Add(new Figure(EFigures.Rook, Figure.BLACK, this));

            for (int i = 0; i < 16; ++i)
            {
                this.figures.Add(new Figure(EFigures.Pawn, i > 7 ? Figure.BLACK : Figure.WHITE, this));
            }

            //TESTING !!!!!
            this.figures.Find(f => f.Figuretype == EFigures.Queen && f.Color == Figure.BLACK).Position = new FigurePosition('b', 4);
            //TESTING !!!!!

            for (int x = 0; x < 8; ++x)
            {
                for (int y = 0; y < 8; ++y)
                {
                    this.gameBoard[x, y] = 0;
                }
            }
            this.figures.ForEach(k => this.gameBoard[k.Position.PositionX - 97, k.Position.PositionY-1] = (int)k.Figuretype);
            this.figures.ForEach(k => k.Ingame = true);
        }

        #region Figure and Position
        public bool PositionFree(FigurePosition SinglePosition)
        {
            return this.figures.Count<Figure>(f => f.Position.Same(SinglePosition) && f.Ingame) == 0;
        }

        public bool PositionFree(FigurePosition[] MultiPositions)
        {
            return this.figures.FindAll(f => MultiPositions.Contains<FigurePosition>(f.Position) && f.Ingame).Count == 0;
        }

        public Figure GetFigureAt(FigurePosition SinglePosition)
        {
            return this.figures.Find(f => f.Position.Same(SinglePosition) && f.Ingame);
        }

        public List<Figure> GetFigureAt(FigurePosition[] Positions)
        {
            return this.figures.FindAll(f => Positions.FirstOrDefault<FigurePosition>( p=> p.Same( f.Position)) != null && f.Ingame);
        }

        public List<Figure> TestMove(Figure MovingFigure, FigurePosition Destination)
        {
            return null;
        }

        public void PreformMove(Figure MovingFigure, FigurePosition Destination)
        {
            
        }

        public bool TestCheckForColor(int COLOR)
        {
            //Get all figures that are not the same color as requested 
            //Get all position where the fig can go
            //If one of this positions match the kings position you are in check
            //If all positions are matched it is checkmate

            return false;
        }

        #endregion

    }
}

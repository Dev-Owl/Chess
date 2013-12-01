using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using Chess.Tools;
using Chess.Engine;

namespace Chess.GUI
{
    public class GameBoard  : Panel
    {

        MoveGenerator moveGenerator;
        
        public MoveGenerator MoveGenerator
        {
            get { return moveGenerator; }
            set { moveGenerator = value; }
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

        bool gameRunning = false;
        public bool GameRunning
        {
            get { return gameRunning; }
            set { gameRunning = value; }
        }

        public delegate void PropertyChangeHandler(string Event, ChangedEventArgs e);
        // This is not the event you are looking for ;)
        public event PropertyChangeHandler PropertyChange;



        public GameBoard() :base()
        {
            whiteFigureFiles = new Dictionary<EFigures, Image>();
            blackFigureFiles = new Dictionary<EFigures, Image>();
            base.DoubleBuffered = true;
            this.moveGenerator = new MoveGenerator();
        }      

        #region Drawing
        private Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                base.OnPaint(e);
                Graphics gra = e.Graphics;
                drawBoard(gra);
                if (this.gameRunning)
                {
                    drawFigures(gra);
                }
            }
            catch (Exception ex)
            { }
        }

        private void drawBoard(Graphics gra)
        {
            try
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
                                gra.FillRectangle(activeField, DrawHelper.ToDrawingRectangle(index, width, height,this.Width));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { 
            
            }
        }

        private void drawFigures(Graphics gra)
        {
            int width = this.FieldSizeX, height = this.FieldSizeY;
            UInt64 position = 1;
            for (Int16 i = 0; i < 64; i++)
            {
                Figure tmp = this.moveGenerator.GetFigureAtPosition(position  );
                if (tmp != null)
                {
                    if (tmp.Color == Defaults.BLACK)
                    {
                        gra.DrawImage(this.blackFigureFiles[tmp.Type], DrawHelper.ToDrawingPoint(i, width, height,this.Width, offsetX, OffsetY));
                    }
                    else
                    {
                        gra.DrawImage(this.whiteFigureFiles[tmp.Type], DrawHelper.ToDrawingPoint(i, width, height,this.Width, offsetX, OffsetY));
                    }
                }
                position = (position << 1);
            }
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
                selectedX = (int)(e.X / FieldSizeX);
                selectedY = (int)(e.Y / FieldSizeY);
                UInt64 bitBoardPosition = DrawHelper.FromDrawingPoint(7-selectedX, 7 - selectedY);
                Figure fig = this.moveGenerator.GetFigureAtPosition(bitBoardPosition);
                if(fig != null)
                {
                    //Get valid moves for the selected figures
                    selected = true;
                    SelectedMask = this.moveGenerator.GetMoveForFigure(fig, (Int16)((7 - selectedX) + ((7 - selectedY) * 8)));
                    FireChangeEvent("Figure selected", SelectedMask);
                }
                this.Invalidate();
            }
        }

        public void StartNewGame()
        {
            
            LoadResources();
            this.gameRunning = true;
            moveGenerator.NewGame();
            this.Invalidate();
            FireChangeEvent("New Game");
        }

        private void FireChangeEvent(string Event ="",object data=null)
        {
               if (PropertyChange != null)
            {
                PropertyChange(Event, new ChangedEventArgs( data));
            }
        }

        #endregion
    }
}

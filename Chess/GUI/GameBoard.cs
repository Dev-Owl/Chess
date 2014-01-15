using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using Chess.Tools;
using Chess.Engine;
using Chess.GUI.Forms;

namespace Chess.GUI
{
    //TODO: Add documentaion for the class below

    public class GameBoard  : Panel, IPromotion
    {

        MoveGenerator moveGenerator;        
        public MoveGenerator MoveGenerator
        {
            get { return moveGenerator; }
            set { moveGenerator = value; this.moveGenerator.PromotionHandler = this; this.moveGenerator.GameEnded += moveGenerator_GameEnded; }
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
        Brush activeFieldPlayer = new SolidBrush(Color.FromArgb(70, 36, 255, 36));
        Brush activeFieldEnemy = new SolidBrush(Color.FromArgb(70, 255, 36, 36));


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

        public Figure SelectedFigure { get; set; }

        private bool imagesLoaded = false;
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

        public int ActiveColor { get; set; }


        public GameBoard() :base()
        {
            whiteFigureFiles = new Dictionary<EFigures, Image>();
            blackFigureFiles = new Dictionary<EFigures, Image>();
            base.DoubleBuffered = true;
            this.moveGenerator = new MoveGenerator();
            this.moveGenerator.PromotionHandler = this;
            this.ActiveColor = Defaults.WHITE;
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
                    Brush drawBursh = this.SelectedFigure == null ? this.activeFieldPlayer: this.ActiveColor == this.SelectedFigure.Color ? activeFieldPlayer : activeFieldEnemy;
                    gra.FillRectangle(drawBursh, selectedX * width, selectedY * height, width, height);
                    if (this.SelectedMask > 0)
                    {
                        for (int index = 0; index < 64; ++index)
                        {
                            if (((SelectedMask >> index) & 1) != 0)
                            {
                                gra.FillRectangle(drawBursh, DrawHelper.ToDrawingRectangle(index, width, height, this.Width));
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
            if (!imagesLoaded)
            {
                if ((whiteFigureFiles.Count + blackFigureFiles.Count) == 0)
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
                    imagesLoaded = true;
                }
            }
        }
        #endregion

        #region EventAndCallbacks

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            selectedX = (int)(e.X / FieldSizeX);
            selectedY = (int)(e.Y / FieldSizeY);

            if (gameRunning)
            {
                if (!ClickedMarkedField())
                {
                    SelectedMask = 0;
                    UInt64 bitBoardPosition = DrawHelper.FromDrawingPoint(7 - selectedX, 7 - selectedY);
                    Figure fig = this.moveGenerator.GetFigureAtPosition(bitBoardPosition);
                    selected = true;
                    if (fig != null)
                    {
                        //Get valid moves for the selected figures
                        SelectedMask = this.moveGenerator.GetMoveForFigure(fig,GetShortPosition(selectedX,selectedY));
                        this.SelectedFigure = fig;
                        FireChangeEvent("Figure selected", SelectedMask);
                    }
                    else {
                        this.SelectedFigure = null;
                    }
                }
                this.Invalidate();
            }
        }

        private short GetShortPosition(int x, int y)
        {
            return Math.Abs((Int16)((7 - x) + ((7 - y) * 8)));
        }

        private bool ClickedMarkedField()
        {
            bool moveDone = false;
            if (this.SelectedFigure != null)
            {
                //Get the bitboard position of the click
                UInt64 bitBoardPosition = DrawHelper.FromDrawingPoint(7 - selectedX, 7 - selectedY);
                //If the move fields contain this position and the color of the selected figure is matching move the figure
                if ((SelectedMask & bitBoardPosition) > 0 && this.ActiveColor == this.SelectedFigure.Color)
                {
                    //The figure that should be move ( current selected one) and the position in the  1 to 64 matrix
                    this.moveGenerator.MakeAMove(this.SelectedFigure, GetShortPosition(selectedX, selectedY));
                    //Change the color of the active player
                    this.ActiveColor *= -1;
                    //Show the calling function that the move is done
                    moveDone = true;
                    //Fire Move event
                    FireChangeEvent("Figure moved", this.SelectedFigure);
                    //Clear the selected values
                    this.SelectedMask = 0;
                    this.SelectedFigure = null;
                    this.selected = false;
                    
                }
            }
            return moveDone;
        }

        public void StartNewGame(GameInfo NewGame)
        {
            LoadResources();
            this.gameRunning = true;
            moveGenerator.NewGame(NewGame);
            this.Invalidate();
            FireChangeEvent("New Game");
        }

        public void MoveReverted()
        {
            //Clear the selected values
            this.SelectedMask = 0;
            this.SelectedFigure = null;
            this.selected = false;
            //Change the active player
            this.ActiveColor *= -1;
            FireChangeEvent("Move reverted");
            this.Invalidate();
        }

        public void SaveGame(string Path)
        {
            if (gameRunning)
            {
                this.moveGenerator.History.SaveHistoryToDisk(this.moveGenerator.CurrentGame, Path);
                FireChangeEvent("Game saved");
            }
        }

        public void LoadGame(string Path)
        {
            GameHistory history = new GameHistory();

            if (history.LoadHistoryFromDisk(Path))
            {
                GameInfo info = history.History.Keys.First<GameInfo>();
                this.moveGenerator.NewGame(info);
                history.MoveGenerator = this.moveGenerator;
                this.moveGenerator.History = history;
                this.moveGenerator.CurrentGameState = history.History[info][history.History[info].Count-1];
                history.History[info].Remove(this.moveGenerator.CurrentGameState);
                this.gameRunning = true;
                this.moveGenerator.GameRunning = true;
                LoadResources();
                this.ActiveColor = history.ActiveColor;
                FireChangeEvent("Game loaded", this.moveGenerator.CurrentGameState.SquarsBlocked);
                this.Invalidate();
            }
            else
            {
                MessageBox.Show("Loading failed");
            }

            
        }

        private void FireChangeEvent(string Event ="",object data=null)
        {
            if (PropertyChange != null)
            {
                PropertyChange(Event, new ChangedEventArgs( data));
            }
        }

        void moveGenerator_GameEnded(object sender, GameEndedEventArgs e)
        {
            if (e.Winner == Defaults.WHITE)
            {
                MessageBox.Show("The White player wins the game.", "Game over");
            }
            else
            {
                MessageBox.Show("The Black player wins the game.", "Game over");
            }
            this.GameRunning = false;
        }

        #endregion

        public EFigures GetDecision()
        {
            PromotionForm form = new PromotionForm();
            form.ShowDialog(this.Parent);
            return form.Result;
        }
    }
}

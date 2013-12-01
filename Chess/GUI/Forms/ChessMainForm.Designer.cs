namespace Chess.GUI.Forms
{
    partial class ChessMainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Chess.Engine.MoveGenerator moveGenerator2 = new Chess.Engine.MoveGenerator();
            Chess.Engine.AttackDatabase attackDatabase2 = new Chess.Engine.AttackDatabase();
            Chess.Engine.BitBoard bitBoard2 = new Chess.Engine.BitBoard();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChessMainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.gameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gameBoard = new Chess.GUI.GameBoard();
            this.optionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gameToolStripMenuItem,
            this.optionToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(784, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "mainMenu";
            // 
            // gameToolStripMenuItem
            // 
            this.gameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.gameToolStripMenuItem.Name = "gameToolStripMenuItem";
            this.gameToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.gameToolStripMenuItem.Text = "Game";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.loadToolStripMenuItem.Text = "Load";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // gameBoard
            // 
            this.gameBoard.GameRunning = false;
            this.gameBoard.Location = new System.Drawing.Point(12, 27);
            moveGenerator2.AttackDatabase = attackDatabase2;
            moveGenerator2.CurrentGame = null;
            bitBoard2.AttackedBy = ((System.Collections.Generic.Dictionary<ulong, System.Collections.Generic.List<Chess.Engine.Figure>>)(resources.GetObject("bitBoard2.AttackedBy")));
            bitBoard2.AttackedByBlack = ((ulong)(0ul));
            bitBoard2.AttackedByWhite = ((ulong)(0ul));
            bitBoard2.Blackbishops = ((ulong)(0ul));
            bitBoard2.BlackKing = ((ulong)(0ul));
            bitBoard2.BlackKingCheck = false;
            bitBoard2.BlackKnights = ((ulong)(0ul));
            bitBoard2.BlackPawns = ((ulong)(0ul));
            bitBoard2.BlackPieces = ((ulong)(0ul));
            bitBoard2.BlackQueens = ((ulong)(0ul));
            bitBoard2.BlackRooks = ((ulong)(0ul));
            bitBoard2.EmptySquares = ((ulong)(0ul));
            bitBoard2.ProtecteddBy = ((System.Collections.Generic.Dictionary<ulong, System.Collections.Generic.List<Chess.Engine.Figure>>)(resources.GetObject("bitBoard2.ProtecteddBy")));
            bitBoard2.SquarsBlocked = ((ulong)(0ul));
            bitBoard2.WhiteBishops = ((ulong)(0ul));
            bitBoard2.WhiteKing = ((ulong)(0ul));
            bitBoard2.WhiteKingCheck = false;
            bitBoard2.WhiteKnights = ((ulong)(0ul));
            bitBoard2.WhitePawns = ((ulong)(0ul));
            bitBoard2.WhitePieces = ((ulong)(0ul));
            bitBoard2.WhiteQueens = ((ulong)(0ul));
            bitBoard2.WhiteRooks = ((ulong)(0ul));
            moveGenerator2.CurrentGameState = bitBoard2;
            moveGenerator2.GameRunning = false;
            this.gameBoard.MoveGenerator = moveGenerator2;
            this.gameBoard.Name = "gameBoard";
            this.gameBoard.OffsetX = 5;
            this.gameBoard.OffsetY = 5;
            this.gameBoard.Selected = false;
            this.gameBoard.SelectedX = -1;
            this.gameBoard.SelectedY = -1;
            this.gameBoard.Size = new System.Drawing.Size(760, 523);
            this.gameBoard.TabIndex = 1;
            // 
            // optionToolStripMenuItem
            // 
            this.optionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.debugViewToolStripMenuItem});
            this.optionToolStripMenuItem.Name = "optionToolStripMenuItem";
            this.optionToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.optionToolStripMenuItem.Text = "Option";
            // 
            // debugViewToolStripMenuItem
            // 
            this.debugViewToolStripMenuItem.Name = "debugViewToolStripMenuItem";
            this.debugViewToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.debugViewToolStripMenuItem.Text = "Debug view";
            // 
            // ChessMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.gameBoard);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "ChessMainForm";
            this.Text = "Chess 1.0";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem gameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private GameBoard gameBoard;
        private System.Windows.Forms.ToolStripMenuItem optionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debugViewToolStripMenuItem;
    }
}
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
            Chess.Engine.MoveGenerator moveGenerator1 = new Chess.Engine.MoveGenerator();
            Chess.Engine.AttackDatabase attackDatabase1 = new Chess.Engine.AttackDatabase();
            Chess.Engine.BitBoard bitBoard1 = new Chess.Engine.BitBoard();
            Chess.Engine.GameHistory gameHistory1 = new Chess.Engine.GameHistory();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChessMainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.gameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.actionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.revertMoveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gameBoard = new Chess.GUI.GameBoard();
            this.openFileDialogSave = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialogSave = new System.Windows.Forms.SaveFileDialog();
            this.buildAttackDBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gameToolStripMenuItem,
            this.optionToolStripMenuItem,
            this.actionsToolStripMenuItem});
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
            this.newToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // optionToolStripMenuItem
            // 
            this.optionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.debugViewToolStripMenuItem,
            this.buildAttackDBToolStripMenuItem});
            this.optionToolStripMenuItem.Name = "optionToolStripMenuItem";
            this.optionToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.optionToolStripMenuItem.Text = "Option";
            // 
            // debugViewToolStripMenuItem
            // 
            this.debugViewToolStripMenuItem.Name = "debugViewToolStripMenuItem";
            this.debugViewToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.debugViewToolStripMenuItem.Text = "Debug view";
            this.debugViewToolStripMenuItem.Click += new System.EventHandler(this.debugViewToolStripMenuItem_Click);
            // 
            // actionsToolStripMenuItem
            // 
            this.actionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.revertMoveToolStripMenuItem});
            this.actionsToolStripMenuItem.Name = "actionsToolStripMenuItem";
            this.actionsToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.actionsToolStripMenuItem.Text = "Actions";
            // 
            // revertMoveToolStripMenuItem
            // 
            this.revertMoveToolStripMenuItem.Enabled = false;
            this.revertMoveToolStripMenuItem.Name = "revertMoveToolStripMenuItem";
            this.revertMoveToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.revertMoveToolStripMenuItem.Text = "Revert Move";
            this.revertMoveToolStripMenuItem.Click += new System.EventHandler(this.revertMoveToolStripMenuItem_Click);
            // 
            // gameBoard
            // 
            this.gameBoard.ActiveColor = 1;
            this.gameBoard.GameRunning = false;
            this.gameBoard.Location = new System.Drawing.Point(12, 27);
            moveGenerator1.AttackDatabase = attackDatabase1;
            moveGenerator1.CurrentGame = null;
            bitBoard1.AttackedBy = null;
            bitBoard1.AttackedByBlack = ((ulong)(0ul));
            bitBoard1.AttackedByWhite = ((ulong)(0ul));
            bitBoard1.Blackbishops = ((ulong)(0ul));
            bitBoard1.BlackKing = ((ulong)(0ul));
            bitBoard1.BlackKingCheck = false;
            bitBoard1.BlackKnights = ((ulong)(0ul));
            bitBoard1.BlackPawns = ((ulong)(0ul));
            bitBoard1.BlackPieces = ((ulong)(0ul));
            bitBoard1.BlackQueens = ((ulong)(0ul));
            bitBoard1.BlackRooks = ((ulong)(0ul));
            bitBoard1.EmptySquares = ((ulong)(0ul));
            bitBoard1.ProtecteddBy = null;
            bitBoard1.SquarsBlocked = ((ulong)(0ul));
            bitBoard1.WhiteBishops = ((ulong)(0ul));
            bitBoard1.WhiteKing = ((ulong)(0ul));
            bitBoard1.WhiteKingCheck = false;
            bitBoard1.WhiteKnights = ((ulong)(0ul));
            bitBoard1.WhitePawns = ((ulong)(0ul));
            bitBoard1.WhitePieces = ((ulong)(0ul));
            bitBoard1.WhiteQueens = ((ulong)(0ul));
            bitBoard1.WhiteRooks = ((ulong)(0ul));
            moveGenerator1.CurrentGameState = bitBoard1;
            moveGenerator1.GameRunning = false;
            gameHistory1.ActiveColor = 1;
            gameHistory1.History = ((System.Collections.Generic.Dictionary<Chess.Engine.GameInfo, System.Collections.Generic.List<Chess.Engine.BitBoard>>)(resources.GetObject("gameHistory1.History")));
            gameHistory1.MoveGenerator = moveGenerator1;
            moveGenerator1.History = gameHistory1;
            this.gameBoard.MoveGenerator = moveGenerator1;
            this.gameBoard.Name = "gameBoard";
            this.gameBoard.OffsetX = 5;
            this.gameBoard.OffsetY = 5;
            this.gameBoard.Selected = false;
            this.gameBoard.SelectedFigure = null;
            this.gameBoard.SelectedX = -1;
            this.gameBoard.SelectedY = -1;
            this.gameBoard.Size = new System.Drawing.Size(760, 523);
            this.gameBoard.TabIndex = 1;
            // 
            // openFileDialogSave
            // 
            this.openFileDialogSave.FileName = "openFileDialog1";
            // 
            // buildAttackDBToolStripMenuItem
            // 
            this.buildAttackDBToolStripMenuItem.Name = "buildAttackDBToolStripMenuItem";
            this.buildAttackDBToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.buildAttackDBToolStripMenuItem.Text = "Build Attack DB";
            this.buildAttackDBToolStripMenuItem.Click += new System.EventHandler(this.buildAttackDBToolStripMenuItem_Click);
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
        private System.Windows.Forms.ToolStripMenuItem actionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem revertMoveToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialogSave;
        private System.Windows.Forms.SaveFileDialog saveFileDialogSave;
        private System.Windows.Forms.ToolStripMenuItem buildAttackDBToolStripMenuItem;
    }
}
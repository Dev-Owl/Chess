using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chess.Log;
using Chess.Tools;
using System.Diagnostics;
using System.IO;
using Chess.GUI;
using ABChess.Engine;

namespace Chess.GUI.Forms
{
    public partial class ChessMainForm : Form
    {

        Process mongoProcess;
        DebugViewFrom debugView;
        string DEFAULT_FILTER = "Chess saves (*.chess)|*.chess";
        string DEFAULT_DB_PATH = @"data\db\";
        string DEFAULT_AI_PATH = @"data\ai\";

        public ChessMainForm()
        {
            InitializeComponent();
            Directory.CreateDirectory(DEFAULT_DB_PATH);
            Directory.CreateDirectory(DEFAULT_AI_PATH);
            //StartMongoDB();
            this.FormClosing += MainForm_FormClosing;
            debugView = new DebugViewFrom(this.gameBoard);
            saveFileDialogSave.SupportMultiDottedExtensions = true;
            saveFileDialogSave.Filter = DEFAULT_FILTER;
            saveFileDialogSave.FileName = "";
            openFileDialogSave.CheckFileExists = true;
            openFileDialogSave.Filter = DEFAULT_FILTER;
            openFileDialogSave.FileName = "";
            this.gameBoard.PropertyChange += gameBoard_PropertyChange;
            this.saveToolStripMenuItem.Enabled = false;
            this.debugViewToolStripMenuItem.Enabled = false;
            this.Text =string.Format("ABChess Version {0}", Program.VERSION);
            this.gameBoard.AILoader = new AILoader(DEFAULT_AI_PATH);
            //Load the AI plug ins
            this.gameBoard.AILoader.Run();
        }

        void gameBoard_PropertyChange(string Event, ChangedEventArgs e)
        {
            switch (Event)
            {
                case "New Game": {
                    this.debugViewToolStripMenuItem.Enabled = true;
                    this.saveToolStripMenuItem.Enabled = true;
                    this.revertMoveToolStripMenuItem.Enabled = true;
                } break;
            }
        }

        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are you sure to quite the game ?", "Warning", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
            {
                //StopMongoDB();
                e.Cancel = true;
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartNewGameFrom newGameForm = new StartNewGameFrom(this);
            newGameForm.ShowDialog(this);
        }

        public void StartNewGame(GameInfo NewGame)
        {
            this.gameBoard.StartNewGame(NewGame);
            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void revertMoveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.gameBoard.MoveGenerator.History.RevertLastMove())
            {
                this.gameBoard.MoveReverted();
            }
        }

        private void debugViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            debugView.Show(this);
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialogSave.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.gameBoard.LoadGame(openFileDialogSave.FileName);
                this.gameBoard_PropertyChange("New Game", null);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialogSave.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                this.gameBoard.SaveGame(saveFileDialogSave.FileName);
            }
        }

        private void buildAttackDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AttackDatabase attackDB = new AttackDatabase( new Thinking());
            attackDB.BuildAttackboard();
        }

        private void reloadAIPluginsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.gameBoard.GameRunning)
            {
                MessageBox.Show("You can not reload the AI Plugins during a running game!");
            }
            else
            { 
                //Reload the AI 
            }
        }

        public AILoader GetAILoader()
        {
            return this.gameBoard.AILoader;
        }
    }
}

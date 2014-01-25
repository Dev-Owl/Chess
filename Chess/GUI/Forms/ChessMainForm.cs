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

        public ChessMainForm()
        {
            InitializeComponent();
            Directory.CreateDirectory(@"data\db\");
            StartMongoDB();
            this.FormClosing += MainForm_FormClosing;
            debugView = new DebugViewFrom(this.gameBoard);
            saveFileDialogSave.SupportMultiDottedExtensions = true;
            openFileDialogSave.CheckFileExists = true;
            this.gameBoard.PropertyChange += gameBoard_PropertyChange;
            this.saveToolStripMenuItem.Enabled = false;
            this.debugViewToolStripMenuItem.Enabled = false;
            this.Text =string.Format("ABChess Version {0}", Program.VERSION);
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
            if (MessageBox.Show("Are you sure to quite the game ?", "Warning", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                StopMongoDB();
            }
            else {
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

        private void StartMongoDB()
        {
            this.mongoProcess = new Process();

            this.mongoProcess.StartInfo.Arguments = "--dbpath \"" + Path.Combine(Environment.CurrentDirectory, @"data\db").Replace("\\", "\\") + "\"";
            this.mongoProcess.StartInfo.UseShellExecute = false;
            this.mongoProcess.StartInfo.CreateNoWindow = true;
            this.mongoProcess.StartInfo.FileName = @"data\mongod.exe ";
            this.mongoProcess.Start();
        }

        private void StopMongoDB()
        {
            if (!this.mongoProcess.HasExited)
            {
                this.mongoProcess.Kill();
            }
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

        private void ChessMainForm_Load(object sender, EventArgs e)
        {

        }
    }
}

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
using Chess.Engine;

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
            this.revertMoveToolStripMenuItem.Enabled = true;
        }

        private void StartMongoDB()
        {
            this.mongoProcess = new Process();

            this.mongoProcess.StartInfo.Arguments = "--dbpath \"" + Path.Combine(Environment.CurrentDirectory, @"data\db").Replace("\\", "\\") + "\"";
            this.mongoProcess.StartInfo.UseShellExecute = false;
            this.mongoProcess.StartInfo.CreateNoWindow = false;
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
            if (MessageBox.Show("Are you sure to quite the game ?", "Warning", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                this.Close();
            }
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
    }
}

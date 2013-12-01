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

        public ChessMainForm()
        {
            InitializeComponent();
            Directory.CreateDirectory(@"data\db\");
            StartMongoDB();
            this.FormClosing += MainForm_FormClosing;

        }

        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopMongoDB();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.gameBoard.StartNewGame();
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
            this.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ABChess.Engine;

namespace Chess.GUI.Forms
{
    public partial class StartNewGameFrom : Form
    {
        ChessMainForm mainForm = null;

        public StartNewGameFrom( ChessMainForm MainForm)
        {
            this.mainForm = MainForm;
            InitializeComponent();
        }

        private void buttonCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void StartNewGameFrom_Load(object sender, EventArgs e)
        {
            
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            GameInfo newGame = new GameInfo();
            if (radioWhiteHuman.Checked)
            {
                newGame.Player1 = "Human";
                newGame.Player1IsAI = false;
            }
            else
            {
                newGame.Player1 = "Computer";
                newGame.Player1IsAI = true;
            }

            if (radioBlackHuman.Checked)
            {
                newGame.Player2 = "Human";
                newGame.Player2IsAI = false;
            }
            else
            {
                newGame.Player2 = "Computer";
                newGame.Player2IsAI = true;
            }
            newGame.StartingTime = (UInt64)(DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
            this.mainForm.StartNewGame(newGame);
            this.Close();
        }
    }
}

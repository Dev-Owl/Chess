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
using ABChess.Tools;


namespace Chess.GUI.Forms
{
    public partial class DebugViewFrom : Form
    {
        private GameBoard board;

        public DebugViewFrom(GameBoard Board)
        {
            InitializeComponent();
            this.board = Board;
            this.board.PropertyChange += board_PropertyChange;
            this.FormClosing += DebugViewFrom_FormClosing;
        }

        void DebugViewFrom_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
           
        }

        void board_PropertyChange(string Event, ChangedEventArgs e)
        {
            this.listBoxEvents.Items.Add(Event);
            switch (Event)
            {
                case "New Game":
                    {
                        this.textBoxDataView.Text= BitOperations.CreateHumanString(this.board.MoveGenerator.CurrentGameState.SquarsBlocked);
                    } break;
                case "Figure selected":
                    {
                        this.textBoxDataView.Text = BitOperations.CreateHumanString((UInt64)e.EventData);
                    } break;
                case "Figure moved":
                    {
                        this.textBoxDataView.Text = BitOperations.CreateHumanString(this.board.MoveGenerator.CurrentGameState.SquarsBlocked);
                    }
                    break;
                case "Move reverted":
                    {
                        this.textBoxDataView.Text = BitOperations.CreateHumanString(this.board.MoveGenerator.CurrentGameState.SquarsBlocked);
                    }break;
                case "Game loaded":
                    {
                        this.textBoxDataView.Text = BitOperations.CreateHumanString((UInt64)e.EventData);
                    }break;
            }
        }

        private void DebugViewFrom_Load(object sender, EventArgs e)
        {
            this.listBoxEvents.Focus();
        }
    }
}

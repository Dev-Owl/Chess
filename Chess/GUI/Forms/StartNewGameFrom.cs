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
        AILoader mainLoader;

        public StartNewGameFrom( ChessMainForm MainForm)
        {
            this.mainForm = MainForm;
            InitializeComponent();
            this.mainLoader = this.mainForm.GetAILoader();
        }

        private void buttonCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void StartNewGameFrom_Load(object sender, EventArgs e)
        {
            BuildAIList();
        }

        private void BuildAIList()
        {
            List<string> LoadedAI = new List<string>();
            foreach ( AIInvoker singleAI in mainLoader.InvokerObjects)
            {
                if (singleAI.Loaded)
                {
                    LoadedAI.Add(singleAI.AIDescription.ModuleName);
                }
            }
            AddItems(comboBox1, LoadedAI);
            AddItems(comboBox3, LoadedAI);
            this.radioWhiteAI.Enabled = true;
            this.radioBlackAI.Enabled = true;
        }
     
        private void AddItems(ComboBox Box, List<string> AIElements)
        {
            Box.Items.AddRange(AIElements.ToArray());
            Box.Enabled = true;
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
                if (this.comboBox1.SelectedItem != null)
                {
                    newGame.Player1 = this.comboBox1.SelectedItem.ToString();
                    newGame.Player1IsAI = true;
                    newGame.AI1 = GetAIByName(newGame.Player1);
                }
                else
                {
                    MessageBox.Show("Please select an AI for the white player");
                    return;
                }
            }

            if (radioBlackHuman.Checked)
            {
                newGame.Player2 = "Human";
                newGame.Player2IsAI = false;
            }
            else
            {
                if (this.comboBox3.SelectedItem != null)
                {
                    newGame.Player2 = this.comboBox3.SelectedItem.ToString();
                    newGame.Player2IsAI = true;
                    newGame.AI2 = GetAIByName(newGame.Player2);
                }
                else {
                    MessageBox.Show("Please select an AI for the black player");
                    return;
                }
                
            }
            newGame.StartingTime = (UInt64)(DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
            this.mainForm.StartNewGame(newGame);
            this.Close();
        }

        private void radioWhiteAI_CheckedChanged(object sender, EventArgs e)
        {
            
        }

		public IAI GetAIByName( string Name)
		{
			foreach (AIInvoker singleAI in mainLoader.InvokerObjects) 
			{
				if (singleAI.AIDescription.ModuleName == Name) {
					return singleAI.Clone().GetAIInterfacObject();
				}
			}
			return null;
		}
    }
}

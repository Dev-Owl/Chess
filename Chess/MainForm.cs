using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chess.Log;


namespace Chess
{
    public partial class MainForm : Form
    {
        EventLog fromLog;

        public MainForm()
        {
            InitializeComponent();
            this.fromLog = new EventLog(this.textBox1);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.fromLog.Log("Loading complete");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.panel1.StartNewGame();
        }
    }
}

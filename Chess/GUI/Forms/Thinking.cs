using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ABChess.Engine;

namespace Chess
{
    public partial class Thinking : Form , IThinking
    {
        public Thinking()
        {
            InitializeComponent();
            
        }

        private void Thinking_Load(object sender, EventArgs e)
        {
            this.BringToFront();
        }

        public void SetMessage(string Message)
        {
            if (this.labelCurrentStep.InvokeRequired)
            {
                labelCurrentStep.Invoke(new Action(() => labelCurrentStep.Text = Message));
            }
            else
            {
                this.labelCurrentStep.Text = Message;
            }
        }


        public new void Close()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => base.Close()));
            }
            else
            {
                base.Close();
            }
        }

        
    }
}

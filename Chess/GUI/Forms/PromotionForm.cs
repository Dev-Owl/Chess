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
    public partial class PromotionForm : Form
    {

       
        public EFigures Result { get; set; }

        public PromotionForm()
        {
            InitializeComponent();
            Result = EFigures.NAN;
        }

        private void SaveAndClose(EFigures Result)
        {
            this.Result = Result;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.SaveAndClose(EFigures.Queen);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.SaveAndClose(EFigures.Rook);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.SaveAndClose(EFigures.Bishop);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.SaveAndClose(EFigures.Knight);
        }

    }
}

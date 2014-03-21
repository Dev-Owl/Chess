namespace Chess.GUI.Forms
{
    partial class StartNewGameFrom
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonCancle = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.radioWhiteAI = new System.Windows.Forms.RadioButton();
            this.radioWhiteHuman = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.radioBlackAI = new System.Windows.Forms.RadioButton();
            this.radioBlackHuman = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(416, 118);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonCancle
            // 
            this.buttonCancle.Location = new System.Drawing.Point(326, 118);
            this.buttonCancle.Name = "buttonCancle";
            this.buttonCancle.Size = new System.Drawing.Size(75, 23);
            this.buttonCancle.TabIndex = 1;
            this.buttonCancle.Text = "Cancle";
            this.buttonCancle.UseVisualStyleBackColor = true;
            this.buttonCancle.Click += new System.EventHandler(this.buttonCancle_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.radioWhiteAI);
            this.groupBox1.Controls.Add(this.radioWhiteHuman);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 100);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "White";
            // 
            // comboBox1
            // 
            this.comboBox1.Enabled = false;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(40, 63);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 11;
            // 
            // radioWhiteAI
            // 
            this.radioWhiteAI.AutoSize = true;
            this.radioWhiteAI.Enabled = false;
            this.radioWhiteAI.Location = new System.Drawing.Point(43, 40);
            this.radioWhiteAI.Name = "radioWhiteAI";
            this.radioWhiteAI.Size = new System.Drawing.Size(70, 17);
            this.radioWhiteAI.TabIndex = 10;
            this.radioWhiteAI.Text = "Computer";
            this.radioWhiteAI.UseVisualStyleBackColor = true;
            this.radioWhiteAI.CheckedChanged += new System.EventHandler(this.radioWhiteAI_CheckedChanged);
            // 
            // radioWhiteHuman
            // 
            this.radioWhiteHuman.AutoSize = true;
            this.radioWhiteHuman.Checked = true;
            this.radioWhiteHuman.Location = new System.Drawing.Point(43, 16);
            this.radioWhiteHuman.Name = "radioWhiteHuman";
            this.radioWhiteHuman.Size = new System.Drawing.Size(59, 17);
            this.radioWhiteHuman.TabIndex = 9;
            this.radioWhiteHuman.TabStop = true;
            this.radioWhiteHuman.Text = "Human";
            this.radioWhiteHuman.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBox3);
            this.groupBox2.Controls.Add(this.radioBlackAI);
            this.groupBox2.Controls.Add(this.radioBlackHuman);
            this.groupBox2.Location = new System.Drawing.Point(291, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 100);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Black";
            // 
            // comboBox3
            // 
            this.comboBox3.Enabled = false;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(40, 63);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(121, 21);
            this.comboBox3.TabIndex = 11;
            // 
            // radioBlackAI
            // 
            this.radioBlackAI.AutoSize = true;
            this.radioBlackAI.Enabled = false;
            this.radioBlackAI.Location = new System.Drawing.Point(43, 40);
            this.radioBlackAI.Name = "radioBlackAI";
            this.radioBlackAI.Size = new System.Drawing.Size(70, 17);
            this.radioBlackAI.TabIndex = 10;
            this.radioBlackAI.Text = "Computer";
            this.radioBlackAI.UseVisualStyleBackColor = true;
            // 
            // radioBlackHuman
            // 
            this.radioBlackHuman.AutoSize = true;
            this.radioBlackHuman.Checked = true;
            this.radioBlackHuman.Location = new System.Drawing.Point(43, 16);
            this.radioBlackHuman.Name = "radioBlackHuman";
            this.radioBlackHuman.Size = new System.Drawing.Size(59, 17);
            this.radioBlackHuman.TabIndex = 9;
            this.radioBlackHuman.TabStop = true;
            this.radioBlackHuman.Text = "Human";
            this.radioBlackHuman.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(233, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 25);
            this.label1.TabIndex = 12;
            this.label1.Text = "VS";
            // 
            // StartNewGameFrom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 147);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonCancle);
            this.Controls.Add(this.buttonStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StartNewGameFrom";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Start new Game";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.StartNewGameFrom_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonCancle;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.RadioButton radioWhiteAI;
        private System.Windows.Forms.RadioButton radioWhiteHuman;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.RadioButton radioBlackAI;
        private System.Windows.Forms.RadioButton radioBlackHuman;
        private System.Windows.Forms.Label label1;
    }
}
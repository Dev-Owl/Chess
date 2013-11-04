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
using Chess.Tools;
using System.Diagnostics;
using System.IO;
using Chess.GUI;
using Chess.Engine;

namespace Chess
{
    public partial class MainForm : Form
    {
        Chess.Log.EventLog fromLog;
        Process mongoProcess;        
        public MainForm()
        {
            InitializeComponent();
            this.fromLog = new Chess.Log.EventLog(this.textBox1);
            this.panel1.PropertyChange += panel1_PropertyChange;
            Directory.CreateDirectory(@"data\db\");
            StartMongoDB();
            this.FormClosing += MainForm_FormClosing;

        }

        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopMongoDB();
        }

        void panel1_PropertyChange(string Event,ChangedEventArgs e)
        {
            switch (Event)
            {
                case "New Game":
                    {
                        this.binaryViewBox.Text = BitOperations.CreateHumanString(this.panel1.MoveGenerator.CurrentGameState.SquarsBlocked);
                        this.fromLog.Log(Event);        
                    }break;
                case "Figure selected":
                    {
                        this.binaryViewBox.Text = BitOperations.CreateHumanString((UInt64)e.EventData);
                        this.fromLog.Log(Event);  
                    }break;
                default:
                    {
                        this.fromLog.Log(Event);
                    }
                    break;
            }
            

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.fromLog.Log("Loading complete");
         
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.panel1.StartNewGame();
        }

        private void StartMongoDB()
        {
            this.mongoProcess = new Process();

            this.mongoProcess.StartInfo.Arguments = "--dbpath \"" + Path.Combine(Environment.CurrentDirectory, @"data\db").Replace("\\","\\") + "\""; 
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

        private void button2_Click(object sender, EventArgs e)
        {
            AttackDatabase db = new AttackDatabase();
            db.BuildAttackboard();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UInt64 attacked = 0;
            UInt64 shift = 1;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            for (UInt64 i = 0; i < 64; ++i)
            {
                Figure fig =this.panel1.MoveGenerator.GetFigureAtPosition(i);
                if( fig!=null)
                    attacked |= this.panel1.MoveGenerator.GetMoveForFigure(fig, (short)i);
                shift = shift << 1;
            }   
            watch.Stop();
            MessageBox.Show("Time " + watch.ElapsedMilliseconds.ToString());
            this.binaryViewBox.Text = BitOperations.CreateHumanString(attacked);
        }
    }
}

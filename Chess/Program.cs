using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Chess
{
    static class Program
    {
        public static string VERSION = "0.001";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Chess.GUI.Forms.ChessMainForm());
            }
            catch (Exception ex)
            {
                try
                {
                    MessageBox.Show("I´m very sorry but an error occourd. The log was written to the file error.log inside the application directory, please send it to c.muehle18@googlemail.com", "Oops....", MessageBoxButtons.OK);
                    File.WriteAllText("error.log",string.Format("Please send the log to c.muehle18@googlemail.com"+Environment.NewLine+"Porgram version: {0}" + Environment.NewLine + "OS Info: {1}"
                                      + Environment.NewLine + "Error: {2}",Program.VERSION,Environment.OSVersion.Platform.ToString() + Environment.OSVersion.VersionString, ex.ToString()));
                    Application.Exit();
                }
                catch 
                {
                }
            }
        }
    }
}

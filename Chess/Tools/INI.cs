using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Chess.Tools
{
    public class INI : IDisposable
    {
        private string workingSection;
        private string pathToFile;
        private StreamReader sr;
        private Dictionary<String, List<string[]>> content;
    

        public INI(string PathToFile)
        {
            if (!File.Exists(PathToFile))
            {
                throw new Exception(String.Format("Unable to find file in {0}", PathToFile));
            }
            this.pathToFile = PathToFile;
            this.content = new Dictionary<string, List<string[]>>();
            Read();
        }

        ~INI()
        {
            Dispose();
        }

        public void Read()
        {
            Clear();
            try 
            {
                if (sr != null)
                {
                    sr.Dispose();
                }
                sr = new StreamReader(this.pathToFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during opening the file. System message:"+ ex.Message,"Error",MessageBoxButtons.OK);
            }
            while (!sr.EndOfStream)
            {
                ParseLine(sr.ReadLine());
            }
            sr.Dispose();
        }

        private void ParseLine(string Line)
        {
            if (!String.IsNullOrEmpty(Line))
            {
                switch (Line[0])
                {
                    case '[':
                        {
                            workingSection = Line.Replace("[", "").Replace("]", "");
                            this.content.Add(workingSection, new List<string[]>());
                        }
                        break;
                    default:
                        {
                            if (!Line.StartsWith("#"))
                            {
                                this.content[workingSection].Add(Line.Split(new char[] { '=' }));
                            }
                        }
                        break;
                }
            }
        }

        public void Clear()
        {
            content.Clear();
            workingSection = string.Empty;
        }

        public string Get(string Section, string Parameter, string DefaultValue = "")
        {
            if (this.content.ContainsKey(Section))
            {
                string[] retValue = this.content[Section].Find(k=> k.Contains<String>(Parameter));
                if(retValue == null)
                {
                    return DefaultValue;
                }
                else
                {
                    return retValue[1];
                }
            }
            else
            {
                return DefaultValue;
            }
        }

        public void Dispose()
        {
            
            if (sr != null)
            {
                sr.Dispose();
            }
            GC.SuppressFinalize(this);
        }

      
        
    }
}

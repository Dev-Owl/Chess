using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ABChess.Engine
{
    public class AILoader
    {
        private string loadingPath;
        public string LoadingPath
        {
            get { return loadingPath; }
            set { loadingPath = value; }
        }

        List<AIInvoker> invokerObjects;

        public List<AIInvoker> InvokerObjects
        {
            get { return invokerObjects; }
            set { invokerObjects = value; }
        }

        public AILoader(string PathToCheck)
        {
            this.loadingPath = PathToCheck;
            invokerObjects = new List<AIInvoker>();
        }

        public void Run()
        {
            this.invokerObjects.Clear();
            string[] directorys = Directory.GetDirectories(this.loadingPath);
            foreach (string subDirecotry in directorys)
            {
                string[] files = Directory.GetFiles(subDirecotry, "*.xml");
                foreach (string file in files)
                {
                    var invokerObj = new AIInvoker(Path.GetFullPath(file), Path.GetFullPath(file.Replace(".xml", ".dll")));
                    invokerObj.Load();
                    invokerObjects.Add(invokerObj);
                }
            }
        }
    }
    
}

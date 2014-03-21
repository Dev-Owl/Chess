using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;


namespace ABChess.Engine
{
    public class AIInvoker
    {
        string descriptionFile;
        public string DescriptionFile
        {
            get { return descriptionFile; }
            set { descriptionFile = value; }
        }

        string dllFile;
        public string DllFile
        {
            get { return dllFile; }
            set { dllFile = value; }
        }

        bool loaded;
        public bool Loaded
        {
            get { return loaded; }
            set { loaded = value; }
        }

        AIDescription aiDescription;
        public AIDescription AIDescription
        {
            get { return aiDescription; }
            set { aiDescription = value; }
        }

        private Type loadedType;
        private Object loadedObject;

        public Object LoadedObject
        {
            get { return loadedObject; }
            set { loadedObject = value; }
        }


        public AIInvoker(string DescriptionFile, string DllFile)
        {
            this.descriptionFile = DescriptionFile;
            this.dllFile = DllFile;
            this.loaded = false;
            if (!File.Exists(descriptionFile) || !File.Exists(dllFile))
            {
                throw new FileNotFoundException(String.Format("The File for plugin {0} was not found",dllFile), dllFile);
            }
        }

        public void Load()
        {
            this.loaded = false;
            using (StreamReader sr = new StreamReader(this.descriptionFile))
            {
                this.aiDescription = new AIDescription();
                this.aiDescription.FromXML(sr);
            }
            Assembly DLL = Assembly.LoadFile(this.dllFile);
            loadedType = DLL.GetType(this.aiDescription.MainClassName);
            loadedObject = Activator.CreateInstance(loadedType, this.aiDescription.ConstructorArgument);
            this.loaded = true;
        }

        public void RunDefaultMethod()
        {
            loadedType.GetMethod(this.aiDescription.MainClassStartMethod).Invoke(this.loadedObject, null);
        }

        public IAI GetAIInterfacObject()
        {
            IAI result = null;
            if (this.loaded)
            {
                result = this.loadedObject as IAI;
            }
            return result;
        }

        public AIInvoker Clone()
        {
            var invoker = new AIInvoker(this.descriptionFile, this.dllFile);
            invoker.Load();
            return invoker;
        }
    }

}

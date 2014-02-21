using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ABChess.Engine
{
    public class AIDescription
    {
        public string ModuleName { get; set; }
        public string MainClassName { get; set; }
        public string MainClassStartMethod { get; set; }
        public string[] ConstructorArgument { get; set; }

        public void ToXML(StreamWriter sw)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AIDescription));
            serializer.Serialize(sw, this);
            sw.Close();
        }

        public void FromXML(StreamReader sr)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AIDescription));
            AIDescription tmp = serializer.Deserialize(sr) as AIDescription;
            this.ModuleName = tmp.ModuleName;
            this.MainClassName = tmp.MainClassName;
            this.MainClassStartMethod = tmp.MainClassStartMethod;
            this.ConstructorArgument = tmp.ConstructorArgument;
            sr.Close();
        }
    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NetMastery.Lab02CLR.Formatters.BuiltInFormatters
{
    [Serializable]
    public class XmlMethodNode
    {
        
        public TimeSpan ExecutionTime { get; set; }
        
        public string MethodName { get; set; }
        
        public string ClassName { get; set; }
        
        public int ParametrCounts { get; set; }
        [XmlArray("ChildNodes"), XmlArrayItem(typeof(XmlMethodNode), ElementName = "MethodNode")]
        public List<XmlMethodNode> ChildNodes { get; set; }
    }
    [Serializable]
    public class XmlThreadNode
    {
        
        public int ThreadId { get; set; }
        
        public string ThreadName { get; set; }
        [XmlArray("Root"), XmlArrayItem(typeof(XmlMethodNode), ElementName = "MethodNode")]
        public List<XmlMethodNode> Root { get; set; }
        
        public TimeSpan OverallTime { get; set; }
    }
    [Serializable]
    public class XmlTraceResult
    {
        [XmlArray("Root"), XmlArrayItem(typeof(XmlThreadNode), ElementName = "ThreadNode")]
        public List<XmlThreadNode> Root { get; set; }
    }
}

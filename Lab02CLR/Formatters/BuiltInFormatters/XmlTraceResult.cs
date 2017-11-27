using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace NetMastery.Lab02CLR.Formatters.BuiltInFormatters
{
    [Serializable]
    public class XmlMethodNode
    {
        [XmlAttribute(AttributeName = "time")]
        public double ExecutionTime { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string MethodName { get; set; }
        [XmlAttribute(AttributeName = "class")]
        public string ClassName { get; set; }
        [XmlAttribute(AttributeName = "params")]
        public int ParametrCounts { get; set; }
        [XmlElement(ElementName = "method")]
        public List<XmlMethodNode> ChildNodes { get; set; }
    }
    [Serializable]
    public class XmlThreadNode
    {
        [XmlAttribute(AttributeName = "id")]
        public int ThreadId { get; set; }
        [XmlIgnore]
        public string ThreadName { get; set; }
        [XmlElement(ElementName = "method")]
        public List<XmlMethodNode> Root { get; set; }
        [XmlAttribute(AttributeName = "time")]
        public int OverallTime { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "root")]
    public class XmlTraceResult
    {
        [XmlElement(ElementName = "thread")]
        public List<XmlThreadNode> Root { get; set; }
    }
}

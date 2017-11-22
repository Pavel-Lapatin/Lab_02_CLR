using System;
using System.Collections.Generic;

using System.Xml.Serialization;
using System.Xml;

using NetMastery.Lab02CLR.Formatters.FormatterPluginContract;

namespace NetMastery.Lab02CLR.Tracer.DataLayer
{
    [Serializable]
    public class ThreadNode : IThreadNode
    {
        public int ThreadId { get; set; }
        public string ThreadName { get; set; }
        [XmlElement("Method", typeof(MethodNode))]
        public List<IMethodNode> Root { get; set; }
        public TimeSpan OverallTime { get; set; }
        public ThreadNode()
        {

        }
        
    }
}


using NetMastery.Lab02CLR.Formatters.FormatterPluginContract;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace NetMastery.Lab02CLR.Tracer.DataLayer
{
    [Serializable]
    public class TraceResult : ITraceResult
    {
        [XmlElement("Root", typeof(ThreadNode))]
        public List<IThreadNode> Root { get; set; }
        

        public TraceResult()
        {
            Root = new List<IThreadNode>();
        }
    }
        
}

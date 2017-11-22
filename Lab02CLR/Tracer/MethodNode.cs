using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;

using NetMastery.Lab02CLR.Formatters.FormatterPluginContract;

namespace NetMastery.Lab02CLR.Tracer.DataLayer
{
    [Serializable]
    public class MethodNode : IMethodNode
    {
        public TimeSpan ExecutionTime { get; set; }
        public string MethodName { get; set; }
        public string ClassName { get; set; }
        public int ParametrCounts { get; set; }
        [XmlElement("Method", typeof(MethodNode))]
        public List<IMethodNode> ChildNodes { get; set; }
        public MethodNode()
        {

        }
        
    }
}

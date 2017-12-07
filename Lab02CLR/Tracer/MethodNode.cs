using System;
using System.Collections.Generic;
using NetMastery.Lab02CLR.Formatters.FormatterPluginContract;

namespace NetMastery.Lab02CLR.TracerLibrary
{
    [Serializable]
    internal class MethodNode : IMethodNode
    {
        public TimeSpan ExecutionTime { get; set; }
        public string MethodName { get; set; }
        public string ClassName { get; set; }
        public int ParametrCounts { get; set; }
        public List<IMethodNode> ChildNodes { get; set; } 
    }
}


using NetMastery.Lab02CLR.Formatters.FormatterPluginContract;
using System;
using System.Collections.Generic;

namespace NetMastery.Lab02CLR.TracerLibrary
{ 
    [Serializable]
    internal class TraceResult : ITraceResult
    {
        public List<IThreadNode> Root { get; set; }
        public TraceResult()
        {
            Root = new List<IThreadNode>();
        }
    }
        
}

using System;
using System.Collections.Generic;
using NetMastery.Lab02CLR.Formatters.FormatterPluginContract;

namespace NetMastery.Lab02CLR.TracerLibrary
{
    [Serializable]
    internal class ThreadNode : IThreadNode
    {
        public int ThreadId { get; set; }
        public string ThreadName { get; set; }
        public List<IMethodNode> Root { get; set; }
        public TimeSpan OverallTime { get; set; }
    }
}

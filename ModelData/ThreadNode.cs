using FormatterPluginContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelData
{
    [Serializable]
    public class ThreadNode : IThreadNode
    {
        public int ThreadId { get; set; }
        public string ThreadName { get; set; }
        public IList<IMethodNode> Root { get; set; }
        public TimeSpan OverallTime { get; set; }
    }
}

using FormatterPluginContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ModelData
{
    public class TraceResult : ITraceResult
    {
        
        public int ThreadId { get; set; }
        
        public IList<IMethodNode> Root { get; set; }
       
        public TimeSpan OverallTime { get; set; }
       
        public TraceResult()
        {
            Root = new List<IMethodNode>();
        }
    }
}

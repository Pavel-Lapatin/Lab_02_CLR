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
        public IList<IThreadNode> Root { get; set; }

        public TraceResult()
        {
            Root = new List<IThreadNode>();
        }
    }
        
}

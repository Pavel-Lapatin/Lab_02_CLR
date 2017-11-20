using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace FormatterPluginContract
{
    public interface IMethodNode
    {
        [YamlIgnore]
        long StartExecutionTime { get; set; }
        TimeSpan ExecutionTime { get; set; }
        string MethodName { get; set; }
        string ClassName { get; set; }
        int ParametrCounts { get; set; }
        IList<IMethodNode> ChildNodes { get; set; }
    }

    public interface ITraceResult
    {
        int ThreadId { get; set; }
        TimeSpan OverallTime { get; set; }
        IList<IMethodNode> Root { get; set; }
        
    }
}

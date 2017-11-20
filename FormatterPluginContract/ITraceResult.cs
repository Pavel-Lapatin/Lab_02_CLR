using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormatterPluginContract
{
    public interface IMethodNode
    {
        long StartExecutionTime { get; set; }
        TimeSpan ExecutionTime { get; set; }
        string MethodName { get; set; }
        string ClassName { get; set; }
        int ParametrCounts { get; set; }
        IList<IMethodNode> ChildNodes { get; set; }
    }

    public interface IThreadNode
    {
        int ThreadId { get; set; }
        string ThreadName { get; set; }
        IList<IMethodNode> Root { get; set; }
        TimeSpan OverallTime { get; set; }
    }

    public interface ITraceResult
    {
        IList<IThreadNode> Root { get; set; }
    }
}

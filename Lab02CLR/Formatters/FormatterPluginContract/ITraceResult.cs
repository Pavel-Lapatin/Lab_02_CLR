using System;
using System.Collections.Generic;

namespace NetMastery.Lab02CLR.Formatters.FormatterPluginContract
{
    public interface IMethodNode
    {
        TimeSpan ExecutionTime { get; set; }
        string MethodName { get; set; }
        string ClassName { get; set; }
        int ParametrCounts { get; set; }
        List<IMethodNode> ChildNodes { get; set; }
    }

    public interface IThreadNode
    {
        int ThreadId { get; set; }
        string ThreadName { get; set; }
        List<IMethodNode> Root { get; set; }
        TimeSpan OverallTime { get; set; }
    }

    public interface ITraceResult
    {
        List<IThreadNode> Root { get; set; }
    }
}

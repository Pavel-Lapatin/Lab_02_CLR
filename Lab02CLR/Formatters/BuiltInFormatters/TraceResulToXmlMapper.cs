using System.Collections.Generic;
using NetMastery.Lab02CLR.Formatters.FormatterPluginContract;

namespace NetMastery.Lab02CLR.Formatters.BuiltInFormatters
{
    internal static class TraceResulToXmlMapper
    {
        internal static XmlTraceResult MapToXml(ITraceResult traceResult)
        {
            XmlTraceResult xmlTraceResult = new XmlTraceResult
            {
                Root = new List<XmlThreadNode>(),
            };
            foreach (var thread in traceResult.Root)
            {
                xmlTraceResult.Root.Add(ThreadNodeMap(thread));
            }
            return xmlTraceResult;
        }

        private static XmlThreadNode ThreadNodeMap(IThreadNode threadNode)
        {
            var xmlThreadNode = new XmlThreadNode
            {
                OverallTime = threadNode.OverallTime.Milliseconds,
                ThreadId = threadNode.ThreadId,
                ThreadName = threadNode.ThreadName,
                Root = new List<XmlMethodNode>()
            };
            foreach (var method in threadNode.Root)
            {
               xmlThreadNode.Root.Add(MethodNodeMap(method));
            }
            return xmlThreadNode;
        }

        private static XmlMethodNode MethodNodeMap(IMethodNode methodNode)
        {
            var xmlMethodNode = new XmlMethodNode
            {
                ExecutionTime = methodNode.ExecutionTime.TotalMilliseconds,
                ClassName = methodNode.ClassName,
                MethodName = methodNode.MethodName,
                ParametrCounts = methodNode.ParametrCounts,
                ChildNodes = new List<XmlMethodNode>()

            };
            foreach (var method in methodNode.ChildNodes)
            {
                    xmlMethodNode.ChildNodes.Add(MethodNodeMap(method));
            }
            return xmlMethodNode;
        }

    }


}

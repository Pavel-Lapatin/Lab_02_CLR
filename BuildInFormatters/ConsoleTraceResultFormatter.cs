using FormatterPluginContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelData;

namespace BuildInFormatters
{
    public class ConsoleTraceResultFormatter : ITraceResultFormatter
    {
        public string FlagValue { get => "console"; }
        public string Output { get; set; }
        public void Format(ITraceResult traceResult)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine($"root");
            strBuilder.AppendLine($"    thread id {traceResult.ThreadId.ToString()}, time: {traceResult.OverallTime.Ticks} ticks");
            AddMethodNodeInfo(traceResult.Root, strBuilder, 1);
            strBuilder.AppendLine($"    thread");
            strBuilder.AppendLine($"root");
            Output = strBuilder.ToString();
        }

        private void AddMethodNodeInfo(IList<IMethodNode> parentMethod, StringBuilder strBuilder, int pos)
        {
            int i = 0;
            pos++;
            while (parentMethod.Count > i)
            {
                strBuilder.AppendLine($"{String.Concat(Enumerable.Repeat("    ", (pos)))}method Name={parentMethod[i].MethodName}," +
                    $"time={parentMethod[i].ExecutionTime.Ticks} ticks, " +
                    $"class={parentMethod[i].ClassName}, " +
                    $"params={parentMethod[i].ParametrCounts}");
                if (parentMethod[i].ChildNodes.Count != 0)
                {
                    AddMethodNodeInfo(parentMethod[i].ChildNodes, strBuilder, pos);
                }
                i++;
            }
        }

        public string GetFormat()
        {
            return Output;
        }
    }
}

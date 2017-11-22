using NetMastery.Lab02CLR.Formatters.FormatterPluginContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;

namespace NetMastery.Lab02CLR.Formatters.BuiltInFormatters
{
    public class ConsoleTraceResultFormatter : ITraceResultFormatter
    {
        public string FlagValue => "console";
        public string Output { get; set; }

        public string GetFormat()
        {
            return Output;
        }

        public void Format(ITraceResult traceResult)
        {
            var strBuilder = new StringBuilder();
            strBuilder.AppendLine("root");
            for (var i = 0; i < traceResult.Root.Count; i++)
            {
                strBuilder.AppendLine($"    thread id {traceResult.Root[i].ThreadId}, time: {traceResult.Root[i].OverallTime.Ticks} ticks");
                AddMethodNodeInfo(traceResult.Root[i].Root, strBuilder, 1);
                strBuilder.AppendLine("    thread");
            }
            strBuilder.AppendLine("root");
            Output = strBuilder.ToString();
        }

        private void AddMethodNodeInfo(IList<IMethodNode> parentMethod, StringBuilder strBuilder, int pos)
        {
            var i = 0;
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
    }
}

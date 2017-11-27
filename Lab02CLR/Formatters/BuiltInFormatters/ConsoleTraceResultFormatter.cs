using NetMastery.Lab02CLR.Formatters.FormatterPluginContract;
using System.Collections.Generic;
using System.Linq;
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
            foreach (var thread in traceResult.Root)
            {
                strBuilder.AppendFormat("    thread id={0}, time={1} ms\n", thread.ThreadId, thread.ThreadId);
                AddMethodNodeInfo(thread.Root, strBuilder, 1);
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
                strBuilder.AppendFormat("{0}method Name={1}, time={2} ms, class={3}, params={4}\n", string.Concat(Enumerable.Repeat("    ", (pos))),parentMethod[i].MethodName,
                   parentMethod[i].ExecutionTime.Milliseconds, parentMethod[i].ClassName,parentMethod[i].ParametrCounts);

                if (parentMethod[i].ChildNodes.Count != 0)
                {
                    AddMethodNodeInfo(parentMethod[i].ChildNodes, strBuilder, pos);
                }
                i++;
            }
        }
    }
}

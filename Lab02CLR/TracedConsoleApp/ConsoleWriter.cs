using System;
using NetMastery.Lab02CLR.Formatters.FormatterPluginContract;
using NetMastery.Lab02CLR.TracerLibrary;

namespace NetMastery.Lab02CLR.TracedConsoleApp
{
    internal class ConsoleWriter : Writer
    {
        public ConsoleWriter(ITraceResultFormatter traceResult) : base(traceResult) { }
    
        public override void WriteResult(ITraceResult results)
        {
                Formatter.Format(results);
                using (var writer = Console.Out)
                {
                    writer.Write(Formatter.GetFormat());
                }
        }
    }
}

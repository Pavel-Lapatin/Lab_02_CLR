using System;
using NetMastery.Lab02CLR.Formatters.FormatterPluginContract;

namespace NetMastery.Lab02CLR.TracedConsoleApp
{
    internal class WriterToConsole
    {
        readonly ITraceResultFormatter _formatter;
        public WriterToConsole(ITraceResultFormatter formatter)
        {
            _formatter = formatter;
        }

        public void WriteResult(ITraceResult results)
        {
                _formatter.Format(results);
                using (var writer = Console.Out)
                {
                    writer.Write(_formatter.GetFormat());
                }
        }
    }
}

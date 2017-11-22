using NetMastery.Lab02CLR.Formatters.FormatterPluginContract;
using System.IO;

namespace NetMastery.Lab02CLR.Tracer
{
    public class WriterToFile
    {
        readonly ITraceResultFormatter _formatter;
        readonly string _filePath;

        public WriterToFile(ITraceResultFormatter formatter, string filePath)
        {
            _formatter = formatter;
            _filePath = filePath;
        }

        public void WriteResult(ITraceResult results)
        {
            _formatter.Format(results);
            if (_filePath == null)
            {
                throw new FileNotFoundException();
            }
            using (TextWriter writer = File.CreateText(_filePath))
            {
                writer.Write(_formatter.GetFormat());
            }
        }
    }
}

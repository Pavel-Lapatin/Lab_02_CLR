using NetMastery.Lab02CLR.Formatters.FormatterPluginContract;
using System.IO;

namespace NetMastery.Lab02CLR.TracerLibrary
{
    public class FileWriter : Writer
    {

        private readonly string _filePath;

        public FileWriter(ITraceResultFormatter formatter, string filePath) : base(formatter)
        {
            _filePath = filePath;
        }

        public override void WriteResult(ITraceResult results)
        {
            Formatter.Format(results);
            if (_filePath == null)
            {
                throw new FileNotFoundException();
            }
            using (TextWriter writer = File.CreateText(_filePath))
            {
                writer.Write(Formatter.GetFormat());
            }
        }
    }
}

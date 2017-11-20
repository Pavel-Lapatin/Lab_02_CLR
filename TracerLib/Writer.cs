using FormatterPluginContract;
using ModelData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracedConsoleApp
{
    public class Writer
    {
        ITraceResultFormatter _formatter;
        string _filePath;
        public Writer(ITraceResultFormatter formatter, string filePath = null)
        {
            _formatter = formatter;
            _filePath = filePath;
        }

        public void WriteResult(ITraceResult results)
        {
            try
            {
                _formatter.Format(results);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"Couldn't load the assembly {e.FileName}");
            }
            if (_filePath == null)
            {
                using (TextWriter writer = Console.Out)
                { 
                    writer.Write(_formatter.GetFormat());
                }
            }
            else
            {
                try
                {
                    using (TextWriter writer = File.CreateText(_filePath))
                    {
                        writer.Write(_formatter.GetFormat());
                    }
                }
                catch (DirectoryNotFoundException)
                {
                    Console.WriteLine("Directory hasn' been found");
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine("You are not allowed to write into this Directory");
                }
            }
        }
    }
}

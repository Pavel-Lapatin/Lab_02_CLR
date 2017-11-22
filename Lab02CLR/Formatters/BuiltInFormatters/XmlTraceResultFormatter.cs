using NetMastery.Lab02CLR.Formatters.FormatterPluginContract;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace NetMastery.Lab02CLR.Formatters.BuiltInFormatters
{
    public class XmlTraceResultFormatter : ITraceResultFormatter
    {
        public string FlagValue => "xml";
        private string _output;

        public void Format(ITraceResult traceResult)
        {
            var xmlSerializer = new XmlSerializer(typeof(XmlTraceResult));

            using (var textWriter = new StringWriter())
            {
               xmlSerializer.Serialize(textWriter, TraceResulToXmlMapper.MapToXml(traceResult));
               _output = textWriter.ToString();
            };
            
        }

        public string GetFormat()
        {
            return _output;
        }
    }
}

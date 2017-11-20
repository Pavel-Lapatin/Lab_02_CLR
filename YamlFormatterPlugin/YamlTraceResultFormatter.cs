using FormatterPluginContract;
using ModelData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace YamlFormatterPlugin
{
    public class YamlTraceResultFormatter : ITraceResultFormatter
    {
        public string FlagValue { get => "yaml"; }
        public string Output { get; set; }


        public void Format(ITraceResult traceResult)
        {
            var serializer = new SerializerBuilder().Build();
            Output = serializer.Serialize(traceResult);
        }

        public string GetFormat()
        {
            return Output;
        }
    }
}

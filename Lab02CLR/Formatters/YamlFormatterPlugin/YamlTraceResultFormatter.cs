using NetMastery.Lab02CLR.Formatters.FormatterPluginContract;
using YamlDotNet.Serialization;

namespace NetMastery.Lab02CLR.Formatters.YamlFormatterPlugin
{
    public class YamlTraceResultFormatter : ITraceResultFormatter
    {
        public string FlagValue => "yaml";
        private string _output;


        public void Format(ITraceResult traceResult)
        {
            var serializer = new SerializerBuilder().Build();
            _output = serializer.Serialize(traceResult);
        }

        public string GetFormat()
        {
            return _output;
        }
    }
}

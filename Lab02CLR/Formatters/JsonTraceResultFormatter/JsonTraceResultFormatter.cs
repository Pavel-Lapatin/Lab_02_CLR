using Newtonsoft.Json;
using NetMastery.Lab02CLR.Formatters.FormatterPluginContract;

namespace NetMastery.Lab02CLR.Formatters.JsonFormatterPlugin
{
    public class JsonTraceResultFormatter : ITraceResultFormatter
    {
        public string Output { get; set; }

        public string FlagValue => "json";

        public void Format(ITraceResult traceResult)
        {
            var result = JsonConvert.SerializeObject(traceResult, Formatting.Indented);
            Output = result;
        }

        public string GetFormat()
        {
            return Output;
        }
    }
}

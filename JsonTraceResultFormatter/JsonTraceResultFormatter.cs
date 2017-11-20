using FormatterPluginContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JsonTraceResultFormatter
{
    public class JsonTraceResultFormatter : ITraceResultFormatter
    {
        public string Output { get; set; }

        public string FlagValue { get => "json"; }

        public void Format(ITraceResult traceResult)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            Output = JsonConvert.SerializeObject(traceResult.Root);
        }

        public string GetFormat()
        {
            return Output;
        }
    }
}

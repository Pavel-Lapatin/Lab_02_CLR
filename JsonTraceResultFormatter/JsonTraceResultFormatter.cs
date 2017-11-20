using FormatterPluginContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace JsonTraceResultFormatter
{
    public class JsonTraceResultFormatter : ITraceResultFormatter
    {
        public string Output { get; set; }

        public string FlagValue { get => "json"; }

        public void Format(ITraceResult traceResult)
        {
            string result = JsonConvert.SerializeObject(traceResult, Formatting.Indented);
            Output = result;
        }

        public string GetFormat()
        {
            return Output;
        }
    }
}

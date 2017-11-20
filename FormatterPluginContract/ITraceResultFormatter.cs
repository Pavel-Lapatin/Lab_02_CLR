using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormatterPluginContract
{
    public interface ITraceResultFormatter
    {
        string FlagValue { get;}
        string GetFormat();
        void Format(ITraceResult traceResult);
    }
}

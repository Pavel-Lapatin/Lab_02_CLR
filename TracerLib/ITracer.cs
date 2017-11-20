using FormatterPluginContract;
using ModelData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracerLib
{
    public interface ITracer
    {
        void StartTrace();
        void StopTrace();
        ITraceResult GetTraceResult();
    }
}

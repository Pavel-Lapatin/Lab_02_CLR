using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetMastery.Lab02CLR.Tracer.DataLayer
{
    [Serializable]
    public class TracerMethodNode : MethodNode
    {
        [NonSerialized]
        private readonly long _startExecutionTime;

        public TracerMethodNode(long startExecutionTime)
        {
            _startExecutionTime = startExecutionTime;
        }

        public long GetStartExecutionTime()
        {
            return _startExecutionTime;
        }
    }
}

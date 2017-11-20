using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelData
{
    [Serializable]
    public class TracerMethodNode : MethodNode
    {
        [NonSerialized]
        public readonly long _startExecutionTime;
        public TracerMethodNode(long startExecutionTime)
        {
            _startExecutionTime = startExecutionTime;
        }
    }
}

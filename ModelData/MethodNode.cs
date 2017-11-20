using FormatterPluginContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ModelData
{
    [Serializable]
    public class MethodNode : IMethodNode
    {
        public TimeSpan ExecutionTime { get; set; }
        public string MethodName { get; set; }
        public string ClassName { get; set; }
        public int ParametrCounts { get; set; }
        [DataMember(EmitDefaultValue = true)]
        public IList<IMethodNode> ChildNodes { get; set; }
    }
}

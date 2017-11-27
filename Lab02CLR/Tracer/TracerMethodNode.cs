using System;

namespace NetMastery.Lab02CLR.TracerLibrary
{
    [Serializable]
    internal class TracerMethodNode : MethodNode
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

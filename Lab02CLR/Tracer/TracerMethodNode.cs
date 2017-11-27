using System;

namespace NetMastery.Lab02CLR.TracerLibrary
{
    [Serializable]
    internal class TracerMethodNode : MethodNode
    {
        [NonSerialized]
        private readonly long startExecutionTime;

        public TracerMethodNode(long startExecutionTime)
        {
            this.startExecutionTime = startExecutionTime;
        }

        public long GetStartExecutionTime()
        {
            return startExecutionTime;
        }
    }
}

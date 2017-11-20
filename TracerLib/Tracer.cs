using FormatterPluginContract;
using ModelData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TracerLib
{
    public class Tracer : ITracer
    {
        private static Tracer instance = null;
        private Stack<MethodNode> StackForInvokedMethods = new Stack<MethodNode>();
        private static Stopwatch watch = new Stopwatch();
        private TraceResult result = new TraceResult();

        private Tracer()
        {
            result.ThreadId =  Thread.CurrentThread.ManagedThreadId;
            result.Root = new List<IMethodNode>();
        }

        public static Tracer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Tracer();
                }
                return instance;
            }
        }

        public TraceResult GetTraceResult()
        {

            return result;
        }

        public void StartTrace()
        {
            watch.Stop();
            var stackTrace = new StackTrace();
            var frame = stackTrace.GetFrame(1);
            var methodBase = frame.GetMethod();
            var currentMethod = new MethodNode
            {
                MethodName = methodBase.Name,
                ClassName = methodBase.DeclaringType.Name,
                StartExecutionTime = watch.ElapsedTicks,
                ParametrCounts = methodBase.GetParameters().Length,
                ChildNodes = new List<IMethodNode>(),
            };
            if (StackForInvokedMethods.Count == 0)
            {
                result.Root.Add(currentMethod);
            }
            else
            {
                StackForInvokedMethods.Peek().ChildNodes.Add(currentMethod);
            }
            StackForInvokedMethods.Push(currentMethod);
            watch.Start();
        }

        public void StopTrace()
        {
            watch.Stop();
            if (StackForInvokedMethods.Peek() != null)
            {
                var currentMethod = StackForInvokedMethods.Pop();
                long time = watch.ElapsedTicks - currentMethod.StartExecutionTime;
                currentMethod.ExecutionTime = TimeSpan.FromTicks(time);
                result.OverallTime = TimeSpan.FromTicks(result.OverallTime.Ticks + time);
            }

            watch.Start();
        }
    }
}

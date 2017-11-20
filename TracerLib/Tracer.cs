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
        private IDictionary<int, Stack<MethodNode>> StacksForThreads = new Dictionary<int, Stack<MethodNode>>();
        private Stack<MethodNode> StackForInvokedMethods = new Stack<MethodNode>();
        private static Stopwatch watch = new Stopwatch();
        private TraceResult result = new TraceResult();

        private Tracer()
        {
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
            Stack<MethodNode> currentStack;
            var threadId = Thread.CurrentThread.ManagedThreadId;
            var currentThreadNode = result.Root.FirstOrDefault(x => x.ThreadId == threadId);
            if(currentThreadNode != null)
            {
                currentStack = StacksForThreads[threadId];

            }
            else
            {
                currentStack = new Stack<MethodNode>();
                StacksForThreads.Add(threadId, currentStack);
                currentThreadNode = new ThreadNode
                {
                    ThreadId = Thread.CurrentThread.ManagedThreadId,
                    ThreadName = Thread.CurrentThread.Name,
                    Root = new List<IMethodNode>()

                };
                result.Root.Add(currentThreadNode);
            }
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
            if (currentStack.Count == 0)
            {
                currentThreadNode.Root.Add(currentMethod);
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
            var threadId = Thread.CurrentThread.ManagedThreadId;
            var currentThreadNode = result.Root.FirstOrDefault(x => x.ThreadId == threadId);
            if (currentThreadNode != null)
            {
                Console.WriteLine("This is impossible");
            }
            var currentStack = StacksForThreads[threadId];

            if (currentStack.Peek() != null)
            {
                var currentMethod = currentStack.Pop();
                long time = watch.ElapsedTicks - currentMethod.StartExecutionTime;
                currentMethod.ExecutionTime = TimeSpan.FromTicks(time);
                currentThreadNode.OverallTime = TimeSpan.FromTicks(currentThreadNode.OverallTime.Ticks + time);
            }

            watch.Start();
        }
    }
}

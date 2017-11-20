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
        private IDictionary<int, Stack<TracerMethodNode>> StacksForThreads = new Dictionary<int, Stack<TracerMethodNode>>();
        private Stack<TracerMethodNode> StackForInvokedMethods = new Stack<TracerMethodNode>();
        private static Stopwatch watch = new Stopwatch();
        private TraceResult result = new TraceResult();
        private Object _startLock = new Object();
        private Object _stopLock = new Object();
        private static Object  _ctortLock = new Object();

        private Tracer()
        {
        }

        public static Tracer Instance
        {
            get
            {
                lock (_ctortLock)
                {

                    if (instance == null)
                    {
                        instance = new Tracer();
                    }
                }
                return instance;
            }
        }

        public ITraceResult GetTraceResult()
        {

            return result;
        }

        public void StartTrace()
        {
            lock (_startLock)
            {
                watch.Stop();
                Stack<TracerMethodNode> currentStack;
                var threadId = Thread.CurrentThread.ManagedThreadId;
                var currentThreadNode = result.Root.FirstOrDefault(x => x.ThreadId == threadId);
                if (currentThreadNode != null)
                {
                    currentStack = StacksForThreads[threadId];

                }
                else
                {
                    currentStack = new Stack<TracerMethodNode>();
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
                var currentMethod = new TracerMethodNode (watch.ElapsedTicks)
                {
                    MethodName = methodBase.Name,
                    ClassName = methodBase.DeclaringType.Name,
                    ParametrCounts = methodBase.GetParameters().Length,
                    ChildNodes = new List<IMethodNode>(),
                };
                if (currentStack.Count == 0)
                {
                    currentThreadNode.Root.Add((MethodNode)currentMethod);
                }
                else
                {
                    currentStack.Peek().ChildNodes.Add((MethodNode)currentMethod);
                }
                currentStack.Push(currentMethod);
                watch.Start();
            }
        }

        public void StopTrace()
        {
            lock (_stopLock)
            {
                watch.Stop();
                var threadId = Thread.CurrentThread.ManagedThreadId;
                var currentThreadNode = result.Root.FirstOrDefault(x => x.ThreadId == threadId);
                if (currentThreadNode == null)
                {
                    Console.WriteLine("This is impossible");
                }
                var currentStack = StacksForThreads[threadId];

                if (currentStack.Peek() != null)
                {
                    var currentMethod = currentStack.Pop();
                    long time = watch.ElapsedTicks - currentMethod._startExecutionTime;
                    currentMethod.ExecutionTime = TimeSpan.FromTicks(time);
                    currentThreadNode.OverallTime = TimeSpan.FromTicks(currentThreadNode.OverallTime.Ticks + time);
                }
                watch.Start();
            }
        }
    }
}

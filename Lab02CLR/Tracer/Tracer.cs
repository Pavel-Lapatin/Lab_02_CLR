using NetMastery.Lab02CLR.Formatters.FormatterPluginContract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace NetMastery.Lab02CLR.TracerLibrary
{
    public class Tracer : ITracer
    {
        private static Tracer instance;

        private IDictionary<ThreadNode, Stack<TracerMethodNode>> stacksForThreads =
            new Dictionary<ThreadNode, Stack<TracerMethodNode>>();

        private Stopwatch watch = new Stopwatch();
        private object startLock = new object();
        private object stopLock = new object();
        private static object ctortLock = new object();

        public static Tracer Instance
        {
            get
            {
                lock (ctortLock)
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
            var results = new TraceResult();
            foreach (var thread in stacksForThreads.Keys)
            {
                results.Root.Add(thread);
            }
            return results;
        }

        public void StartTrace()
        {
            lock (startLock)
            {
                watch.Stop();
                var threadId = Thread.CurrentThread.ManagedThreadId;
                var currentThreadNode = GetCurrentThreadNode(threadId);
                var stackTrace = new StackTrace();
                var currentMethodNode = GetCurrentMethodNode(stackTrace);
                Stack<TracerMethodNode> currentStackForMethodNodes;
                if (currentThreadNode != null)
                {
                    currentStackForMethodNodes = stacksForThreads[currentThreadNode];
                }
                else
                {
                    currentStackForMethodNodes = new Stack<TracerMethodNode>();
                    currentThreadNode = new ThreadNode
                    {
                        ThreadId = threadId,
                        ThreadName = Thread.CurrentThread.Name,
                        Root = new List<IMethodNode>()

                    };
                    stacksForThreads.Add(currentThreadNode, currentStackForMethodNodes);
                }
                if (currentStackForMethodNodes.Count == 0)
                {
                    currentThreadNode.Root.Add(currentMethodNode);
                }
                else
                {
                    currentStackForMethodNodes.Peek().ChildNodes.Add(currentMethodNode);
                }
                currentStackForMethodNodes.Push(currentMethodNode);
                watch.Start();
            }
        }

        public void StopTrace()
        {
            lock (stopLock)
            {
                watch.Stop();
                var threadId = Thread.CurrentThread.ManagedThreadId;
                var currentThreadNode = GetCurrentThreadNode(threadId);
                if (currentThreadNode == null) throw new NullReferenceException("Stop method is rised before start");
                var currentStack = stacksForThreads[currentThreadNode];
                if (currentStack.Peek() != null)
                {
                    var currentMethod = currentStack.Pop();
                    var time = watch.ElapsedMilliseconds - currentMethod.GetStartExecutionTime();
                    currentMethod.ExecutionTime = TimeSpan.FromMilliseconds(time);
                    currentThreadNode.OverallTime = TimeSpan.FromMilliseconds(currentThreadNode.OverallTime.Milliseconds + time);
                }
                watch.Start();
            }
        }

        private TracerMethodNode GetCurrentMethodNode(StackTrace stackTrace)
        {
            var frame = stackTrace.GetFrame(1);
            var methodBase = frame.GetMethod();
            return new TracerMethodNode(watch.ElapsedMilliseconds)
            {
                MethodName = methodBase.Name,
                ClassName = methodBase.DeclaringType?.Name,
                ParametrCounts = methodBase.GetParameters().Length,
                ChildNodes = new List<IMethodNode>(),
            };
        }

        private ThreadNode GetCurrentThreadNode(int threadId)
        {
            return stacksForThreads.Keys.FirstOrDefault(x => x.ThreadId == threadId);
        }
    }
}

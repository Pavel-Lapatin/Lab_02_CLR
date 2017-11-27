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
        private static Tracer _instance;

        private readonly IDictionary<ThreadNode, Stack<TracerMethodNode>> _stacksForThreads =
            new Dictionary<ThreadNode, Stack<TracerMethodNode>>();

        private static readonly Stopwatch Watch = new Stopwatch();
        private readonly object _startLock = new object();
        private readonly object _stopLock = new object();
        private static readonly object CtortLock = new object();

        public static Tracer Instance
        {
            get
            {
                lock (CtortLock)
                {

                    if (_instance == null)
                    {
                        _instance = new Tracer();
                    }
                }
                return _instance;
            }
        }

        public ITraceResult GetTraceResult()
        {
            var results = new TraceResult();
            foreach (var thread in _stacksForThreads.Keys)
            {
                results.Root.Add(thread);
            }
            return results;
        }

        public void StartTrace()
        {
            lock (_startLock)
            {
                Watch.Stop();
                var threadId = Thread.CurrentThread.ManagedThreadId;
                var currentThreadNode = GetCurrentThreadNode(threadId);
                var stackTrace = new StackTrace();
                var currentMethodNode = GetCurrentMethodNode(stackTrace);
                Stack<TracerMethodNode> currentStackForMethodNodes;
                if (currentThreadNode != null)
                {
                    currentStackForMethodNodes = _stacksForThreads[currentThreadNode];
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
                    _stacksForThreads.Add(currentThreadNode, currentStackForMethodNodes);
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
                Watch.Start();
            }
        }

        public void StopTrace()
        {
            lock (_stopLock)
            {
                Watch.Stop();
                var threadId = Thread.CurrentThread.ManagedThreadId;
                var currentThreadNode = GetCurrentThreadNode(threadId);
                if (currentThreadNode == null) throw new NullReferenceException("Stop method is rised before start");
                var currentStack = _stacksForThreads[currentThreadNode];
                if (currentStack.Peek() != null)
                {
                    var currentMethod = currentStack.Pop();
                    var time = Watch.ElapsedTicks - currentMethod.GetStartExecutionTime();
                    currentMethod.ExecutionTime = TimeSpan.FromTicks(time);
                    currentThreadNode.OverallTime = TimeSpan.FromTicks(currentThreadNode.OverallTime.Ticks + time);
                }
                Watch.Start();
            }
        }

        private TracerMethodNode GetCurrentMethodNode(StackTrace stackTrace)
        {
            var frame = stackTrace.GetFrame(1);
            var methodBase = frame.GetMethod();
            return new TracerMethodNode(Watch.ElapsedTicks)
            {
                MethodName = methodBase.Name,
                ClassName = methodBase.DeclaringType?.Name,
                ParametrCounts = methodBase.GetParameters().Length,
                ChildNodes = new List<IMethodNode>(),
            };
        }

        private ThreadNode GetCurrentThreadNode(int threadId)
        {
            return _stacksForThreads.Keys.FirstOrDefault(x => x.ThreadId == threadId);
        }
    }
}

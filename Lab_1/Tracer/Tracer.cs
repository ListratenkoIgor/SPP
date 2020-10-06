using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Listsoft
{
    namespace Lab_Tracer
    {
        public class SomethingFellOutException : Exception
        {
            public SomethingFellOutException()
            {
            }

            public SomethingFellOutException(string message)
                : base(message)
            {
            }

            public SomethingFellOutException(string message, Exception inner)
                : base(message, inner)
            {
            }
        }
        class TracedItem
        {
            public string Name { get; set; }
            public Stopwatch stopwatch;
            public MethodInfo methodInfo;
            internal TracedItem()
            {
                stopwatch = new Stopwatch();
            }
        }
        public class Tracer : ITracer
        {
            private StackTrace stackTrace;
            private ConcurrentDictionary<int, Stack<TracedItem>> context =
                new ConcurrentDictionary<int, Stack<TracedItem>>();
            private TraceResult traceResult = new TraceResult();

            public void StartTrace()
            {
                int threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
                traceResult.threads.GetOrAdd(threadId, new ThreadInfo
                {
                    time = 0
                });
                Stack<TracedItem> stack = context.GetOrAdd(threadId, new Stack<TracedItem>());
                TracedItem tracedItem = new TracedItem();
                stackTrace = new StackTrace();
                tracedItem.Name = stackTrace.GetFrame(1).GetMethod().Name;
                MethodInfo preparedItem = new MethodInfo()
                {
                    name = tracedItem.Name,
                    className = stackTrace.GetFrame(1).GetMethod().DeclaringType.Name
                };
                tracedItem.methodInfo = preparedItem;
                if (stack.Count != 0)
                {
                    MethodInfo father = stack.Peek().methodInfo;
                    father.methods.Add(preparedItem);
                }
                else
                {
                    ThreadInfo threadInfo;
                    if (traceResult.threads.TryGetValue(threadId, out threadInfo))
                    {
                        threadInfo.methods.Add(preparedItem);
                    }
                    else
                    {
                        throw new SomethingFellOutException("Поток отвалился или не найден");
                    }
                }
                stack.Push(tracedItem);
                tracedItem.stopwatch.Restart();
            }
            public void StopTrace()
            {
                int threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
                Stack<TracedItem> stack;
                context.TryGetValue(threadId, out stack);
                TracedItem tracedItem = stack.Pop();
                tracedItem.stopwatch.Stop();
                TimeSpan timeSpan = tracedItem.stopwatch.Elapsed;
                tracedItem.methodInfo.time = timeSpan.Milliseconds;
                ThreadInfo threadInfo;
                if (traceResult.threads.TryGetValue(threadId, out threadInfo))
                {
                    threadInfo.time = 0;
                    foreach (MethodInfo method in threadInfo.methods)
                    {
                        threadInfo.time += method.time;
                    }
                }
                else
                {
                    throw new SomethingFellOutException(String.Format("Стек потока {0} отвалился или не найден", threadId));
                }
                Console.WriteLine("Elapsed time of method {0}.{1}: {2}ms in thread {3}", tracedItem.methodInfo.className, tracedItem.Name, String.Format("{0:00}", timeSpan.Milliseconds), threadId);
            }
            public TraceResult GetTraceResult()
            {
                return traceResult;
            }
        }
    }
}
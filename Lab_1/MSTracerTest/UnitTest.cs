using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Collections.Generic;

namespace Listsoft
{
    namespace Lab_Tracer
    {
        namespace MSTracerTest
        {
            [TestClass]
            public class TracerTest
            {
                private ITracer tracer = new Tracer();
                private const int SLEEP_TIME = 30;
                private const int THREADS_COUNT = 3;

                private void SingleMethod()
                {
                    tracer.StartTrace();
                    Thread.Sleep(SLEEP_TIME);
                    tracer.StopTrace();
                }

                private void MethodWithInnerMethod()
                {
                    tracer.StartTrace();
                    Thread.Sleep(SLEEP_TIME);
                    SingleMethod();
                    tracer.StopTrace();
                }

                [TestMethod]
                public void TestSingleMethod()
                {
                    SingleMethod();
                    TraceResult traceResult = tracer.GetTraceResult();
                    int threadId = Thread.CurrentThread.ManagedThreadId;
                    ThreadInfo threadInfo;
                    traceResult.threads.TryGetValue(threadId, out threadInfo);
                    long countedTime = threadInfo.methods[0].time;
                }

                [TestMethod]
                public void TestMethodWithInnerMethod()
                {
                    MethodWithInnerMethod();
                    TraceResult traceResult = tracer.GetTraceResult();
                    int threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
                    ThreadInfo threadInfo;
                    traceResult.threads.TryGetValue(threadId, out threadInfo);
                    long countedTime = threadInfo.methods[0].time;
                }

                [TestMethod]
                public void TestSingleMethodInMultiThreads()
                {
                    var threads = new List<Thread>();
                    double expectedTotalElapsedTime = 0;
                    for (int i = 0; i < THREADS_COUNT; i++)
                    {
                        var newThread = new Thread(SingleMethod);
                        threads.Add(newThread);
                        expectedTotalElapsedTime += SLEEP_TIME;
                    }
                    foreach (var thread in threads)
                    {
                        thread.Start();
                    }
                    foreach (var thread in threads)
                    {
                        thread.Join();
                    }
                    double actualTotalElapsedTime = 0;
                    foreach (var threadItem in tracer.GetTraceResult().threads)
                    {                       
                        long countedTime = threadItem.Value.time;
                        actualTotalElapsedTime += countedTime;
                    }

                }
            }
        }
    }
}
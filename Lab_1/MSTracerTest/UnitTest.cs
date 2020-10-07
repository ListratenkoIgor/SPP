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
                    Assert.AreEqual(nameof(SingleMethod), threadInfo.methods[0].name);
                    Assert.AreEqual(nameof(TracerTest), threadInfo.methods[0].className);
                    Assert.AreEqual(0, threadInfo.methods[0].methods.Count);
                    Assert.IsTrue(countedTime >= SLEEP_TIME);
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
                    Assert.AreEqual(1, threadInfo.methods[0].methods.Count);
                    Assert.AreEqual(0, threadInfo.methods[0].methods[0].methods.Count);
                    Assert.AreEqual(nameof(MethodWithInnerMethod), threadInfo.methods[0].name);
                    Assert.AreEqual(nameof(SingleMethod), threadInfo.methods[0].methods[0].name);
                }

                [TestMethod]
                public void TestSingleMethodInMultiThreads()
                {
                    var threads = new List<Thread>();
                    long expectedTotalElapsedTime = THREADS_COUNT * SLEEP_TIME;
                    for (int i = 0; i < THREADS_COUNT; i++)
                    {
                        var newThread = new Thread(SingleMethod);
                        threads.Add(newThread);
                    }
                    foreach (var thread in threads)
                    {
                        thread.Start();
                    }
                    foreach (var thread in threads)
                    {
                        thread.Join();
                    }
                    long actualTotalElapsedTime = 0;
                    foreach (var threadItem in tracer.GetTraceResult().threads)
                    {                       
                        actualTotalElapsedTime += threadItem.Value.time; 
                    }
                    Assert.AreEqual(THREADS_COUNT, tracer.GetTraceResult().threads.Count);
                    Assert.IsTrue(actualTotalElapsedTime >= expectedTotalElapsedTime);
                }
            }
        }
    }
}
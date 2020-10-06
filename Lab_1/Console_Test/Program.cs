using System.Threading;
using System;
using Listsoft.Lab_Tracer.Writer;
using Listsoft.Lab_Tracer.Serilization;

namespace Listsoft
{
    namespace Lab_Tracer
    {
        namespace ConsoleTest
        {
            class FirstClass
            {
                private ITracer tracer;

                internal FirstClass(ITracer tracer)
                {
                    this.tracer = tracer;
                }
                public void Test()
                {
                    tracer.StartTrace();
                    for (int i = 0; i < 50000000; i++)
                    {
                        int b = i + 2;
                    }
                    new SecondClass(tracer).Inner();
                    tracer.StopTrace();
                }
                public void Single()
                {
                    tracer.StartTrace();
                    for (int i = 0; i < 100000000; i++) { }
                    tracer.StopTrace();
                }
            }

            public class SecondClass
            {
                private ITracer tracer;
                public SecondClass(ITracer tracer)
                {
                    this.tracer = tracer;
                }

                public void Inner()
                {
                    tracer.StartTrace();
                    for (int i = 0; i < 1000000; i++)
                    {
                        int a = i + 1;
                    }
                    tracer.StopTrace();
                }
                public void Requrse(int i)
                {
                    tracer.StartTrace();
                    if (i > 0)
                    {
                        Requrse(i - 1);
                    }
                    Thread.Sleep(30);
                    tracer.StopTrace();
                    if (i == 0)
                    {

                    }
                }
            }
            class Program
            {
                static void ThreadExample(ITracer tracer)
                {
                    new FirstClass(tracer).Test();
                    new SecondClass(tracer).Requrse(7);
                }
                static void Main(string[] args)
                {
                    ITracer tracer = new Tracer();
                    WriterBridge writerBridge = new WriterBridge();
                    new FirstClass(tracer).Test();
                    new FirstClass(tracer).Single();
                    Thread thread1 = new Thread(() => new FirstClass(tracer).Single());
                    Thread thread2 = new Thread(() => ThreadExample(tracer));
                    thread2.Start();
                    thread1.Start();
                    new FirstClass(tracer).Single();
                    thread2.Join();
                    thread1.Join();
                    TraceResult traceResult = tracer.GetTraceResult();
                    Console.WriteLine();
                    ThreadInfo ti;
                    ti = traceResult.threads[Thread.CurrentThread.ManagedThreadId];
                    Console.WriteLine("Main Thread {0} Elapsed time:{1}ms", Thread.CurrentThread.ManagedThreadId, ti.time);
                    ti = traceResult.threads[thread1.ManagedThreadId];
                    Console.WriteLine("Thread {0} Elapsed time:{1}ms", thread1.ManagedThreadId, ti.time);
                    ti = traceResult.threads[thread2.ManagedThreadId];
                    Console.WriteLine("Thread {0} Elapsed time:{1}ms", thread2.ManagedThreadId, ti.time, ti.time);
                    Console.WriteLine();
                    writerBridge.SetIntarface(new ConsoleWriter());
                    writerBridge.Write(new JsonSerializer(), traceResult);
                    writerBridge.Write(new XmlSerializer(), traceResult);
                    writerBridge.SetIntarface(new FileWriter());
                    writerBridge.Write(new JsonSerializer(),traceResult);
                    writerBridge.Write(new XmlSerializer(), traceResult);
                    Console.ReadLine();
                }
            }
        }
    }
}
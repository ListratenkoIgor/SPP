using System.IO;
using Listsoft.Lab_Tracer.Serilization;

namespace Listsoft
{
    namespace Lab_Tracer
    {
        namespace Writer
        {
            public class FileWriter : IWriter
            {
                public void Write(ISerializer serializer, TraceResult traceResult)
                {
                    using (StreamWriter sw = new StreamWriter("traceResult" + serializer.GetExtension()))
                    {
                        sw.WriteLine(serializer.Serialize(traceResult));
                    }
                }
            }
        }
    }
}
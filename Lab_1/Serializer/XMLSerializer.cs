using System;
using System.Collections.Generic;
using System.IO;

namespace Listsoft
{
    namespace Lab_Tracer
    {
        namespace Serilization
        {
            public class XmlSerializer : ISerializer
            {
                public string Serialize(TraceResult traceResult)
                {
                    List<ThreadItem> items = new List<ThreadItem>();
                    foreach (var thread in traceResult.threads)
                    {
                        items.Add(new ThreadItem()
                        {
                            id = thread.Key.ToString(),
                            methods = thread.Value.methods,
                            time = thread.Value.time.ToString()
                        }); ;
                    }

                    System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(items.GetType());

                    using (StringWriter textWriter = new StringWriter())
                    {
                        xmlSerializer.Serialize(textWriter, items);
                        return textWriter.ToString();
                    }
                }
                public string GetExtension() {
                    return ".xml";
                } 
            }
        }
    }
}

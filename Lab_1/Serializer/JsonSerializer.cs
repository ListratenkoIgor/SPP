using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;


namespace Listsoft
{
    namespace Lab_Tracer
    {
        namespace Serilization
        {
            public class JsonSerializer : ISerializer
            {
                public string Serialize(TraceResult traceResult)
                {
                    return JsonConvert.SerializeObject(traceResult, Formatting.Indented);
                }
                public string GetExtension()
                {
                    return ".json";
                }
            }
        }
    }
}

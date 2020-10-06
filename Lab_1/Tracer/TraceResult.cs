using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Listsoft
{
    namespace Lab_Tracer
    {
        public class MethodInfo
        {
            public string name;
            public string className;
            public long time;
            public List<MethodInfo> methods = new List<MethodInfo>();
        }
        public class ThreadInfo
        {
            public long time;
            public List<MethodInfo> methods = new List<MethodInfo>();
        }
        public class TraceResult
        {
            public ConcurrentDictionary<int, ThreadInfo> threads =
                new ConcurrentDictionary<int, ThreadInfo>();
        }
        public class ThreadItem
        {
            public string id;
            public string time;
            public List<MethodInfo> methods = new List<MethodInfo>();
        }
    }
}
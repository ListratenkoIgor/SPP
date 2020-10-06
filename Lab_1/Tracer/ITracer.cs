namespace Listsoft
{
    namespace Lab_Tracer
    {
        public interface ITracer
        {
            void StartTrace();
            void StopTrace();
            TraceResult GetTraceResult();
        }

    }
}
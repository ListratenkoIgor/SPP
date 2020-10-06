using Listsoft.Lab_Tracer.Serilization;

namespace Listsoft
{
    namespace Lab_Tracer
    {
        namespace Writer
        {
            public interface IWriter
            {
                void Write(ISerializer serializer, TraceResult traceResult);
            }
            public class WriterBridge
            {
                private IWriter _currentInterface = null;
                public WriterBridge()
                {                    
                }
                public WriterBridge(IWriter writer) {
                    _currentInterface = writer;
                }
                public void SetIntarface(IWriter writer) {
                    _currentInterface = writer;
                }
                public void Write(ISerializer serializer, TraceResult traceResult) {
                    _currentInterface.Write(serializer, traceResult);
                }
            }
        }
    }
}
namespace DependencyInjectionContainerTester
{
    public class Controller : IController
    {
        private readonly IService _service;

        public Controller(IService service)
        {
            _service = service;
        }
        
        public string Dispatch()
        {
            return $"Controller: {_service.Service()}";
        }
    }
}
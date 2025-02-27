using Framework.Mvcs.Model;
using Framework.Mvcs.Service;
using Framework.Mvcs.View;

//Keep As:RMC.Mini.Controller
namespace Framework.Mvcs.Controller
{
    /// <summary>
    /// Optional. Useful when you want to use 'no concern' here,
    /// but your setup requires one.
    /// </summary>
    public class DummyController : BaseController<DummyModel, DummyView, DummyService>
    {
        public DummyController(DummyModel model, DummyView view, DummyService service) 
            : base(model, view, service)
        {
        }
    }
}
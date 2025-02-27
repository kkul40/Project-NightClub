using Framework.Mvcs.Controller;
using Framework.Mvcs.Model;
using Framework.Mvcs.Service;
using Framework.Mvcs.View;

//Keep As:RMC.Mini
namespace Framework.Structures.Standard
{
    /// <summary>
    /// Enforces API for types which Initialize.
    /// </summary>
    public interface IInitializableWithMini
    {
        //  Properties  ------------------------------------
        public bool IsInitialized { get; }
        public IMiniMvcs<Context.Context,
            IModel,
            IView,
            IController,
            IService> MiniMvcs { get; }

        //  General Methods  ------------------------------
        public void Initialize(IMiniMvcs<Context.Context,
            IModel,
            IView,
            IController,
            IService> miniMvcs);
        
        void RequireIsInitialized();
    }
}
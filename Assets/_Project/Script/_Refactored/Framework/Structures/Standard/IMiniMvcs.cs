using Framework.Context;
using Framework.Locators;
using Framework.Mvcs.Controller;
using Framework.Mvcs.Model;
using Framework.Mvcs.Service;
using Framework.Mvcs.View;
using Framework.Structures.Simple;

//Keep As:RMC.Mini
namespace Framework.Structures.Standard
{
    /// <summary>
    /// Enforces API for types which Initialize.
    /// </summary>
    public interface IMiniMvcs 
        <
        TContext, 
        TModel, 
        TView, 
        TController, 
        TService> 
    
        : ISimpleMiniMvcs
    
        where TContext : IContext 
        where TModel : class, IModel
        where TView : class, IView 
        where TController : class, IController 
        where TService : class, IService
    {
        TContext Context { get; }

        /// <summary>
        /// The ModelLocator is the only ModelLocator that already exists in the
        /// Context. So we fetch it from there.
        /// </summary>
        Locator<TModel> ModelLocator { get; }
        Locator<TView> ViewLocator { get; }
        Locator<TController> ControllerLocator { get; }
        Locator<TService> ServiceLocator { get; }
    }
        //  Properties ------------------------------------
    

        //  Methods ---------------------------------------
    }

using System;
using Framework.Locators;
using Framework.Mvcs.Controller.Commands;
using Framework.Mvcs.Model;

//Keep As:RMC.Mini
namespace Framework.Context
{
    /// <summary>
    /// See <see cref="Context"/>
    /// </summary>
    public interface IContext : IDisposable
    {
        Locator<IModel> ModelLocator { get; }
        ICommandManager CommandManager { get; }
    }
}
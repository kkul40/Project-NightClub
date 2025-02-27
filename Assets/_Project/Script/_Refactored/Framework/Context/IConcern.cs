

//Keep As:RMC.Mini

using System;
using Framework.Structures.Simple;

namespace Framework.Context
{
    /// <summary>
    /// Enforces API for types which are one
    /// area of coding concern within the
    /// <see cref="ISimpleMiniMvcs"/> architectural framework.
    /// </summary>
    public interface IConcern : IInitializableWithContext, IDisposable
    {
        //  Properties ------------------------------------
    

        //  Methods ---------------------------------------
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

//Keep As:RMC.Mini
namespace Framework.Locators
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// Locator base class providing utility methods for type hierarchy and disposal.
    /// </summary>
    public class Locator : IDisposable
    {
        //  Events ----------------------------------------

        //  Properties ------------------------------------

        //  Fields ----------------------------------------

        //  Initialization  -------------------------------

        //  Unity Methods   -------------------------------

        //  Other Methods ---------------------------------

        /// <summary>
        /// Gets the most specific type of a given type.
        /// </summary>
        public static Type GetLowestType(Type type)
        {
            return GetTypeHierarchy(type).LastOrDefault();
        }

        /// <summary>
        /// Gets the type hierarchy of a given type.
        /// </summary>
        private static Type[] GetTypeHierarchy(Type type)
        {
            var hierarchy = new List<Type>();

            while (type != null)
            {
                hierarchy.Insert(0, type);  // Insert at the beginning to maintain order
                type = type.BaseType;
            }

            return hierarchy.ToArray();
        }

        /// <summary>
        /// Disposes of the locator.
        /// </summary>
        public virtual void Dispose()
        {
            throw new NotImplementedException();
        }

        //  Event Handlers --------------------------------
    }
}
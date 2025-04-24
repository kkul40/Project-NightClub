using System;
using Framework.Context;
using UI.GamePages;
using UnityEngine;

namespace Framework.Mvcs.View
{
    public enum PageType
    {
        FullPage,
        MiniPage,
        PopUp,
    }

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Model stores runtime data 
    /// </summary>
    public abstract class BaseView : MonoBehaviour, IView
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }
        public abstract PageType PageType { get; protected set; }

        private bool isToggled;
        public bool IsToggled
        {
            get
            {
                return isToggled;
            }
        }

        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private IContext _context;

        //  Initialization  -------------------------------
        public virtual void Initialize(IContext context)
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                _context = context;
            }
        }
        
        public void RequireIsInitialized()
        {
            if (!_isInitialized)
            {
                throw new Exception("MustBeInitialized");
            }
        }

        public virtual void EventEnable()
        {
            // Subscribe to Events.
        }

        public virtual void EventDisable()
        {
            // Unsubcribe to Events.
        }

        
        //  Dispose Methods --------------------------------
        public virtual void Dispose()
        {
            // Optional: Handle any cleanup here...
        }
        
        //  Methods ---------------------------------------
        public virtual void ToggleView(bool toggle)
        {
            this.gameObject.SetActive(toggle);
            isToggled = toggle;
        }

        public virtual void ToggleView()
        {
            this.gameObject.SetActive(!gameObject.activeInHierarchy);
            isToggled = gameObject.activeInHierarchy;
        }

        //  Event Handlers --------------------------------
    }
}
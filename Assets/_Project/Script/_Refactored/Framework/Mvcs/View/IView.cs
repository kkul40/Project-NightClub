using Framework.Context;

namespace Framework.Mvcs.View
{
    /// <summary>
    /// The View handles user interface and user input
    /// </summary>
    public interface IView : IConcern
    {
        //  Properties ------------------------------------

        //  Methods ---------------------------------------
        public void ToggleView(bool toggle);
        public void ToggleView();
    }
}
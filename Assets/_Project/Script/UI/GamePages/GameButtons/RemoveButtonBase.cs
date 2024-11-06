using Disco_Building;

namespace UI.GamePages.GameButtons
{
    public class RemoveButtonBase : UIButtonBase
    {
        public override void OnClick()
        {
            BuildingManager.Instance.StartRemoving();
        }
    }
}
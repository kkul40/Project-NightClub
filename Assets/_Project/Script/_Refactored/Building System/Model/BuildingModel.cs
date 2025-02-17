using System.Collections.Generic;
using Data;
using Disco_ScriptableObject;
using RMC.Mini;
using RMC.Mini.Model;

public class BuildingModel : BaseModel
{
    public List<StoreItemSO> StoreItems;
    public override void Initialize(IContext context)
    {
        if (!IsInitialized)
        {
            StoreItems = new List<StoreItemSO>();
            foreach (var item in DiscoData.Instance.AllInGameItems)
            {
                StoreItems.Add(item.Value);
            }
        }
    }
}
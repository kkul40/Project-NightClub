namespace BuildingSystemFolder
{
    // public class FloorMaterialPlacer : IBuilder
    // {
    //     private FloorMaterialSo _floorMaterialSo;
    //     private IMaterial _tempMaterialObject;
    //     private IMaterial _lastMaterialObject;
    //     
    //     private Material[] savedMaterials;
    //     
    //     public void Setup(PlacableItemSo placableItemSo)
    //     {
    //         if (placableItemSo is FloorMaterialSo floorMaterialSo)
    //         {
    //             _floorMaterialSo = floorMaterialSo;
    //             BuildingManager.Instance.GetTileIndicator.SetTileIndicator(PlacingType.Place);
    //         }
    //     }
    //
    //     public void BuildUpdate()
    //     {
    //         BuildingManager.Instance.GetMouseCellPosition();
    //
    //         var hitTransform = InputSystem.Instance.GetLastHitTransform();
    //
    //         if (hitTransform.TryGetComponent(out FloorTile floorTile) && hitTransform.TryGetComponent(out IMaterial material))
    //         {
    //             _tempMaterialObject = material;
    //         }
    //         else
    //         {
    //             _tempMaterialObject = null;
    //         }
    //         
    //         if (_tempMaterialObject != _lastMaterialObject) // Optimization
    //         {
    //             // Reset Previous Material
    //             if (_lastMaterialObject != null)
    //             {
    //                 _lastMaterialObject.ResetMaterial(savedMaterials);
    //                 _lastMaterialObject = null;
    //             }
    //         
    //             // Preview Selected Material On Object
    //             if (_tempMaterialObject != null)
    //             {
    //                 _lastMaterialObject = _tempMaterialObject;
    //                 savedMaterials = _tempMaterialObject.GetCurrentMaterial();
    //                 _tempMaterialObject.ChangeMaterial(_floorMaterialSo.Material);
    //             }
    //         }
    //         
    //         if (InputSystem.Instance.LeftHoldClickOnWorld)
    //         {
    //             if (_tempMaterialObject != null)
    //             {
    //                 _lastMaterialObject = null;
    //             }
    //         }
    //
    //         if (InputSystem.Instance.Esc)
    //         {
    //             Exit();
    //         }
    //     }
    //
    //     public void Exit()
    //     {
    //         if (_lastMaterialObject != null)
    //         {
    //             _lastMaterialObject.ResetMaterial(savedMaterials);
    //             _lastMaterialObject = null;
    //         }
    //         
    //         BuildingManager.Instance.StopBuilding();
    //     }
    // }
}
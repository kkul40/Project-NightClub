using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Script.NewSystem
{
    public class WallPaperPlacer : MonoBehaviour, IBuild
    {
        private WallPaperSo tempWallPaperSo;
        private Wall tempWall;
        private Wall lastWall;

        private Material[] savedMaterials;
        
        public void Setup<T>(T itemSo) where T : ItemSo
        {
            if (itemSo is WallPaperSo wallPaperSo)
            {
                tempWallPaperSo = wallPaperSo;
            }
        }

        public void BuildUpdate()
        {
            BuildingSystem.Instance.GetMouseCellPosition();
            var mousePos = InputSystem.Instance.GetMouseMapPosition();
            
            tempWall = GetClosestWall(mousePos);

            if (tempWall != lastWall) // Optimization
            {
                if (lastWall != null)
                {
                    lastWall.ResetWallPaper(savedMaterials);
                    lastWall = null;
                }
            
                if (tempWall != null)
                {
                    lastWall = tempWall;
                    savedMaterials = tempWall.GetCurrentMaterial();
                    tempWall.ChangeWallpaper(tempWallPaperSo.Material);
                }
            }
            
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (tempWall != null)
                {
                    tempWall.ChangeWallpaper(tempWallPaperSo.Material);
                    lastWall = null;
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Exit();
            }
        }

        public void Exit()
        {
            if (lastWall != null)
            {
                lastWall.ChangeWallpaper(savedMaterials[1]);
                lastWall = null;
            }
            
            BuildingSystem.Instance.ResetPlacerAndRemover();
        }
        
        private Wall GetClosestWall(Vector3 cellPos)
        {
            float lastDis = 9999;
            Wall closestWall = null;
            foreach(var wall in GameData.Instance.GetWallMapPosList())
            {
                var dis = Vector3.Distance(cellPos, wall.transform.position);
                if (dis < lastDis)
                {
                    closestWall = wall;
                    lastDis = dis;
                }
            }

            return closestWall;
        }
    }
}
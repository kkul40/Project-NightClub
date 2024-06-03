using UnityEngine;

namespace BuildingSystem.Builders
{
    public class RotationMethodHandlerAuto : IRotationMethod
    {
        public void OnStart(BuildingNeedsData buildingNeedsData)
        {
        }

        public void OnRotate(BuildingNeedsData buildingNeedsData)
        {
            Quaternion quaternion = GetClosestWallRotation(buildingNeedsData);
            Direction direction = DirectionHelper.GetDirectionFromQuaternion(quaternion);

            buildingNeedsData.RotationData = new RotationData(quaternion, direction);
        }

        private Quaternion GetClosestWallRotation(BuildingNeedsData buildingNeedsData)
        {
            float lastDis = 9999;
            Quaternion closestChangableMaterial = Quaternion.identity;
            foreach(var wall in buildingNeedsData.GameData.GetWallMapPosList())
            {
                var dis = Vector3.Distance(buildingNeedsData.InputSystem.GetMouseMapPosition(), wall.transform.position);
                if (dis < lastDis)
                {
                    closestChangableMaterial = wall.transform.rotation;
                    lastDis = dis;
                }
            }
            return closestChangableMaterial;
        }
    }
}
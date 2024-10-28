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
            var quaternion = GetClosestWallRotation(buildingNeedsData);
            var direction = RotationDataExtensionMethod.GetDirectionFromQuaternion(quaternion);

            buildingNeedsData.RotationData = new RotationData(quaternion, direction);
        }

        private Quaternion GetClosestWallRotation(BuildingNeedsData buildingNeedsData)
        {
            float lastDis = 9999;
            var closestWallRotation = Quaternion.identity;
            foreach (var wall in buildingNeedsData.DiscoData.MapData.WallDatas)
            {
                if (wall.assignedWall == null) continue;

                var dis = Vector3.Distance(buildingNeedsData.InputSystem.MousePosition,
                    wall.assignedWall.transform.position);
                if (dis < lastDis)
                {
                    closestWallRotation = wall.assignedWall.transform.rotation;
                    lastDis = dis;
                }
            }

            return closestWallRotation;
        }
    }
}
using System;
using Data;
using Disco_ScriptableObject;
using ExtensionMethods;
using PropBehaviours;
using Unity.Mathematics;
using UnityEngine;

namespace Disco_Building.Builders
{
    public class WallDoorBuilderMethod : IBuildingMethod
    {
        public bool PressAndHold { get; } = false;
        public bool isFinished { get; private set; }

        private GameObject _tempObject;
        private quaternion _wallRotation;
        private bool isWallOnX;

        private WallAssignmentData _closestAssignmentData;

        public void OnStart(BuildingNeedsData BD)
        {
            Debug.Log("Door Builder Started");

            var door = BD.DiscoData.MapData.WallDatas.Find(x => x.assignedWall is WallDoor);
            
            // Add Missing Wall
            var newWall = MapGeneratorSystem.Instance.CreateObject(MapGeneratorSystem.Instance.GetWallPrefab, door.assignedWall.transform.position, door.assignedWall.transform.rotation);
            newWall.transform.SetParent(SceneGameObjectHandler.Instance.GetWallHolder);
            BD.DiscoData.MapData.WallDatas.Add(new WallAssignmentData(door.CellPosition));
            BD.DiscoData.MapData.WallDatas[^1].AssignReferance(newWall.GetComponent<Wall>());
            
            // Destory Door
            MonoBehaviour.Destroy(door.assignedWall.gameObject);
            BD.DiscoData.MapData.WallDatas.Remove(door);
            
            
            // Create Temp Door
            _tempObject = MonoBehaviour.Instantiate(MapGeneratorSystem.Instance.GetWallDoorPrefab, BD.CellPosition, Quaternion.identity);
        }

        public bool OnValidate(BuildingNeedsData BD)
        {
            return true;
        }

        public void OnUpdate(BuildingNeedsData BD)
        {
            _tempObject.transform.position = Vector3.Lerp(_tempObject.transform.position,
                GetClosestWall(BD) + new Vector3(0.02f, 0.02f, 0.02f),
                Time.deltaTime * BD.MoveSpeed);
            
            _tempObject.transform.rotation = _wallRotation;
        }

        public void OnPlace(BuildingNeedsData BD)
        {
            Vector3 position = isWallOnX ? new Vector3(BD.CellPosition.x + 0.5f, 0, 0) : new Vector3(0, 0, BD.CellPosition.z + 0.5f);
            
            // Spawn Door
            var newWallDoorObject = MapGeneratorSystem.Instance.CreateObject(MapGeneratorSystem.Instance.GetWallDoorPrefab, position, _wallRotation);
            newWallDoorObject.transform.SetParent(SceneGameObjectHandler.Instance.GetWallHolder);
            BD.DiscoData.MapData.ChangeDoorPosition((int)Mathf.Max(position.x, position.z) + 1, isWallOnX);
            
            //Remove Wall From The Position
            MonoBehaviour.Destroy(_closestAssignmentData.assignedWall.gameObject);

            _closestAssignmentData.AssignReferance(newWallDoorObject.GetComponent<Wall>());
            var found = DiscoData.Instance.FindAItemByID(_closestAssignmentData.assignedMaterialID) as MaterialItemSo;
            if (found == null)
                _closestAssignmentData.AssignNewID(InitConfig.Instance.GetDefaultWallMaterial);
            else
                _closestAssignmentData.AssignNewID(found);

            
            isFinished = true;
        }

        public void OnStop(BuildingNeedsData BD)
        {
            if(_tempObject != null) 
                MonoBehaviour.Destroy(_tempObject);

        }

        public Vector3 GetClosestWall(BuildingNeedsData BD)
        {
            float lastDis = 9999;
            Vector3 closestWallPos = Vector3.zero;
            foreach (var wall in BD.DiscoData.MapData.WallDatas)
            {
                var dis = Vector3.Distance(BD.InputSystem.MousePosition,
                    wall.assignedWall.transform.position);
                
                if (dis < lastDis)
                {
                    closestWallPos = wall.assignedWall.transform.position;
                    _closestAssignmentData = wall;
                    _wallRotation = wall.assignedWall.transform.rotation;
                    isWallOnX = _wallRotation != RotationData.Left.rotation;
                    lastDis = dis;
                }
            }

            return closestWallPos;
        }
    }
}
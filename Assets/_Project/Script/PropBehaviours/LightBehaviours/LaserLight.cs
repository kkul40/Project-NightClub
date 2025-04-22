using System;
using System.Collections;
using Data;
using Disco_Building;
using DiscoSystem;
using ExtensionMethods;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PropBehaviours.LightBehaviours
{
    public class LaserLight : IPropUnit
    {
        [SerializeField] private LineRenderer _lineRenderer;
        
        private float delay = 10;
        private float timer = 0;

        public float laserSpeed = 10;

        private bool isMovingForward = true;

        private Coroutine _routine;

        private Direction direction;

        public override void Initialize(int ID, Vector3Int cellPosition, ePlacementLayer placementLayer)
        {
            base.Initialize(ID, cellPosition, placementLayer);
            direction = transform.rotation.GetDirectionFromQuaternion();
        }

        private void Awake()
        {
            _lineRenderer = GetComponentInChildren<LineRenderer>();
            _lineRenderer.positionCount = 0;

            delay = Random.Range(5, 20);
        }
        

        private void Update()
        {
            if (_routine != null) return;
            
            timer += Time.deltaTime;

            if (timer > delay)
            {
                _routine = StartCoroutine(ShootLaserCo());
            }
        }

        public override void OnRelocated()
        {
            if (_routine != null)
            {
                StopCoroutine(_routine);
                _lineRenderer.enabled = false;
            }
        }

        private IEnumerator ShootLaserCo()
        {
            Vector3[] positions = GetPositions();
            _lineRenderer.positionCount = positions.Length;
            _lineRenderer.enabled = true;

            if (direction == Direction.Up || direction == Direction.Right)
            {
                Debug.LogError("There is Rotoation Bugg Here");
                yield break;
            }
            
            while (true)
            {
                for (int i = 0; i < positions.Length; i++)
                {
                    if (i % 2 != 1) continue;
                    
                    if (isMovingForward)
                    {
                        if (MoveForward(positions, i))
                        {
                            isMovingForward = false;
                        }
                    }
                    else
                    {
                        if (MoveBackward(positions, i))
                        {
                            timer = 0;
                            _routine = null;
                            _lineRenderer.enabled = false;
                            isMovingForward = true;
                            delay = Random.Range(5, 20);
                            yield break;
                        }
                    }
                }
                
                _lineRenderer.SetPositions(positions);
                yield return null;
            }
        }

        private bool MoveForward(Vector3[] positions, int i)
        {
            Vector3 pos = Vector3.zero;
            
            bool isReached = false;
            
            switch (direction)
            {
                case Direction.Down:
                    pos = positions[i];
                    pos += Vector3.forward * laserSpeed * Time.deltaTime;
                    positions[i] = pos;
                    
                    if (positions[i].z > DiscoData.Instance.MapData.GetFloorGridData(1, DiscoData.Instance.MapData.CurrentMapSize.y - 1).CellPosition.CellCenterPosition(eGridType.PlacementGrid).z)
                        isReached = true;
                    break;
                case Direction.Left:
                    pos = positions[i];
                    pos += Vector3.right * laserSpeed * Time.deltaTime;
                    positions[i] = pos;
                    
                    if (positions[i].x > DiscoData.Instance.MapData.GetFloorGridData(DiscoData.Instance.MapData.CurrentMapSize.x - 1, 1).CellPosition.CellCenterPosition(eGridType.PlacementGrid).x)
                        isReached = true;
                    break;
            }

            if (isReached) return true;
            return false;
        }

        private bool MoveBackward(Vector3[] positions, int i)
        {
            bool isReached = false;
            switch (direction)
            {
                case Direction.Down:
                    positions[i] -= Vector3.forward * laserSpeed * Time.deltaTime;
                    
                    if (positions[1].z < 0.5f)
                        isReached = true;
                    break;
                case Direction.Left:
                    positions[i] -= Vector3.right * laserSpeed * Time.deltaTime;
                    
                    if (positions[1].x < 0.5f)
                        isReached = true;
                    break;
            }

            if (isReached) return true;
            return false;
        }
        
        private Vector3[] GetPositions()
        {
            Vector3[] positions = new Vector3[1];

            Vector2Int mapSize = DiscoData.Instance.MapData.CurrentMapSize;

            switch (direction)
            {
                case Direction.Down:
                    positions = new Vector3[mapSize.x * 2];
                    break;
                case Direction.Left:
                    positions = new Vector3[mapSize.y * 2];
                    break;
                default:
                    Debug.LogError($"Its not supposed to Face This Direction - {direction}");
                    break;
            }

            int index = 0;
            for (int i = 0; i < positions.Length; i++)
            {
                if (i % 2 == 0)
                    positions[i] = transform.GetChild(0).position + transform.forward * 0.4f;
                else
                {
                    Vector3 directPos = Vector3.zero;
                    switch (direction)
                    {
                        case Direction.Down:
                            directPos = DiscoData.Instance.MapData.GetFloorGridData(index++, 1).CellPosition.CellCenterPosition(eGridType.PlacementGrid);
                            break;
                        case Direction.Left:
                            directPos = DiscoData.Instance.MapData.GetFloorGridData(1, index++).CellPosition.CellCenterPosition(eGridType.PlacementGrid);
                            break;
                    }

                    positions[i] = directPos;
                }
            }

            return positions;
        }
    }
}
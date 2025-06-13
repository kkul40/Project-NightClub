using System;
using System.Collections.Generic;
using Data;
using ExtensionMethods;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace DiscoSystem.NewPathFinder
{
    public sealed class PathFindingSystem : Singleton<PathFindingSystem>
    {
        struct PathFindingJobHandle
        {
            public JobHandle handle;
            public NativeList<int3> indexPath;
            public NativeArray<byte> validGrid;
            public Action<List<Vector3>> onComplete;
        }

        private List<PathFindingJobHandle> jobHandles = new List<PathFindingJobHandle>();
        
        private void Update()
        { 
            for (int i = jobHandles.Count - 1; i >= 0; i--)
            {
                PathFindingJobHandle jobHandle = jobHandles[i];
                if (jobHandle.handle.IsCompleted)
                {
                    jobHandle.handle.Complete();
                    
                    var path = new List<Vector3>();
                    for (int index = 0; index < jobHandle.indexPath.Length; index++)
                    {
                        Vector2Int coordinate = new Vector2Int(jobHandle.indexPath[index].x, jobHandle.indexPath[index].z);
                        path.Add(coordinate.CoordinatesToWorldPosition());
                    }

                    jobHandle.indexPath.Dispose();
                    jobHandle.validGrid.Dispose();
                    jobHandle.onComplete?.Invoke(path);
                    jobHandles.RemoveAt(i);
                }
            }
        }

        public bool RequestPath(Vector3 startPos, Vector3 endPos, Action<List<Vector3>> resultPath)
        {
            if (GetPathData == null) 
                return false;
            
            int gridWidth = GetPathData.PathFinderSize().x;
            int gridHeight = GetPathData.PathFinderSize().y;

            // Bunu Cachele
            NativeArray<byte> validGrid = new NativeArray<byte>(gridWidth * gridHeight, Allocator.TempJob);
            
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    Vector2Int cell = new Vector2Int(x, y);
                    bool isValid = IsValid(cell);
                    int index = x * gridWidth + y;
                    validGrid[index] = (byte)(isValid ? 1 : 0);
                }
            }

            var nativePath = new NativeList<int3>(Allocator.TempJob);

            AStarJob job = new AStarJob
            {
                startCoord = WorldPosToGrid(startPos),
                destCoord = WorldPosToGrid(endPos),
                gridWidth = gridWidth,
                gridHeight = gridHeight,
                validGrid = validGrid,
                indexPath = nativePath
            };

            JobHandle handle = job.Schedule();
            
            jobHandles.Add(new PathFindingJobHandle
            {
                handle = handle,
                indexPath = job.indexPath,
                onComplete = resultPath
            });
            
            return true;
        }
        
        private bool IsValid(Vector2Int index)
        {
            PathFinderNode node = GetNode(index);
            if (node.IsWall) return false;
            
            return node.IsWalkable;
        }
        
        private int2 WorldPosToGrid(Vector3 pos)
        {
            var index = pos.WorldPosToCellPos(eGridType.PathFinderGrid);
            return new int2(index.x, index.z);
        }
        
        private PathFinderNode GetNode(Vector2Int index)
        {
            return GetPathData.GetPath(index.x, index.y);
        }

        private PathData GetPathData => DiscoData.Instance.MapData.Path;

        private void OnDestroy()
        {
            for (int i = jobHandles.Count - 1; i >= 0; i--)
            {
                PathFindingJobHandle jobHandle = jobHandles[i];
                jobHandle.handle.Complete();
                
                jobHandle.indexPath.Dispose();
                jobHandle.validGrid.Dispose();
                jobHandles.RemoveAt(i);
            }
        }
    }
}
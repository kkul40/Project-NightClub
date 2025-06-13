using System;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace DiscoSystem.NewPathFinder
{
    public sealed class PathFindingAgent
    {
        private Queue<Vector3> path;
        private Vector3 nextPosition;
        private Transform agent;
        
        public float speed { get; set; }
        public float rotationSpeed { get; set; }
        public float minimumStoppingDistance { get; set; }
        public bool isStopped { get; set; }

        public PathFindingAgent(Transform agent)
        {
            this.agent = agent;
            speed = 1.5f;
            rotationSpeed = 7.5f;
            minimumStoppingDistance = 0.1f;
            isStopped = true;
            path = new Queue<Vector3>();
            nextPosition = agent.position;
        }
        
        public void Update(float deltaTime)
        {
            if (isStopped) return;
            float distanceToNext = Vector3.Distance(agent.position, nextPosition);

            if (distanceToNext < minimumStoppingDistance)
            {
                if (path.Count == 0)
                {
                    isStopped = true;
                    return;
                }
                nextPosition = path.Dequeue();
            }
            
            // Move Agent
            agent.position = Vector3.MoveTowards(agent.position, nextPosition, deltaTime * speed);

            // Rotate Agent
            Vector3 direction = nextPosition - agent.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            agent.rotation = Quaternion.Slerp(agent.rotation, targetRotation, deltaTime * rotationSpeed);
        }

        public bool SetDestination(Vector3 target)
        {
            return PathFindingSystem.Instance.RequestPath(agent.position, target, OnRequestPathComplete);
        }

        public bool CalculatePath(Vector3 targetPosition, List<Vector3> path)
        {
            return true;
        }

        public bool Warp(Vector3 newPosition)
        {
            return true;
        }

        public void ResetPath()
        {
            path.Clear();
            nextPosition = agent.position;
        }

        private void OnRequestPathComplete(List<Vector3> resultPath)
        {
            path.Clear();
            for (int i = 0; i < resultPath.Count; i++)
                path.Enqueue(resultPath[i]);

            isStopped = false;
            nextPosition = agent.position;
        }
        
    }
}
using System.Collections.Generic;
using DG.Tweening;
using DiscoSystem.Character;
using UnityEngine;

namespace DiscoSystem.NewPathFinder
{
    public sealed class PathFindingAgent
    {
        private Queue<Vector3> path;
        private Vector3 nextPosition;
        private Transform agent;
        private PathUserType userType;
        
        public float speed { get; set; }
        public float rotationSpeed { get; set; }
        public float minimumStoppingDistance { get; set; }
        public bool isStopped { get; set; }

        public Vector3 NextPosition
        {
            get { return nextPosition; }
            set
            {
                nextPosition = value;
                isStopped = false;
            }
        }

        public PathFindingAgent(Transform agent, PathUserType userType)
        {
            this.agent = agent;
            this.userType = userType;
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
            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                agent.rotation = Quaternion.Slerp(agent.rotation, targetRotation, deltaTime * rotationSpeed);
            }
        }

        public bool SetDestination(Vector3 target)
        {
            return PathFindingSystem.Instance.RequestPath(agent.position, target, userType, OnRequestPathComplete);
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
        
        // TODO Bu Fonskiyonla Oyna 0.5f Olayini Sevmedim
        public void SetPositioning(Quaternion? rotation = null, Vector3? position = null, float? duration = null)
        {
            if (rotation != null)
                agent.DORotate(rotation.Value.eulerAngles, duration ?? 0.5f);

            if (position != null)
                agent.DOMove((Vector3)position, duration ?? 0.5f);
        }
        
        public List<Vector3> GetPath()
        {
            List<Vector3> output = new List<Vector3>();
            foreach (var vector in path)
                output.Add(vector);
            return output;
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
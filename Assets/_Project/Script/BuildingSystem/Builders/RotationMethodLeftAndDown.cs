﻿using System;

namespace BuildingSystem.Builders
{
    public class RotationMethodLeftAndDown : IRotationMethod
    {
        public void OnStart(BuildingNeedsData buildingNeedsData)
        {
            var d = buildingNeedsData.RotationData.direction;
            if (d == Direction.Right || d == Direction.Up) buildingNeedsData.RotationData = RotationData.Down;
        }

        public void OnRotate(BuildingNeedsData buildingNeedsData)
        {
            if (InputSystem.Instance.TurnLeft) buildingNeedsData.RotationData = RotationData.Down;

            if (InputSystem.Instance.TurnRight) buildingNeedsData.RotationData = RotationData.Left;
        }
    }
}
using System;
using UnityEngine;

namespace _Project.Script.NewSystem
{
    public class Wall : MonoBehaviour
    {
        private void Awake()
        {
            GameData.Instance.WallMap.Add(this);
        }
    }
}
using System;
using DiscoSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Testing
{
    public class Developer : Singleton<Developer>
    {
        private void Awake()
        {
            // Application.targetFrameRate = 120;
        }
    }
}
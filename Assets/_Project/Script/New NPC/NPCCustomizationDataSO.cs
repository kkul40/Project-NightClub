﻿using System.Collections.Generic;
using UnityEngine;

namespace New_NPC
{
    [CreateAssetMenu(fileName = "NPC", menuName = "NPC/New Customization Data")]
    public class NPCCustomizationDataSO : ScriptableObject
    {
        public List<Mesh> NpcBodyMesh;
        public List<GameObject> NpcHairPrefabs;
        public List<GameObject> NpcBeardPrefabs;
        public List<GameObject> NpcAttachtmentPrefabs;
        public List<GameObject> NpcEaringPrefabs;
    }
}
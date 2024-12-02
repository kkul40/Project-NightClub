using System.Collections.Generic;
using UnityEngine;

namespace NPCBehaviour
{
    [CreateAssetMenu(fileName = "New NPC Customization Data", menuName = "Data/NPC Customization Data")]
    public class NPCCustomizationDataSO : ScriptableObject
    {
        public List<Mesh> NpcBodyMesh;
        public List<GameObject> NpcHairPrefabs;
        public List<GameObject> NpcBeardPrefabs;
        public List<GameObject> NpcAttachtmentPrefabs;
        public List<GameObject> NpcEaringPrefabs;
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace New_NPC
{
    public class NPCRandomizer : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer skinMesh;
        [SerializeField] private Transform Head;

        public void Customize(NPCCustomizationDataSO customizationDataSo, eGenderType genderType)
        {
            Mesh mesh = customizationDataSo.NpcBodyMesh[Random.Range(0, customizationDataSo.NpcBodyMesh.Count)];
            skinMesh.sharedMesh = mesh;

            TryAplly(customizationDataSo.NpcHairPrefabs, Head);
            TryAplly(customizationDataSo.NpcBeardPrefabs, Head);
            TryAplly(customizationDataSo.NpcAttachtmentPrefabs, Head);
            TryAplly(customizationDataSo.NpcEaringPrefabs, Head);
        }

        private void TryAplly(List<GameObject> prefabs, Transform transform)
        {
            if (prefabs.Count <= 0) return;
            var prefab = prefabs[Random.Range(0, prefabs.Count)];

            if (prefab != null)
            {
                Instantiate(prefab, transform);
                return;
            }
        }
    }

    public enum eGenderType
    {
        Male,
        Female,
    }
}
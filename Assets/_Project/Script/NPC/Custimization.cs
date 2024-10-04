using System.Collections.Generic;
using UnityEngine;

namespace NPC_Stuff
{
    public class Custimization : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer skinMesh;
        [SerializeField] private Transform Head;

        public void Randomize(NPCCustomizationDataSO customizationDataSo, eGenderType genderType)
        {
            var mesh = customizationDataSo.NpcBodyMesh[Random.Range(0, customizationDataSo.NpcBodyMesh.Count)];
            skinMesh.sharedMesh = mesh;

            TryAplly(customizationDataSo.NpcHairPrefabs, Head);
            TryAplly(customizationDataSo.NpcBeardPrefabs, Head);
            if (Random.value > 0.8f) TryAplly(customizationDataSo.NpcAttachtmentPrefabs, Head);
            TryAplly(customizationDataSo.NpcEaringPrefabs, Head);
        }

        public void Customize()
        {
            // You Got It.
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
        Female
    }
}
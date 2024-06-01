using System.Collections;
using BuildingSystem;
using Data;
using New_NPC;
using NPC;
using UnityEngine;
using UnityEngine.Serialization;

namespace System
{
    public class NPCSystem : Singleton<NPCSystem>
    {
        [SerializeField] private GameObject _npcPrefab;
        [FormerlySerializedAs("_sceneTransformContainer")] [SerializeField] private SceneGameObjectHandler sceneGameObjectHandler;

        private int maxNPC = 25;

        private void Start()
        {
            StartCoroutine(CoSpawnNPCs());
        }

        private IEnumerator CoSpawnNPCs()
        {
            int npcCount = 0;

            while (npcCount < maxNPC)
            {
                yield return new WaitForSeconds(0.1f);
                var newNPC = Instantiate(_npcPrefab, GameData.Instance.EnterencePosition, Quaternion.identity);
                newNPC.transform.SetParent(sceneGameObjectHandler.NPCHolderTransform);
                eGenderType gender = UnityEngine.Random.value > 0.5f ? eGenderType.Male : eGenderType.Female;

                switch (gender)
                {
                    case eGenderType.Male:
                        newNPC.GetComponent<NPCRandomizer>().Customize(InitConfig.Instance.GetDefaultBoyNpcCustomization, eGenderType.Male);
                        newNPC.GetComponent<NPC.NPC>().Init(InitConfig.Instance.GetDefaultBoyNpcAnimation);
                        break;
                    case eGenderType.Female:
                        newNPC.GetComponent<NPCRandomizer>().Customize(InitConfig.Instance.GetDefaultGirlNpcCustomization, eGenderType.Female);
                        newNPC.GetComponent<NPC.NPC>().Init(InitConfig.Instance.GetDefaultGirlNpcAnimation);
                        break;
                }
                npcCount++;
            }
        }
    }
}
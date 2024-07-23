using System.Collections;
using System.Collections.Generic;
using BuildingSystem;
using Data;
using New_NPC;
using New_NPC.Activities;
using UnityEngine;
using UnityEngine.Serialization;

namespace System
{
    public class NPCSystem : Singleton<NPCSystem>
    {
        [SerializeField] private GameObject _npcPrefab;

        [SerializeField] private List<NPC> _npcActivities = new List<NPC>();

        public int maxNPC = 25;

        private void Start()
        {
            // StartCoroutine(CoSpawnNPCs());
        }

        public void SendNPCs()
        {
            StartCoroutine(CoSpawnNPCs());
        }

        public void RemoveNPCs()
        {
            foreach (var activity in _npcActivities)
            {
                activity._activityHandler.StartNewActivity(new ExitDiscoActivity());
            }
            
            _npcActivities.Clear();
        }

        private IEnumerator CoSpawnNPCs()
        {
            yield return new WaitForSeconds(0.5f);
            var npcCount = 0;

            while (npcCount < maxNPC)
            {
                yield return new WaitForSeconds(0.1f);
                var newNPC = Instantiate(_npcPrefab,
                    DiscoData.Instance.MapData.EnterencePosition - new Vector3(0, 0.5f, 0), Quaternion.identity);
                newNPC.transform.SetParent(SceneGameObjectHandler.Instance.GetNPCHolderTransform);
                var gender = UnityEngine.Random.value > 0.5f ? eGenderType.Male : eGenderType.Female;

                switch (gender)
                {
                    case eGenderType.Male:
                        newNPC.GetComponent<NPCRandomizer>()
                            .Customize(InitConfig.Instance.GetDefaultBoyNpcCustomization, eGenderType.Male);
                        newNPC.GetComponent<NPC>().Init(InitConfig.Instance.GetDefaultBoyNpcAnimation);
                        break;
                    case eGenderType.Female:
                        newNPC.GetComponent<NPCRandomizer>()
                            .Customize(InitConfig.Instance.GetDefaultGirlNpcCustomization, eGenderType.Female);
                        newNPC.GetComponent<NPC>().Init(InitConfig.Instance.GetDefaultGirlNpcAnimation);
                        break;
                }
                
                _npcActivities.Add(newNPC.GetComponent<NPC>());
                npcCount++;
            }
        }
    }
}
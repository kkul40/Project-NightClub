﻿using System.Collections;
using System.Collections.Generic;
using BuildingSystem;
using Data;
using New_NPC;
using New_NPC.Activities;
using PropBehaviours;
using UnityEngine;
using UnityEngine.Serialization;

namespace System
{
    public class NPCSystem : Singleton<NPCSystem>
    {
        [SerializeField] private GameObject _npcPrefab;
        [SerializeField] private GameObject _bartenderPrefab;
        [SerializeField] private GameObject _djPrefab;

        [SerializeField] private List<NPC> _npcActivities = new();

        public int maxNPC = 25;

        public Coroutine _npcSpawnRoutine = null;

        public static event Action OnBartenderCreated;

        public void CreateCharacter(int ePersonTypeInt)
        {
            CreateCharacter((ePersonType)ePersonTypeInt);
        }

        public void CreateCharacter(ePersonType personType)
        {
            switch (personType)
            {
                case ePersonType.NPC:
                    SendNPCs();
                    break;
                case ePersonType.Bartender:
                    SendBartender();
                    break;
                case ePersonType.DJ:
                    break;
                default:
                    Debug.LogError("Could Not Found The Person Type");
                    break;
            }
        }

        private void SendBartender()
        {
            var newBartender = Instantiate(_bartenderPrefab,
                DiscoData.Instance.MapData.EnterencePosition -
                new Vector3(0, 0.5f, 0),
                Quaternion.identity);

            newBartender.transform.SetParent(SceneGameObjectHandler.Instance.GetEmployeeHolderTransform);
            OnBartenderCreated?.Invoke();
        }

        private void SendNPCs()
        {
            _npcSpawnRoutine = StartCoroutine(CoSpawnNPCs());
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
                        newNPC.GetComponent<Custimization>()
                            .Randomize(InitConfig.Instance.GetDefaultBoyNpcCustomization, eGenderType.Male);
                        newNPC.GetComponent<NPC>().Init(InitConfig.Instance.GetDefaultBoyNpcAnimation);
                        break;
                    case eGenderType.Female:
                        newNPC.GetComponent<Custimization>()
                            .Randomize(InitConfig.Instance.GetDefaultGirlNpcCustomization, eGenderType.Female);
                        newNPC.GetComponent<NPC>().Init(InitConfig.Instance.GetDefaultGirlNpcAnimation);
                        break;
                }

                _npcActivities.Add(newNPC.GetComponent<NPC>());
                npcCount++;
            }
        }

        public void RemoveNPCs()
        {
            // TODO There is bugg here 'causing null referance on Transforms
            StopCoroutine(_npcSpawnRoutine);
            foreach (var activity in _npcActivities)
                activity._activityHandler.StartNewActivity(new ExitDiscoActivity());

            _npcActivities.Clear();
        }
    }
}
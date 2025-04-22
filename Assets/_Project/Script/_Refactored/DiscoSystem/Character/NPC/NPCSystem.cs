using System.Collections;
using System.Collections.Generic;
using Data;
using DiscoSystem.Building_System;
using DiscoSystem.Building_System.GameEvents;
using DiscoSystem.Character._Player;
using DiscoSystem.Character.Bartender;
using DiscoSystem.Character.Bartender.Command;
using DiscoSystem.Character.NPC.Activity;
using DiscoSystem.Character.NPC.Activity.Activities;
using UnityEngine;

namespace DiscoSystem.Character.NPC
{
    public class NPCSystem : Singleton<NPCSystem>
    {
        public ActivityGiver activityGiver;
        private DiscoData _discoData;
        
        [SerializeField] private GameObject _npcPrefab;
        [SerializeField] private GameObject _bartenderPrefab;
        [SerializeField] private GameObject _djPrefab;

        [SerializeField] private List<NPC> _npcs = new();

        public int maxNPC = 25;

        public Coroutine _npcSpawnRoutine = null;

        public void Initialize(DiscoData discoData)
        {
            activityGiver = new ActivityGiver();
            _discoData = discoData;
        }
        
        /// <summary>
        /// Sends NPC TO Disco
        /// </summary>
        /// <param name="ePersonTypeInt"></param>
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
            var newBartender = Instantiate(_bartenderPrefab, DiscoData.Instance.MapData.SpawnPositon,
                Quaternion.identity);

            newBartender.transform.SetParent(SceneGameObjectHandler.Instance.GetEmployeeHolderTransform);

            var gender = UnityEngine.Random.value > 0.5f ? eGenderType.Male : eGenderType.Female;

            CharacterCustomizer customizer =
                new CharacterCustomizer(gender, InitConfig.Instance.GetefaultBartenderCustomization, newBartender.transform);

            newBartender.GetComponent<Bartender.Bartender>().Init(customizer.GetAnimator, customizer.GetAnimancer, customizer.GetArmature);

            var bartender = newBartender.GetComponent<IBartender>();

            var command = new WalkToEntranceCommand();
            command.InitCommand(null, bartender);
            bartender.AddCommand(command);
            
            GameEvent.Trigger(new Event_BartenderHired(bartender));
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
                NPC npc = CreateNPC();
                _npcs.Add(npc);
                npcCount++;
            }
        }

        public void RemoveNPCs()
        {
            if (_npcSpawnRoutine != null)
                StopCoroutine(_npcSpawnRoutine);

            foreach (var npc in _npcs)
            {
                if (npc == null) continue;
                if(npc._activityHandler.GetCurrentActivity is ExitDiscoActivity) continue;
                npc._activityHandler.StartNewActivity(new ExitDiscoActivity());
            }

            _npcs.Clear();
        }

        private NPC CreateNPC()
        {
            var newNPC = Instantiate(_npcPrefab, DiscoData.Instance.MapData.SpawnPositon, Quaternion.identity);
            newNPC.transform.SetParent(SceneGameObjectHandler.Instance.GetNPCHolderTransform);
            var gender = UnityEngine.Random.value > 0.5f ? eGenderType.Male : eGenderType.Female;

            NPC Npc;
            if (newNPC.TryGetComponent(out NPC npc))
                Npc = npc;
            else
                return null;

            CharacterCustomizer customizer = null;
            NewAnimationSO animation = null;

            switch (gender)
            {
                case eGenderType.Male:
                    customizer = new CharacterCustomizer(eGenderType.Male ,InitConfig.Instance.GetDefaultNPCCustomization, newNPC.transform);
                    animation = InitConfig.Instance.GetDefaultBoyNpcAnimation;
                    break;
                case eGenderType.Female:
                    customizer = new CharacterCustomizer(eGenderType.Female, InitConfig.Instance.GetDefaultNPCCustomization, newNPC.transform);
                    animation = InitConfig.Instance.GetDefaultGirlNpcAnimation;
                    break;
            }

            if (customizer == null || animation == null) return null;
            
            Npc.Initialize(animation, customizer.GetAnimator, customizer.GetAnimancer, customizer.GetArmature, _discoData);
            
            return Npc;
        }
    }
}
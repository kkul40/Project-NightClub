using System.Collections;
using System.Collections.Generic;
using _Initializer;
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
    public class NPCSystem : MonoBehaviour
    {
        public ActivityGiver activityGiver;
        
        [SerializeField] private GameObject _npcPrefab;
        [SerializeField] private GameObject _bartenderPrefab;
        [SerializeField] private GameObject _djPrefab;

        [SerializeField] private List<NPC> npcList;
        [SerializeField] private List<Bartender.Bartender> bartenderList;

        public int maxNPC = 25;

        public Coroutine _npcSpawnRoutine = null;

        public void Initialize()
        {
            activityGiver = new ActivityGiver();
            npcList = new List<NPC>();
            bartenderList = new List<Bartender.Bartender>();
            ServiceLocator.Register(this);
        }

        public void RegisterBartender(Bartender.Bartender bartender)
        {
            bartenderList.Add(bartender);
        }
        
        public void UnRegisterBartender(Bartender.Bartender bartender)
        {
            if(bartenderList.Contains(bartender))
                bartenderList.Remove(bartender);
        }
        
        private void Update()
        {
            List<NPC> toRemove = new List<NPC>();
            
            foreach (var npc in npcList)
            {
                if (npc.ActivityHandler == null) continue;
                
                npc.ActivityHandler.UpdateActivity();
#if UNITY_EDITOR
                npc.debugState = npc.ActivityHandler.GetCurrentActivity.GetType().Name;
#endif
                if (npc.ActivityHandler.isDead)
                {
                    toRemove.Add(npc);
                }
            }

            foreach (var npc in toRemove)
            {
                npcList.Remove(npc);
                Destroy(npc.gameObject);
            }

            foreach (var bartender in bartenderList)
            {
                if (bartender.BartenderCommands.Count > 0)
                    bartender.UpdateCommand();
            }
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

            newBartender.transform.SetParent(ServiceLocator.Get<SceneGameObjectHandler>().GetEmployeeHolderTransform);

            var gender = Random.value > 0.5f ? eGenderType.Male : eGenderType.Female;

            CharacterCustomizer customizer =
                new CharacterCustomizer(gender, ServiceLocator.Get<InitConfig>().GetefaultBartenderCustomization, newBartender.transform);

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
                CreateNPC();
                // _npcs.Add(npc);
                npcCount++;
            }
        }

        public void RemoveNPCs()
        {
            if (_npcSpawnRoutine != null)
                StopCoroutine(_npcSpawnRoutine);

            foreach (var npc in npcList)
            {
                if (npc == null) continue;
                if(npc.ActivityHandler.GetCurrentActivity is ExitDiscoActivity) continue;
                npc.ActivityHandler.StartNewActivity(new ExitDiscoActivity());
            }

            // _npcs.Clear();
        }

        private NPC CreateNPC()
        {
            var newNPC = Instantiate(_npcPrefab, DiscoData.Instance.MapData.SpawnPositon, Quaternion.identity);
            newNPC.transform.SetParent(ServiceLocator.Get<SceneGameObjectHandler>().GetNPCHolderTransform);
            var gender = Random.value > 0.5f ? eGenderType.Male : eGenderType.Female;

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
                    customizer = new CharacterCustomizer(eGenderType.Male ,ServiceLocator.Get<InitConfig>().GetDefaultNPCCustomization, newNPC.transform);
                    animation = ServiceLocator.Get<InitConfig>().GetDefaultBoyNpcAnimation;
                    break;
                case eGenderType.Female:
                    customizer = new CharacterCustomizer(eGenderType.Female, ServiceLocator.Get<InitConfig>().GetDefaultNPCCustomization, newNPC.transform);
                    animation = ServiceLocator.Get<InitConfig>().GetDefaultGirlNpcAnimation;
                    break;
            }

            if (customizer == null || animation == null) return null;
            
            Npc.Initialize(animation, customizer.GetAnimator, customizer.GetAnimancer, customizer.GetArmature);
            npcList.Add(npc);
            
            return Npc;
        }
    }
}
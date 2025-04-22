using Data;
using DiscoSystem.Character._Player;
using DiscoSystem.Character.NPC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TEST
{
    public class AnimationTester : MonoBehaviour
    {
        public static AnimationTester Instance;
        
        [SerializeField] private GameObject _npcPrefab;

        private IAnimationController npcAnimationController;

        private GameObject npcObject;

        public AvatarMask holdmybeerMask;

        private void Awake()
        {
            Instance = this;
            CreateMaleNPC();
        }

        [Button]
        public void CreateMaleNPC()
        {
            if (npcObject != null)
            {
                Destroy(npcObject);
            }
            
            var newNPC = Instantiate(_npcPrefab, Vector3.zero, Quaternion.identity);
            newNPC.transform.SetParent(transform);
            AnimationTestNPC Npc = newNPC.AddComponent<AnimationTestNPC>();
            
            if(newNPC.TryGetComponent(out NPC npc))
                Destroy(npc);
            
            CharacterCustomizer customizer = null;
            NewAnimationSO animation = null;
            
            customizer = new CharacterCustomizer(eGenderType.Male, InitConfig.Instance.GetDefaultNPCCustomization, newNPC.transform);
            animation = InitConfig.Instance.GetDefaultBoyNpcAnimation;

            if (customizer == null || animation == null) return;
            
            Npc.Initialize(animation, customizer.GetAnimator, customizer.GetAnimancer, customizer.GetArmature);
            npcAnimationController = Npc.AnimationController;

            npcObject = newNPC;
        }

        public void PlayAnimationOnLayerOne(int index)
        {
            switch (index)
            {
                case 0:
                    npcAnimationController.PlayAnimation(eAnimationType.NPC_Idle);
                    break;
                case 1:
                    npcAnimationController.PlayAnimation(eAnimationType.NPC_Walk);
                    break;
                case 2:
                    npcAnimationController.PlayAnimation(eAnimationType.NPC_Dance);
                    break;
            }
        }

        public void PlayActionAnimation(int index)
        {
            switch (index)
            {
                case 0:
                    npcAnimationController.PlayActionAnimation(eActionAnimationType.Null);
                    break;
                case 1:
                    npcAnimationController.PlayActionAnimation(eActionAnimationType.NPC_HoldDrink);
                    break;
            }
        }
    }
}
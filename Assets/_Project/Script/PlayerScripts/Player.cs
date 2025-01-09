using CharacterCustomization;
using Data;
using DiscoSystem;
using NPCBehaviour;
using NPCBehaviour.PathFinder;
using UI.GamePages;
using UnityEngine;

namespace PlayerScripts
{
    public class Player : MonoBehaviour, IDoorTrigger, ISaveLoad
    {
        private IAnimationController _animationController;
        private NpcPathFinder _pathFinder;
        private CharacterCustomizeLoader _customizeLoader;

        private void Awake()
        {
            _pathFinder = new NpcPathFinder(transform);
        }

        private void Update()
        {
            if (InputSystem.Instance.RightClickOnWorld && !UIPageManager.Instance.IsAnyUIToggled())
            {
                _pathFinder.GoTargetDestination(InputSystem.Instance.MousePosition, SetIdleAnimation);
                _animationController?.PlayAnimation(eAnimationType.NPC_Walk);
            }
        }

        private void SetIdleAnimation()
        {
            _animationController.PlayAnimation(eAnimationType.NPC_Idle);
        }

        public bool TriggerDoor { get; set; } = true;
        public void LoadData(GameData gameData)
        {
            _customizeLoader = GetComponent<CharacterCustomizeLoader>();
            _customizeLoader.Init(gameData);
            
            var animation = _customizeLoader.gender == eGenderType.Male
                ? InitConfig.Instance.GetDefaultBoyNpcAnimation
                : InitConfig.Instance.GetDefaultGirlNpcAnimation;
            
            _animationController = new NPCAnimationControl(_customizeLoader.GetAnimator, _customizeLoader.GetAnimancer, animation, _customizeLoader.GetArmature);
            _animationController.PlayAnimation(eAnimationType.Bartender_Walk);
        }

        public void SaveData(ref GameData gameData)
        {
        }
    }
}
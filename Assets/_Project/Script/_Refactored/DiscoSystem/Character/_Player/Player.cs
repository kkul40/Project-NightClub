using CharacterCustomization;
using Data;
using Data.New;
using DiscoSystem.Character.NPC;
using UI.GamePages;
using UnityEngine;

namespace DiscoSystem.Character._Player
{
    public class Player : MonoBehaviour, IDoorTrigger
    {
        private IAnimationController _animationController;
        private NpcPathFinder _pathFinder;
        private CharacterCustomizeLoader _customizeLoader;

        public void Initialize(NewGameData gameData)
        {
            _pathFinder = new NpcPathFinder(transform, PathUserType.Player);
            LoadData(gameData);
        }
  
        private void Update()
        {
            if (InputSystem.Instance.RightClickOnWorld && !UIPageManager.Instance.IsAnyUIToggled())
            {
                _pathFinder.GoToDestination(InputSystem.Instance.MousePosition, SetIdleAnimation);
                _animationController?.PlayAnimation(eAnimationType.NPC_Walk);
            }
        }

        private void SetIdleAnimation()
        {
            _animationController.PlayAnimation(eAnimationType.NPC_Idle);
        }

        public bool TriggerDoor { get; set; } = true;
        

        public void LoadData(NewGameData gameData)
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
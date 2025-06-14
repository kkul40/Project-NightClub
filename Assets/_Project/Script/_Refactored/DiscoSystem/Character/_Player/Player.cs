using CharacterCustomization;
using Data;
using Data.New;
using DiscoSystem.Character.NPC;
using DiscoSystem.NewPathFinder;
using UI.GamePages;
using UnityEngine;
using UnityEngine.AI;

namespace DiscoSystem.Character._Player
{
    public class Player : MonoBehaviour, IDoorTrigger
    {
        private IAnimationController _animationController;
        private NpcPathFinder _pathFinder;
        private CharacterCustomizeLoader _customizeLoader;

        private PathFindingAgent _agent;

        public void Initialize(NewGameData gameData, DiscoData discoData)
        {
            _pathFinder = new NpcPathFinder(transform, PathUserType.Player);
            LoadData(gameData);

            _agent = new PathFindingAgent(transform, PathUserType.Player);

        }
  
        private void Update()
        {
            if (_agent == null) return;
            
            if (InputSystem.Instance.GetRightClickOnWorld(InputType.WasPressedThisFrame) && !UIPageManager.Instance.IsAnyUIToggled())
            {
                // Original
                // _pathFinder.GoToDestination(InputSystem.Instance.MousePosition, SetIdleAnimation);
                // _animationController?.PlayAnimation(eAnimationType.NPC_Walk);
                _agent.SetDestination(InputSystem.Instance.MousePosition);
            }
            
            _agent.Update(Time.deltaTime);

            if (!_agent.isStopped)
                _animationController?.PlayAnimation(eAnimationType.NPC_Walk);
            else
                _animationController?.PlayAnimation(eAnimationType.NPC_Idle);
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
    }
}
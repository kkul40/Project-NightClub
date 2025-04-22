using DiscoSystem.Building_System.GameEvents;
using DiscoSystem.ObjectPooling;
using UnityEngine;

namespace UI.Emotes
{
    public enum EmoteTypes
    {
        Happy,
        InLove,
        Sad,
        Angry,
        InFight,
        Disgusted,
        Suprized,
        Exclamation,
    }
    
    public class UIEmoteShower : MonoBehaviour
    {
        [SerializeField] private GameObject _emotePrefab;

        [Header("Emotes")]
        [SerializeField] private Sprite happy;
        [SerializeField] private Sprite inLove;
        [SerializeField] private Sprite sad;
        [SerializeField] private Sprite angry;
        [SerializeField] private Sprite inFight;
        [SerializeField] private Sprite disgusted;
        [SerializeField] private Sprite suprized;
        [SerializeField] private Sprite exclamation;
        
        private ObjectPooler _pool;

        private void Start()
        {
            _pool = new ObjectPooler(_emotePrefab);
            GameEvent.Subscribe<Event_ShowEmote>(ShowEmote);
        }

        private void ShowEmote(Event_ShowEmote emoteEvent)
        {
            Sprite selected = null;
            
            switch (emoteEvent.EmoteTypes)
            {
                case EmoteTypes.Happy:
                    selected = happy;
                    break;
                case EmoteTypes.InLove:
                    selected = inLove;
                    break;
                case EmoteTypes.Sad:
                    selected = sad;
                    break;
                case EmoteTypes.Angry:
                    selected = angry;
                    break;
                case EmoteTypes.InFight:
                    selected = inFight;
                    break;
                case EmoteTypes.Disgusted:
                    selected = disgusted;
                    break;
                case EmoteTypes.Suprized:
                    selected = suprized;
                    break;
                case EmoteTypes.Exclamation:
                    selected = exclamation;
                    break;
                default:
                    Debug.LogError(emoteEvent.EmoteTypes + " : Sprite Is Missing");
                    return;
            }

            var emote = _pool.GetObject(2);
            
            if (emote.TryGetComponent(out UIEmote uiEmote))
            {
                uiEmote.Init(selected, emoteEvent.Target);
            }
        }
    }
}
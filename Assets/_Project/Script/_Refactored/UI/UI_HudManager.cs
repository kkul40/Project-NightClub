using GameEvents;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UI_HudManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI balanceText;

        public void Initialize()
        {
            GameEvent.Subscribe<Event_BalanceUpdated>(UpdateBalance);
        }

        private void UpdateBalance(Event_BalanceUpdated balanceEvent)
        {
            balanceText.text = balanceEvent.Balance.ToString();
        }
    }
}
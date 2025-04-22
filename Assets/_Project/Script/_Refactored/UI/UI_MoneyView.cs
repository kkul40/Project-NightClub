using DiscoSystem.Building_System.GameEvents;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UI_MoneyView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _moneyText;

        private void Awake()
        {
            _moneyText.text = "0";
            GameEvent.Subscribe<Event_BalanceUpdated>(UpdateBalance);
        }

        private void UpdateBalance(Event_BalanceUpdated balanceEvent)
        {
            _moneyText.text = balanceEvent.Balance.ToString();
            Debug.Log("Money Text Updated");
        }
    }
}
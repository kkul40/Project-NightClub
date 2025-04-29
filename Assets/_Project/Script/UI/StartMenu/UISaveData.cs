using System;
using Data;
using Data.New;
using TMPro;
using UnityEngine;

namespace UI.StartMenu
{
    public class UISaveData : MonoBehaviour
    {
        private NewGameData _data;

        public event Action<string> OnDelete;
        public event Action<string> OnLoad;

        [SerializeField] private TextMeshProUGUI _saveName;
        [SerializeField] private TextMeshProUGUI _playTime;
        [SerializeField] private TextMeshProUGUI _lastSaveDate;
        [SerializeField] private TextMeshProUGUI _balance;
        
        public void Init(NewGameData data)
        {
            _data = data;

            // _saveName.text = data.saveDetails.fileName;
            // _playTime.text = data.saveDetails.playTime.ToString();
            // _lastSaveDate.text = data.saveDetails.lastSaveDate;
            // _balance.text = data.savedInventoryData.Balance.ToString();
        }

        public void LoadGame()
        {
            OnLoad?.Invoke(_data.fileName);
        }

        public void DeleteGame()
        {
            OnDelete?.Invoke(_data.fileName);
        }
    }
}
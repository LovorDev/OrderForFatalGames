using System;
using BuffsScript;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIScripts.BuffsUIScripts
{
    public class BaseUIBuff : MonoBehaviour
    {
        
        [SerializeField]
        private Button _button;

        [SerializeField]
        private TextMeshProUGUI _amountBuff;

        [field: SerializeField]
        public BuffType BuffType { get; private set; }

        public event Action<BuffType> ClickBuff;
        
        public int CurrentAmount { get; private set; }

        private void Start()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            ClickBuff?.Invoke(BuffType);
        }

        public void AddBuffAmount()
        {
            CurrentAmount++;
            UpdateText();
        }
        
        private void UpdateText()
        {
            _amountBuff.text = CurrentAmount.ToString();
        }
        public void SetBuffAmount(int value)
        {
            CurrentAmount = value;
            UpdateText();
        }

        public void RemoveBuffAmount()
        {
            CurrentAmount--;
            UpdateText();
        }
    }
}
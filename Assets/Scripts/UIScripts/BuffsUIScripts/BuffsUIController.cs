using System.Collections.Generic;
using System.Linq;
using BuffsScript;
using UnityEngine;

namespace UIScripts.BuffsUIScripts
{
    public class BuffsUIController: MonoBehaviour
    {
        [SerializeField]
        private List<BaseUIBuff> _baseUIBuffs;
        
        public List<BaseUIBuff> UIBuffs=>_baseUIBuffs;
        

        private Dictionary<BuffType, BaseUIBuff> _uiBuffsdictionare;

        private void Start()
        {
            _uiBuffsdictionare = _baseUIBuffs.ToDictionary(k => k.BuffType);
        }

        public void AddBuffAmount(BuffType buffType)
        {
            _uiBuffsdictionare[buffType].AddBuffAmount();
            UpdateUIBuffState(buffType);
        }

        public void RemoveBuffAmount(BuffType buffType)
        {
            _uiBuffsdictionare[buffType].RemoveBuffAmount();
            UpdateUIBuffState(buffType);
        }
        
        public void OnBuffValueChange(BuffType buffType,int currentAmount)
        {
            _uiBuffsdictionare[buffType].SetBuffAmount(currentAmount);
            UpdateUIBuffState(buffType);

        }

        private void UpdateUIBuffState(BuffType buffType)
        {
            _uiBuffsdictionare[buffType].gameObject.SetActive(_uiBuffsdictionare[buffType].CurrentAmount > 0);
        }
    }
}
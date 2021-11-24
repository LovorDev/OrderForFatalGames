using System.Collections.Generic;
using BuffsScript;
using BuffsScript.UsingEffect;
using UIScripts.BuffsUIScripts;
using UnityEngine;

namespace CharacterScripts
{
    public class BuffController : MonoBehaviour
    {
        private Dictionary<BuffType, Stack<UsingEffect>> _usingEffectsDictionary;

        [SerializeField]
        private BuffsUIController _buffsUIController;

        private void Start()
        {
            _usingEffectsDictionary = new Dictionary<BuffType, Stack<UsingEffect>>();

            foreach (var uiBuff in _buffsUIController.UIBuffs)
            {
                uiBuff.ClickBuff += OnBuffClicked;
            }
        }

        private void OnBuffClicked(BuffType type)
        {
            var itemToUse = _usingEffectsDictionary[type].Pop();
            itemToUse.Use();
            RemoveEffect(type);
        }


        public void RemoveEffect(BuffType buffType)
        {
            _buffsUIController.RemoveBuffAmount(buffType);
        }

        public void AddEffect(Buff buff)
        {
            if (!_usingEffectsDictionary.ContainsKey(buff.BuffType))
            {
                var newStack = new Stack<UsingEffect>();
                _usingEffectsDictionary.Add(buff.BuffType, newStack);
            }

            var newItem = Instantiate(buff.UsingEffect, transform);
            
            _usingEffectsDictionary[buff.BuffType].Push(newItem);
            _buffsUIController.AddBuffAmount(buff.BuffType);
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.Events;

namespace BuffsScript.UsingEffect
{
    public abstract class UsingEffect : MonoBehaviour
    {
        [Serializable]
        public class UseBuffEvent : UnityEvent
        {
        }

        [SerializeField]
        private UseBuffEvent _useBuffEvent;

        [SerializeField]
        private float _destroyDelay = .5f;

        public void Use()
        {
            _useBuffEvent?.Invoke();
            UseBuff();
            Destroy(gameObject, _destroyDelay);
        }

        public abstract void UseBuff();
    }
}
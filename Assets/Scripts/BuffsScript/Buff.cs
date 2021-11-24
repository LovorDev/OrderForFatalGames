using UnityEngine;

namespace BuffsScript
{

    public enum BuffType
    {
        Grenade,
        Chainsaw
    }
    public class Buff : MonoBehaviour, IPickable
    {
        [field: SerializeField]
        public UsingEffect.UsingEffect UsingEffect { get; private set; }

        [field: SerializeField]
        public BuffType BuffType { get; private set; }
    }
}
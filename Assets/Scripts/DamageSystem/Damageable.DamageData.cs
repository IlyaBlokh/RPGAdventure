using UnityEngine;

namespace DamageSystem
{
    public partial class Damageable : MonoBehaviour
    {
        public struct DamageData
        {
            public Damageable DamageReceiver;
            public MonoBehaviour DamageDealer;
            public GameObject DamageSender;
            public float DamageAmount;
        }
    }
}
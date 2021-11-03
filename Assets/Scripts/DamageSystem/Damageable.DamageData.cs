using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
    public partial class Damageable : MonoBehaviour
    {
        public struct DamageData
        {
            public MonoBehaviour DamageDealer;
            public float DamageAmount;
            public Vector3 DamageSourcePosition;
        }
    }
}
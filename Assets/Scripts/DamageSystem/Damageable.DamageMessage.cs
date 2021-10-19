using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
    public partial class Damageable : MonoBehaviour
    {
        public struct DamageMessage
        {
            MonoBehaviour DamageDealer;
            float DamageAmount;
            Vector3 DamageSourcePosition;
        }
    }
}
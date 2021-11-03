using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
    public interface IDamageMessageReceiver
    {
        enum DamageMessageType
        {
            DAMAGED,
            DEAD
        }

        void OnDamageMessageReceive(DamageMessageType messageType, Damageable.DamageData damageData);
    }
}

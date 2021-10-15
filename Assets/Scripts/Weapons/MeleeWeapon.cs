using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
    public class MeleeWeapon : MonoBehaviour
    {
        [SerializeField]
        float Damage;

        public void Attack()
        {
            Debug.Log("Melee weapon is swinging");
        }
    }
}

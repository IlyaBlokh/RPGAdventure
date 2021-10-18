using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
    public class Damageable : MonoBehaviour
    {
        [SerializeField]
        float maxHP;

        public void ApplyDamage()
        {
            Debug.Log("Apply damage");
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPGAdventure
{
    public class DamageableUI : MonoBehaviour
    {
        [SerializeField]
        Slider HealthBar;

        public void SetMaxHP(float value)
        {
            HealthBar.maxValue = value;
            SetHP(value);
        }

        public void SetHP(float value)
        {
            HealthBar.value = value;
        }
    }
}
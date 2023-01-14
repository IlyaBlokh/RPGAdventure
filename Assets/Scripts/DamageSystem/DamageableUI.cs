using UnityEngine;
using UnityEngine.UI;

namespace DamageSystem
{
    public class DamageableUI : MonoBehaviour
    {
        [SerializeField] private GameObject HealthUIPrefab;

        private Slider healthBar;

        public GameObject HealthUI => HealthUIPrefab;

        private void Awake()
        {
            healthBar = HealthUI.GetComponentInChildren<Slider>();
        }

        public void SetMaxHp(float value)
        {
            if (HealthUI)
            {
                healthBar.maxValue = value;
                SetHp(value);
            }
        }

        public void SetHp(float value)
        {
            if (HealthUI)
                healthBar.value = value;
        }
    }
}
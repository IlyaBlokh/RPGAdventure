using UnityEngine;
using UnityEngine.UI;

namespace DamageSystem
{
    public class DamageableUI : MonoBehaviour
    {
        [SerializeField]
        GameObject HealthUIPrefab;

        private Slider m_HealthBar;

        public GameObject HealthUI { get => HealthUIPrefab; }

        private void Awake()
        {
            m_HealthBar = HealthUI.GetComponentInChildren<Slider>();
        }

        public void SetMaxHP(float value)
        {
            if (HealthUI)
            {
                m_HealthBar.maxValue = value;
                SetHP(value);
            }
        }

        public void SetHP(float value)
        {
            if (HealthUI)
                m_HealthBar.value = value;
        }
    }
}
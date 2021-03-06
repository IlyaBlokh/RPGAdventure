using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPGAdventure
{
    public partial class Damageable : MonoBehaviour
    {
        [SerializeField]
        float maxHP;

        [SerializeField][Range(0, 360f)]
        float hitAngle;

        [SerializeField]
        int experienceForKill;

        [SerializeField]
        LayerMask layerToInformPlayer;

        [SerializeField]
        List<MonoBehaviour> DamageMessageListeners;

        [SerializeField]
        float UnvulnerabilityTime = 0.25f;

        [SerializeField]
        DamageableUI DamageableUI;

        private float m_currentHP;
        private bool m_isVulnerable;

        public int ExperienceForKill { get => experienceForKill; set => experienceForKill = value; }
        public float MaxHP { get => maxHP; set => maxHP = value; }
        public float CurrentHP { get => m_currentHP; private set => m_currentHP = value; }

        private void Awake()
        {   
            m_isVulnerable = true;
            if ((layerToInformPlayer.value & (1 << gameObject.layer)) != 0)
            {
                DamageMessageListeners.Add(FindObjectOfType<QuestManager>());
                DamageMessageListeners.Add(FindObjectOfType<PlayerStats>());
            }

        }

        void Start()
        {
            DamageableUI = GetComponent<DamageableUI>();
            ResetHealth();
        }

        public void ResetHealth()
        {
            CurrentHP = maxHP;
            DamageableUI?.SetMaxHP(maxHP);
        }

        public void ApplyDamage(DamageData data)
        {
            if (!m_isVulnerable) return;

            if (m_currentHP <= 0) return;

            Vector3 toDamageDealer = data.DamageSender.transform.position - transform.position;
            toDamageDealer.y = 0;
            if (Vector3.Angle(toDamageDealer, transform.forward) > hitAngle / 2) 
                return;

            m_currentHP -= data.DamageAmount;
            DamageableUI?.SetHP(m_currentHP);

            var messageType = m_currentHP <= 0 ? IMessageReceiver.MessageType.DEAD : IMessageReceiver.MessageType.DAMAGED;
            foreach(var damageMessageListener in DamageMessageListeners)
            {
                (damageMessageListener as IMessageReceiver).OnMessageReceive(messageType, data);
            }

            StartCoroutine(SetUnvulnerability());
        }

        private IEnumerator SetUnvulnerability()
        {
            m_isVulnerable = false;
            yield return new WaitForSeconds(UnvulnerabilityTime);
            m_isVulnerable = true;
        }


#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = new Color(.0f, .0f, 0.8f, 0.4f);
            Vector3 rotatedForward = Quaternion.AngleAxis(-hitAngle / 2, transform.up) * transform.forward;
            UnityEditor.Handles.DrawSolidArc(
                transform.position,
                Vector3.up,
                rotatedForward,
                hitAngle,
                1.0f);
        }

#endif
    }
}
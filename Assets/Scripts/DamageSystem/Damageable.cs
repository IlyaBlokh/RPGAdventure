using System.Collections;
using System.Collections.Generic;
using Player;
using QuestSystem;
using UnityEngine;

namespace DamageSystem
{
    public partial class Damageable : MonoBehaviour
    {
        [SerializeField] private float maxHP;
        [SerializeField][Range(0, 360f)] private float hitAngle;
        [SerializeField] private int experienceForKill;
        [SerializeField] private LayerMask layerToInformPlayer;
        [SerializeField] private List<MonoBehaviour> DamageMessageListeners;
        [SerializeField] private float UnvulnerabilityTime = 0.25f;
        [SerializeField] private DamageableUI DamageableUI;

        private bool isVulnerable;

        public int ExperienceForKill => experienceForKill;

        private float CurrentHp { get; set; }

        private void Awake()
        {   
            isVulnerable = true;
            if ((layerToInformPlayer.value & (1 << gameObject.layer)) != 0)
            {
                DamageMessageListeners.Add(FindObjectOfType<QuestManager>());
                DamageMessageListeners.Add(FindObjectOfType<PlayerStats>());
            }
        }

        private void Start()
        {
            DamageableUI = GetComponent<DamageableUI>();
            ResetHealth();
        }

        public void ResetHealth()
        {
            CurrentHp = maxHP;
            DamageableUI.SetMaxHp(maxHP);
        }

        public void ApplyDamage(DamageData data)
        {
            if (!isVulnerable) return;

            if (CurrentHp <= 0) return;

            Vector3 toDamageDealer = data.DamageSender.transform.position - transform.position;
            toDamageDealer.y = 0;
            if (Vector3.Angle(toDamageDealer, transform.forward) > hitAngle / 2) 
                return;

            CurrentHp -= data.DamageAmount;
            DamageableUI.SetHp(CurrentHp);

            IMessageReceiver.MessageType messageType = CurrentHp <= 0 ? IMessageReceiver.MessageType.Dead : IMessageReceiver.MessageType.Damaged;
            foreach(MonoBehaviour damageMessageListener in DamageMessageListeners)
            {
                (damageMessageListener as IMessageReceiver)?.OnMessageReceive(messageType, data);
            }

            StartCoroutine(SetUnvulnerability());
        }

        private IEnumerator SetUnvulnerability()
        {
            isVulnerable = false;
            yield return new WaitForSeconds(UnvulnerabilityTime);
            isVulnerable = true;
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
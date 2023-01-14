using Audio;
using DamageSystem;
using UnityEngine;
using Utils;

namespace Weapons
{
    [RequireComponent(typeof(FixedUpdateFollow))]
    public class MeleeWeapon : MonoBehaviour
    {
        [System.Serializable]
        public class AttackPoint
        {
            public float radius;
            public Vector3 offset;
            public Transform root;
        }

        [SerializeField]
        float Damage;

        [SerializeField]
        AttackPoint[] attackPoints = new AttackPoint[0];

        [SerializeField]
        LayerMask targetLayers;

        [SerializeField]
        RandomAudioPlayer SwingAudioPlayer;
        [SerializeField]
        RandomAudioPlayer ImpactAudioPlayer;

        private GameObject m_Owner;
        private bool isAttacking = false;
        private Vector3[] originalAttackPointPosition;
        private RaycastHit[] raycastHits = new RaycastHit[32];

        public GameObject Owner { get => m_Owner; set => m_Owner = value; }

        private void FixedUpdate()
        {
            if (isAttacking)
            {
                for (var i = 0; i < attackPoints.Length; i++)
                {
                    AttackPoint ap = attackPoints[i];
                    Vector3 currentWorldPos = ap.root.position +
                        ap.root.TransformVector(attackPoints[i].offset);
                    Vector3 attackVector = (currentWorldPos - originalAttackPointPosition[i]).normalized;
                    Ray ray = new Ray(originalAttackPointPosition[i], attackVector);
                    Debug.DrawRay(originalAttackPointPosition[i], attackVector, Color.red, 4.0f);

                    var hitCount = Physics.SphereCastNonAlloc(
                        ray,
                        ap.radius,
                        raycastHits,
                        attackVector.magnitude,
                        ~0,
                        QueryTriggerInteraction.Ignore); 

                    for (var j =0; j < hitCount; j++)
                    {
                        CheckDamage(raycastHits[j].collider);
                    }

                    originalAttackPointPosition[i] = currentWorldPos;
                }
            }
        }

        public void UpdateAttack(bool IsAttacking)
        {
            isAttacking = IsAttacking;
            if (isAttacking)
            {
                SwingAudioPlayer.PlayRandomClip();
                originalAttackPointPosition = new Vector3[attackPoints.Length];
                for (var i = 0; i < attackPoints.Length; i++)
                {
                    AttackPoint ap = attackPoints[i];
                    originalAttackPointPosition[i] =
                        ap.root.position +
                        ap.root.TransformVector(attackPoints[i].offset);
                }
            }
        }

        private void CheckDamage(Collider other)
        {
            if ((targetLayers.value & (1 << other.gameObject.layer)) == 0) return;
            Damageable damageableComponent = other.GetComponent<Damageable>();
            if (damageableComponent != null)
            {
                Damageable.DamageData data;
                data.DamageReceiver = damageableComponent;
                data.DamageDealer = this;
                data.DamageAmount = Damage;
                data.DamageSender = m_Owner;

                ImpactAudioPlayer.PlayRandomClip();
                damageableComponent.ApplyDamage(data);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(.0f, 0.7f, .0f, 0.4f);
            foreach(AttackPoint attackPoint in attackPoints)
            {
                var worldOffset = attackPoint.root.TransformVector(attackPoint.offset);
                Gizmos.DrawSphere(
                    transform.position + worldOffset,
                    attackPoint.radius);
            }
        }
#endif
    }
}

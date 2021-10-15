using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
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

        private bool isAttacking = false;
        private Vector3[] originalAttackPointPosition;

        private void FixedUpdate()
        {
            for (var i = 0; i < attackPoints.Length; i++)
            {
                AttackPoint ap = attackPoints[i];
                Vector3 currentWorldPos = ap.root.position +
                    ap.root.TransformVector(attackPoints[i].offset);
                Vector3 attackVector = currentWorldPos - originalAttackPointPosition[i];
                Ray ray = new Ray(currentWorldPos, attackVector);
                Debug.DrawRay(currentWorldPos, attackVector, Color.red, 4.0f);
            }
        }

        public void Attack()
        {
            isAttacking = true;
            originalAttackPointPosition = new Vector3[attackPoints.Length];
            for(var i = 0; i < attackPoints.Length; i++)
            {
                AttackPoint ap = attackPoints[i];
                originalAttackPointPosition[i] = 
                    ap.root.position +
                    ap.root.TransformVector(attackPoints[i].offset);
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

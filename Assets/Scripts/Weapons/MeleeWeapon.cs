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

        public void Attack()
        {
            Debug.Log("Melee weapon is swinging");
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

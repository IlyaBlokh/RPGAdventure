using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
    public partial class Damageable : MonoBehaviour
    {
        [SerializeField]
        float maxHP;

        [SerializeField][Range(0, 360f)]
        float hitAngle;

        public void ApplyDamage(DamageMessage data)
        {
            Debug.Log("Apply damage:" + data);
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
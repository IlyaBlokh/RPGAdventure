using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
    public class BanditBehaviour : MonoBehaviour
    {
        [SerializeField]
        float DetectionRange = 10.0f;
        [SerializeField]
        float DetectionAngle = 90.0f;

        private float m_DistanceToPlayer;
        private Vector3 m_LookAtPlayer;

        private void Update()
        {
            LookForPlayer();
        }

        private PlayerController LookForPlayer()
        {
            if (PlayerController.Instance == null)
            {
                return null;
            }

            m_LookAtPlayer = PlayerController.Instance.transform.position - transform.position;
            m_DistanceToPlayer = m_LookAtPlayer.magnitude;
            if (m_DistanceToPlayer <= DetectionRange)
            {
                if (Vector3.Dot(transform.forward, m_LookAtPlayer) > Mathf.Cos(Mathf.Deg2Rad * DetectionAngle / 2))
                {
                    return PlayerController.Instance;
                }
            }

            return null;
        }
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = new Color(0.8f, 0, 0, 0.4f);
            Vector3 m_RotatedForward = Quaternion.Euler(0, -DetectionAngle / 2, 0) * transform.forward;
            UnityEditor.Handles.DrawSolidArc(
                transform.position,
                Vector3.up,
                m_RotatedForward,
                DetectionAngle,
                DetectionRange);
        }
#endif
    }
}
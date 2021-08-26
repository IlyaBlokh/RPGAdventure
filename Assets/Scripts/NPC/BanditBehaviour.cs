using UnityEngine.AI;
using UnityEngine;

namespace RPGAdventure
{
    public class BanditBehaviour : MonoBehaviour
    {
        [SerializeField]
        float DetectionRange = 10.0f;
        [SerializeField]
        float DetectionAngle = 90.0f;
        [SerializeField]
        float TimeToStopPursuit = 2.0f;


        private NavMeshAgent m_NavMeshAgent;

        private float m_DistanceToPlayer;
        private Vector3 m_LookAtPlayer;
        private PlayerController m_Target;
        private float m_TimeNoDetecting;

        private void Awake()
        {
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            var target = LookForPlayer();
            if (m_Target == null)
            {
                if (target != null)
                {
                    m_Target = target;
                }
            }
            else
            {
                m_NavMeshAgent.SetDestination(m_Target.transform.position);
                if (target == null)
                {
                    m_TimeNoDetecting += Time.deltaTime;
                    if (m_TimeNoDetecting > TimeToStopPursuit)
                    {
                        m_Target = null;
                        m_TimeNoDetecting = .0f;
                    }
                }
            }
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
                if (Vector3.Dot(transform.forward, m_LookAtPlayer.normalized) > Mathf.Cos(Mathf.Deg2Rad * DetectionAngle / 2))
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
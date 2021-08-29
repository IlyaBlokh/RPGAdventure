using UnityEngine.AI;
using UnityEngine;
using System.Collections;

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
        [SerializeField]
        float TimeToReturnToSpotPos = 2.0f;

        //Components
        private EnemyController m_EnemyController;
        private Animator m_Animator;

        //AI
        private float m_DistanceToPlayer;
        private Vector3 m_LookAtPlayer;
        private Vector3 m_SpotPosition;
        private PlayerController m_MemorizedTarget;
        private float m_TimeNoDetecting;
        private Vector3 m_toBase;

        private readonly int m_HashedInPursuit = Animator.StringToHash("inPursuit");
        private readonly int m_HashedNearSpot = Animator.StringToHash("NearSpot");
        private readonly int m_HashedIsAwared = Animator.StringToHash("isAwared");

        private void Awake()
        {
            m_EnemyController = GetComponent<EnemyController>();
            m_Animator = GetComponent<Animator>();
            m_SpotPosition = transform.position;
        }

        private void Update()
        {
            var targetSpottedNow = LookForPlayer();
            if (m_MemorizedTarget == null)
            {
                if (targetSpottedNow != null)
                {
                    m_MemorizedTarget = targetSpottedNow;
                }
            }
            else
            {
                m_EnemyController.SetDestination(m_MemorizedTarget.transform.position);
                m_Animator.SetBool(m_HashedInPursuit, true);
                m_Animator.SetBool(m_HashedIsAwared, false);
                if (targetSpottedNow == null)
                {
                    m_TimeNoDetecting += Time.deltaTime;
                    if (m_TimeNoDetecting > TimeToStopPursuit)
                    {
                        m_MemorizedTarget = null;
                        m_Animator.SetBool(m_HashedIsAwared, true);
                        StartCoroutine(ReturnToSpotPosition());
                    }
                }
                else
                {
                    m_TimeNoDetecting = .0f;
                }
            }

            m_toBase = m_SpotPosition - transform.position;
            m_toBase.y = .0f;
            m_Animator.SetBool(m_HashedNearSpot, Mathf.Approximately(m_toBase.magnitude, .0f));
        }

        private IEnumerator ReturnToSpotPosition()
        {
            yield return new WaitForSeconds(TimeToReturnToSpotPos);
            m_EnemyController.SetDestination(m_SpotPosition);
            m_Animator.SetBool(m_HashedIsAwared, false);
            m_Animator.SetBool(m_HashedInPursuit, false);
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
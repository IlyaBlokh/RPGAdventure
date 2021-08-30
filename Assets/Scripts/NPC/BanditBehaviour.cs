using UnityEngine.AI;
using UnityEngine;
using System.Collections;

namespace RPGAdventure
{
    public class BanditBehaviour : MonoBehaviour
    {
        [SerializeField]
        float TimeToStopPursuit = 2.0f;
        [SerializeField]
        float TimeToReturnToSpotPos = 2.0f;
        [SerializeField]
        PlayerScanner m_PlayerScanner;

        //Components
        private EnemyController m_EnemyController;
        private Animator m_Animator;

        //AI
        private Vector3 m_SpotPosition;
        private PlayerController m_MemorizedTarget;
        private float m_TimeNoDetecting;
        private Vector3 m_toBase;

        private readonly int m_HashedInPursuit = Animator.StringToHash("inPursuit");
        private readonly int m_HashedNearSpot = Animator.StringToHash("NearSpot");
        private readonly int m_HashedIsAwared = Animator.StringToHash("isAwared");
        private readonly int m_HashedAttack = Animator.StringToHash("Attack");

        private void Awake()
        {
            m_EnemyController = GetComponent<EnemyController>();
            m_Animator = GetComponent<Animator>();
            m_SpotPosition = transform.position;
        }

        private void Update()
        {
            var targetSpottedNow = m_PlayerScanner.Search(transform);
            if (m_MemorizedTarget == null)
            {
                if (targetSpottedNow != null)
                {
                    m_MemorizedTarget = targetSpottedNow;
                }
            }
            else
            {

                if((m_MemorizedTarget.transform.position - transform.position).magnitude <= m_EnemyController.NavMeshAgent.stoppingDistance)
                {
                    m_EnemyController.DisableNavMeshAgent();
                    m_Animator.SetTrigger(m_HashedAttack);
                }
                else
                {
                    m_EnemyController.SetDestination(m_MemorizedTarget.transform.position);
                    m_Animator.SetBool(m_HashedInPursuit, true);
                    m_Animator.SetBool(m_HashedIsAwared, false);
                }

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


#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = new Color(0.8f, 0, 0, 0.4f);
            Vector3 m_RotatedForward = Quaternion.Euler(0, -m_PlayerScanner.DetectionAngle / 2, 0) * transform.forward;
            UnityEditor.Handles.DrawSolidArc(
                transform.position,
                Vector3.up,
                m_RotatedForward,
                m_PlayerScanner.DetectionAngle,
                m_PlayerScanner.DetectionRange);
        }
#endif
    }
}
using UnityEngine;
using UnityEngine.AI;

namespace NPC
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField]
        float SpeedModifyer = 0.7f;

        //Components
        private NavMeshAgent m_NavMeshAgent;
        private Animator m_Animator;

        public NavMeshAgent NavMeshAgent { get => m_NavMeshAgent; set => m_NavMeshAgent = value; }
        public Animator Animator { get => m_Animator; private set => m_Animator = value; }

        private void Awake()
        {
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
            Animator = GetComponent<Animator>();
        }

        private void OnAnimatorMove()
        {
            if (m_NavMeshAgent.enabled)
            {
                m_NavMeshAgent.speed = (Animator.deltaPosition / Time.fixedDeltaTime).magnitude;
                m_NavMeshAgent.speed *= SpeedModifyer;
            }
        }

        public bool SetDestination(Vector3 destination)
        {
            if (!m_NavMeshAgent.enabled) m_NavMeshAgent.enabled = true;
            return m_NavMeshAgent.SetDestination(destination);
        }

        public void DisableNavMeshAgent()
        {
            m_NavMeshAgent.enabled = false;
        }
    }
}

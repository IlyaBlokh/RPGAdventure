using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPGAdventure
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField]
        float SpeedModifyer = 0.7f;

        //Components
        private NavMeshAgent m_NavMeshAgent;
        private Animator m_Animator;

        private void Awake()
        {
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
            m_Animator = GetComponent<Animator>();
        }

        private void OnAnimatorMove()
        {
            if (m_NavMeshAgent == null) return;

            m_NavMeshAgent.speed = (m_Animator.deltaPosition / Time.fixedDeltaTime).magnitude;
            m_NavMeshAgent.speed *= SpeedModifyer;
        }

        public bool SetDestination(Vector3 destination)
        {
            return m_NavMeshAgent.SetDestination(destination);
        }
    }
}

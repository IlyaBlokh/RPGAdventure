using UnityEngine;
using UnityEngine.AI;

namespace NPC
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private float SpeedModifyer = 0.7f;

        //Components
        private NavMeshAgent navMeshAgent;
        public Animator Animator { get; private set; }

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            Animator = GetComponent<Animator>();
        }

        private void OnAnimatorMove()
        {
            if (navMeshAgent.enabled)
            {
                navMeshAgent.speed = (Animator.deltaPosition / Time.fixedDeltaTime).magnitude;
                navMeshAgent.speed *= SpeedModifyer;
            }
        }

        public bool SetDestination(Vector3 destination)
        {
            if (!navMeshAgent.enabled) navMeshAgent.enabled = true;
            return navMeshAgent.SetDestination(destination);
        }

        public void DisableNavMeshAgent()
        {
            navMeshAgent.enabled = false;
        }
    }
}

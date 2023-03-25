using System.Collections;
using DamageSystem;
using Events;
using Player;
using UnityEngine;
using Utils;
using Weapons;
using Zenject;

namespace NPC.Bandit
{
    [RequireComponent(typeof(EnemyController))]
    public class BanditBehaviour : MonoBehaviour, IAttackAnimListener, IMessageReceiver
    {
        [SerializeField] private float TimeToStopPursuit = 2.0f;
        [SerializeField] private float TimeToReturnToSpotPos = 2.0f;
        [SerializeField] private PlayerScanner m_PlayerScanner;
        [SerializeField] private float AttackDistance = 1.1f;

        //Components
        [SerializeField] private MeleeWeapon MeleeWeapon;

        private EnemyController enemyController;

        //AI
        private Vector3 spotPosition;
        private PlayerController followTarget;
        private float timeNoDetecting;
        private Vector3 toBase;
        private Vector3 toTarget;
        private Quaternion initialRotation;
        private PlayerController playerController;

        private bool HasFollowTarget => followTarget != null;

        private readonly int hashedInPursuit = Animator.StringToHash("inPursuit");
        private readonly int hashedNearSpot = Animator.StringToHash("NearSpot");
        private readonly int hashedIsAware = Animator.StringToHash("isAware");
        private readonly int hashedAttack = Animator.StringToHash("Attack");
        private readonly int hashedHurt = Animator.StringToHash("Hurt");
        private readonly int hashedDead = Animator.StringToHash("Dead");

        [Inject]
        private void Construct(PlayerController playerController)
        {
            this.playerController = playerController;
        }
        
        private void Awake()
        {
            enemyController = GetComponent<EnemyController>();
            spotPosition = transform.position;
            initialRotation = transform.rotation;
            MeleeWeapon.Owner = gameObject;
        }

        private void Update()
        {
            if (playerController.IsRespawning)
                StopPursuit(true);
            else
                GuardPosition();
        }

        private void GuardPosition()
        {
            PlayerController detectedTarget = m_PlayerScanner.Search(transform);
            bool hasDetectedTarget = detectedTarget != null;

            if (hasDetectedTarget) followTarget = detectedTarget;

            if (HasFollowTarget)
            {
                AttackOrPursuit();

                if (hasDetectedTarget)
                    timeNoDetecting = .0f;
                else
                    StopPursuit(false);
            }

            CheckOnSpotPosition();
        }

        private void AttackOrPursuit()
        {
            toTarget = followTarget.transform.position - transform.position;
            if (toTarget.magnitude <= AttackDistance)
                Attack();
            else
                Pursuit();
        }

        private void Attack()
        {
            Quaternion targetRotation = Quaternion.LookRotation(toTarget);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360 * Time.deltaTime);
            enemyController.DisableNavMeshAgent();
            enemyController.Animator.SetTrigger(hashedAttack);
        }

        private void Pursuit()
        {
            enemyController.SetDestination(followTarget.transform.position);
            enemyController.Animator.SetBool(hashedInPursuit, true);
            enemyController.Animator.SetBool(hashedIsAware, false);
        }

        private void StopPursuit(bool stopImmediately)
        {
            timeNoDetecting += Time.deltaTime;
            if (stopImmediately || timeNoDetecting > TimeToStopPursuit)
            {                
                followTarget = null;
                enemyController.Animator.SetBool(hashedIsAware, true);
                if (stopImmediately)
                {
                    enemyController.Animator.SetBool(hashedIsAware, false);
                    enemyController.Animator.SetBool(hashedInPursuit, false);
                }
                StartCoroutine(ReturnToSpotPosition());
            }
        }

        private IEnumerator ReturnToSpotPosition()
        {
            yield return new WaitForSeconds(TimeToReturnToSpotPos);
            enemyController.SetDestination(spotPosition);
            enemyController.Animator.SetBool(hashedIsAware, false);
            enemyController.Animator.SetBool(hashedInPursuit, false);
        }

        private void CheckOnSpotPosition()
        {
            toBase = spotPosition - transform.position;
            toBase.y = .0f;
            bool isOnSpot = Mathf.Approximately(toBase.magnitude, .0f);
            enemyController.Animator.SetBool(hashedNearSpot, isOnSpot);
            if (isOnSpot) 
                transform.rotation = Quaternion.RotateTowards(transform.rotation, initialRotation, 360 * Time.deltaTime);
        }

        public void AE_Attack(int attackStatus)
        {
            if (MeleeWeapon == null) 
                Debug.LogError ("MeleeWeapon NULL");
            else
                MeleeWeapon.UpdateAttack(attackStatus == 1);
        }

        public void OnMessageReceive(IMessageReceiver.MessageType messageType, object messageData)
        {
            switch (messageType)
            {
                case IMessageReceiver.MessageType.Damaged:
                    OnDamageReceived();
                    break;
                case IMessageReceiver.MessageType.Dead:
                    OnDead();
                    break;
            }
        }

        private void OnDamageReceived() => 
            enemyController.Animator.SetTrigger(hashedHurt);

        private void OnDead() => 
            enemyController.Animator.SetTrigger(hashedDead);

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = new Color(0.8f, 0, 0, 0.4f);
            Vector3 rotatedForward = Quaternion.Euler(0, -m_PlayerScanner.ScannerDetectionAngle / 2, 0) * transform.forward;
            UnityEditor.Handles.DrawSolidArc(
                transform.position,
                Vector3.up,
                rotatedForward,
                m_PlayerScanner.ScannerDetectionAngle,
                m_PlayerScanner.ScannerDetectionRange);
            UnityEditor.Handles.DrawSolidArc(
                transform.position,
                Vector3.up,
                rotatedForward,
                360,
                m_PlayerScanner.ScannerMeleeDetectionRange);
        }
#endif
    }
}
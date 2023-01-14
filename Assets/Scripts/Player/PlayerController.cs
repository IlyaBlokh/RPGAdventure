using Audio;
using Camera;
using DamageSystem;
using Events;
using UnityEngine;
using Utils;
using Weapons;

namespace Player
{
    [RequireComponent(typeof(Inventory.Inventory))]
    public class PlayerController : MonoBehaviour, IAttackAnimListener, IMessageReceiver
    {
        public static PlayerController Instance
        {
            get { return s_Instance; }
        }

        public bool IsRespawning { get => b_IsRespawning; set => b_IsRespawning = value; }

        [SerializeField]
        float MaxMovementSpeed = 12.0f;

        [SerializeField]
        float MinRotationSpeed = 400.0f;

        [SerializeField]
        float MaxRotationSpeed = 1200.0f;

        [SerializeField]
        float Gravity = 10.0f;

        [SerializeField]
        Vector3 SpawnPosition;

        [SerializeField]
        MeleeWeapon MeleeWeapon;

        [SerializeField]
        Transform PrimaryAttackHand;

        [SerializeField]
        RandomAudioPlayer FootfallAudioPlayer;

        private static PlayerController s_Instance;
        //Components
        private CharacterController m_CharController;
        private CameraController m_CameraController;
        private PlayerInput m_PlayerInput;
        private Animator m_Animator;
        private Damageable m_Damageable;

        //Movement
        private float m_DesiredForwardSpeed;
        private float m_ForwardSpeed = .0f;
        private float m_VerticalSpeed = .0f;
        private readonly int m_HashedForwardSpeed = Animator.StringToHash("ForwardSpeed");
        private Vector3 m_MovementDirection;
        private bool b_IsRespawning = false;

        //Rotation
        private Quaternion m_MovementRotation;
        private Vector3 m_CameraDirection;
        private Quaternion m_TargetRotation;
        private float m_RotationSpeed;

        //Combat
        private readonly int m_HashedMeleeAttack= Animator.StringToHash("MeleeAttack");
        private readonly int m_HashedDeath = Animator.StringToHash("Death");

        //Animation
        private AnimatorStateInfo m_CurrentAnimatorState;
        private AnimatorStateInfo m_NextAnimatorState;
        private bool m_IsAnimatorTransitioning;
        private readonly int m_HashedBlockInput = Animator.StringToHash("BlockInput");


        const float k_Acceleration = 20.0f;
        const float k_Deceleration = 40.0f;
        private void Awake()
        {
            m_CharController = GetComponent<CharacterController>();
            m_PlayerInput = GetComponent<PlayerInput>();
            m_Animator = GetComponent<Animator>();
            m_Damageable = GetComponent<Damageable>();
            m_CameraController = UnityEngine.Camera.main.GetComponent<CameraController>();
            if (MeleeWeapon != null)
                MeleeWeapon.Owner = gameObject;
            SpawnPosition = transform.position;
            s_Instance = this;
        }

        private void FixedUpdate()
        {
            CacheAnimatorState();
            UpdateInputBlocking();
            ComputeForwardMovement();
            ComputeVerticalMovement();
            ComputeRotation();
            Combat();
        }

        private void ComputeForwardMovement()
        {
            m_DesiredForwardSpeed = m_PlayerInput.MoveInput.magnitude * MaxMovementSpeed;
            m_ForwardSpeed = Mathf.MoveTowards(
                m_ForwardSpeed, 
                m_DesiredForwardSpeed, 
                (m_PlayerInput.IsMoving? k_Acceleration : k_Deceleration) * Time.fixedDeltaTime);
            m_Animator.SetFloat(m_HashedForwardSpeed, m_ForwardSpeed);
        }

        private void ComputeVerticalMovement()
        {
            m_VerticalSpeed = -Gravity;
        }

        private void OnAnimatorMove()
        {
            if (b_IsRespawning) return;
            m_MovementDirection = m_Animator.deltaPosition;
            m_MovementDirection += Vector3.up * m_VerticalSpeed * Time.fixedDeltaTime;
            m_CharController.Move(m_MovementDirection);
        }

        private void CacheAnimatorState()
        {
            m_CurrentAnimatorState = m_Animator.GetCurrentAnimatorStateInfo(0);
            m_NextAnimatorState = m_Animator.GetNextAnimatorStateInfo(0);
            m_IsAnimatorTransitioning = m_Animator.IsInTransition(0);
        }

        private void UpdateInputBlocking()
        {
            m_PlayerInput.IsPlayerControllerInputBlocked = 
                m_CurrentAnimatorState.tagHash == m_HashedBlockInput && !m_IsAnimatorTransitioning ||
                m_NextAnimatorState.tagHash == m_HashedBlockInput;
        }

        private void ComputeRotation()
        {
            m_MovementRotation = Quaternion.FromToRotation(Vector3.forward, m_PlayerInput.MoveInput);

            if (Mathf.Approximately(Vector3.Dot(Vector3.forward, m_PlayerInput.MoveInput), -1))
            {
                m_TargetRotation = Quaternion.LookRotation(-m_CameraDirection);
            }
            else
            {
                m_CameraDirection = Quaternion.Euler(0, m_CameraController.m_FreeLookCamera.m_XAxis.Value, 0) * Vector3.forward;
                m_TargetRotation = Quaternion.LookRotation(m_MovementRotation * m_CameraDirection);
            }

            if (m_PlayerInput.IsMoving)
            {
                m_RotationSpeed = Mathf.Lerp(MaxRotationSpeed, MinRotationSpeed , m_ForwardSpeed / m_DesiredForwardSpeed);
                m_TargetRotation = Quaternion.RotateTowards(
                    transform.rotation,
                    m_TargetRotation,
                    m_RotationSpeed * Time.fixedDeltaTime);
                transform.rotation = m_TargetRotation;
            }
        }
    
        public void AE_Attack (int AttackStatus)
        {
            MeleeWeapon?.UpdateAttack(AttackStatus == 1);
        }

        public void AE_Footfall()
        {
            FootfallAudioPlayer.PlayRandomClip();
        }

        private void Combat()
        {
            m_Animator.ResetTrigger(m_HashedMeleeAttack);
            if (m_PlayerInput.IsAttacking)
            {
                m_Animator.SetTrigger(m_HashedMeleeAttack);
            }
        }

        public void Respawn()
        {
            transform.position = SpawnPosition;
        }

        public void EndRespawn()
        {
            b_IsRespawning = false;
            m_Damageable.ResetHealth();
        }

        public void OnMessageReceive(IMessageReceiver.MessageType messageType, object messageData)
        {
            switch (messageType)
            {
                case IMessageReceiver.MessageType.DAMAGED:
                    break;
                case IMessageReceiver.MessageType.DEAD:
                    m_Animator.SetTrigger(m_HashedDeath);
                    break;
                default:
                    break;
            }
        }

        public void EquipItem(GameObject itemToEquip)
        {
            if (itemToEquip.name == MeleeWeapon?.gameObject.name) return;
            Destroy(MeleeWeapon?.gameObject);
            //TODO: disable player attack if disarmed
            //TODO: remove previous item
            var item = Instantiate(itemToEquip);
            MeleeWeapon = item.GetComponent<MeleeWeapon>();
            if (MeleeWeapon)
            {
                MeleeWeapon.Owner = gameObject;
                MeleeWeapon.GetComponent<FixedUpdateFollow>().FollowParent(PrimaryAttackHand);
            }
        }
    }
}
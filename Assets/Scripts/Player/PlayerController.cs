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
        public static PlayerController Instance { get; private set; }

        public bool IsRespawning { get; set; }

        [SerializeField] private float MaxMovementSpeed = 12.0f;
        [SerializeField] private float MinRotationSpeed = 400.0f;
        [SerializeField] private float MaxRotationSpeed = 1200.0f;
        [SerializeField] private float Gravity = 10.0f;
        [SerializeField] private Vector3 SpawnPosition;
        [SerializeField] private MeleeWeapon MeleeWeapon;
        [SerializeField] private Transform PrimaryAttackHand;
        [SerializeField] private RandomAudioPlayer FootfallAudioPlayer;

        //Components
        private CharacterController charController;
        private CameraController cameraController;
        private PlayerInput playerInput;
        private Animator animator;
        private Damageable damageable;

        //Movement
        private float desiredForwardSpeed;
        private float forwardSpeed = .0f;
        private float verticalSpeed = .0f;
        private readonly int hashedForwardSpeed = Animator.StringToHash("ForwardSpeed");
        private Vector3 movementDirection;

        //Rotation
        private Quaternion movementRotation;
        private Vector3 cameraDirection;
        private Quaternion targetRotation;
        private float rotationSpeed;

        //Combat
        private readonly int hashedMeleeAttack= Animator.StringToHash("MeleeAttack");
        private readonly int hashedDeath = Animator.StringToHash("Death");

        //Animation
        private AnimatorStateInfo currentAnimatorState;
        private AnimatorStateInfo nextAnimatorState;
        private bool isAnimatorTransitioning;
        private readonly int hashedBlockInput = Animator.StringToHash("BlockInput");


        private const float Acceleration = 20.0f;
        private const float Deceleration = 40.0f;
        private void Awake()
        {
            charController = GetComponent<CharacterController>();
            playerInput = GetComponent<PlayerInput>();
            animator = GetComponent<Animator>();
            damageable = GetComponent<Damageable>();
            cameraController = UnityEngine.Camera.main.GetComponent<CameraController>();
            if (MeleeWeapon != null)
                MeleeWeapon.Owner = gameObject;
            SpawnPosition = transform.position;
            Instance = this;
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
            desiredForwardSpeed = playerInput.MoveInput.magnitude * MaxMovementSpeed;
            forwardSpeed = Mathf.MoveTowards(
                forwardSpeed, 
                desiredForwardSpeed, 
                (playerInput.IsMoving? Acceleration : Deceleration) * Time.fixedDeltaTime);
            animator.SetFloat(hashedForwardSpeed, forwardSpeed);
        }

        private void ComputeVerticalMovement()
        {
            verticalSpeed = -Gravity;
        }

        private void OnAnimatorMove()
        {
            if (IsRespawning) return;
            movementDirection = animator.deltaPosition;
            movementDirection += Vector3.up * verticalSpeed * Time.fixedDeltaTime;
            charController.Move(movementDirection);
        }

        private void CacheAnimatorState()
        {
            currentAnimatorState = animator.GetCurrentAnimatorStateInfo(0);
            nextAnimatorState = animator.GetNextAnimatorStateInfo(0);
            isAnimatorTransitioning = animator.IsInTransition(0);
        }

        private void UpdateInputBlocking()
        {
            playerInput.IsPlayerControllerInputBlocked = 
                currentAnimatorState.tagHash == hashedBlockInput && !isAnimatorTransitioning ||
                nextAnimatorState.tagHash == hashedBlockInput;
        }

        private void ComputeRotation()
        {
            movementRotation = Quaternion.FromToRotation(Vector3.forward, playerInput.MoveInput);

            if (Mathf.Approximately(Vector3.Dot(Vector3.forward, playerInput.MoveInput), -1))
            {
                targetRotation = Quaternion.LookRotation(-cameraDirection);
            }
            else
            {
                cameraDirection = Quaternion.Euler(0, cameraController.FreeLookCamera.m_XAxis.Value, 0) * Vector3.forward;
                targetRotation = Quaternion.LookRotation(movementRotation * cameraDirection);
            }

            if (playerInput.IsMoving)
            {
                rotationSpeed = Mathf.Lerp(MaxRotationSpeed, MinRotationSpeed , forwardSpeed / desiredForwardSpeed);
                targetRotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.fixedDeltaTime);
                transform.rotation = targetRotation;
            }
        }
    
        public void AE_Attack (int AttackStatus)
        {
            if (MeleeWeapon != null)
                MeleeWeapon.UpdateAttack(AttackStatus == 1);
        }

        public void AE_Footfall()
        {
            FootfallAudioPlayer.PlayRandomClip();
        }

        private void Combat()
        {
            animator.ResetTrigger(hashedMeleeAttack);
            if (playerInput.IsAttacking) 
                animator.SetTrigger(hashedMeleeAttack);
        }

        public void Respawn()
        {
            transform.position = SpawnPosition;
        }

        public void EndRespawn()
        {
            IsRespawning = false;
            damageable.ResetHealth();
        }

        public void OnMessageReceive(IMessageReceiver.MessageType messageType, object messageData)
        {
            switch (messageType)
            {
                case IMessageReceiver.MessageType.Dead:
                    animator.SetTrigger(hashedDeath);
                    break;
            }
        }

        public void EquipItem(GameObject itemToEquip)
        {
            if (MeleeWeapon == null) return;
            if (itemToEquip.name == MeleeWeapon.gameObject.name) return;
            Destroy(MeleeWeapon.gameObject);
            //TODO: disable player attack if disarmed
            //TODO: remove previous item
            GameObject item = Instantiate(itemToEquip);
            MeleeWeapon = item.GetComponent<MeleeWeapon>();
            if (MeleeWeapon)
            {
                MeleeWeapon.Owner = gameObject;
                MeleeWeapon.GetComponent<FixedUpdateFollow>().FollowParent(PrimaryAttackHand);
            }
        }
    }
}
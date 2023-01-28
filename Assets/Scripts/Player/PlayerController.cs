using System;
using Audio;
using Camera;
using Cinemachine;
using DamageSystem;
using Events;
using UnityEngine;
using Utils;
using Weapons;
using Zenject;

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
        private InputController inputController;
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
        private const float RotationSpeed = 50f;

        [Inject]
        private void Construct(CameraController cameraController)
        {
            this.cameraController = cameraController;
        }
        
        private void Awake()
        {
            charController = GetComponent<CharacterController>();
            inputController = GetComponent<InputController>();
            animator = GetComponent<Animator>();
            damageable = GetComponent<Damageable>();
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

        private void CacheAnimatorState()
        {
            currentAnimatorState = animator.GetCurrentAnimatorStateInfo(0);
            nextAnimatorState = animator.GetNextAnimatorStateInfo(0);
            isAnimatorTransitioning = animator.IsInTransition(0);
        }

        private void UpdateInputBlocking()
        {
            inputController.IsPlayerControllerInputBlocked = 
                currentAnimatorState.tagHash == hashedBlockInput && !isAnimatorTransitioning ||
                nextAnimatorState.tagHash == hashedBlockInput;
        }

        private void ComputeForwardMovement()
        {
            desiredForwardSpeed = inputController.MoveInput.magnitude * MaxMovementSpeed;
            forwardSpeed = Mathf.MoveTowards(
                forwardSpeed, 
                desiredForwardSpeed, 
                (inputController.IsMoving? Acceleration : Deceleration) * Time.fixedDeltaTime);
            animator.SetFloat(hashedForwardSpeed, forwardSpeed);
        }

        private void ComputeVerticalMovement() => 
            verticalSpeed = -Gravity;

        private void ComputeRotation()
        {
            float targetRotation = cameraController.RawOrientation.eulerAngles.y;
            transform.eulerAngles = Vector3.Lerp(
                transform.eulerAngles, 
                new Vector3(transform.eulerAngles.x, targetRotation, transform.eulerAngles.z), 
                RotationSpeed * Time.fixedDeltaTime);
        }

        private void OnAnimatorMove()
        {
            if (IsRespawning) return;
            movementDirection = animator.deltaPosition;
            movementDirection += Vector3.up * verticalSpeed * Time.fixedDeltaTime;
            charController.Move(movementDirection);
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
            if (inputController.IsAttacking) 
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
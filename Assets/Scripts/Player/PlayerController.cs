using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
    public class PlayerController : MonoBehaviour, IAttackAnimListener
    {
        public static PlayerController Instance
        {
            get { return s_Instance; }
        }

        [SerializeField]
        float MaxMovementSpeed = 12.0f;

        [SerializeField]
        float MinRotationSpeed = 400.0f;

        [SerializeField]
        float MaxRotationSpeed = 1200.0f;

        [SerializeField]
        float Gravity = 10.0f;

        [SerializeField]
        MeleeWeapon MeleeWeapon;


        private static PlayerController s_Instance;
        //Components
        private CharacterController m_CharController;
        private CameraController m_CameraController;
        private PlayerInput m_PlayerInput;
        private Animator m_Animator;

        //Movement
        private float m_DesiredForwardSpeed;
        private float m_ForwardSpeed = .0f;
        private float m_VerticalSpeed = .0f;
        private readonly int m_HashedForwardSpeed = Animator.StringToHash("ForwardSpeed");
        private Vector3 m_MovementDirection;

        //Rotation
        private Quaternion m_MovementRotation;
        private Vector3 m_CameraDirection;
        private Quaternion m_TargetRotation;
        private float m_RotationSpeed;

        //Combat
        private readonly int m_HashedMeleeAttack= Animator.StringToHash("MeleeAttack");


        const float k_Acceleration = 20.0f;
        const float k_Deceleration = 40.0f;
        private void Awake()
        {
            m_CharController = GetComponent<CharacterController>();
            m_PlayerInput = GetComponent<PlayerInput>();
            m_Animator = GetComponent<Animator>();
            m_CameraController = Camera.main.GetComponent<CameraController>();
            MeleeWeapon.Owner = gameObject;
            s_Instance = this;
        }

        private void FixedUpdate()
        {
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
            m_MovementDirection = m_Animator.deltaPosition;
            m_MovementDirection += Vector3.up * m_VerticalSpeed * Time.fixedDeltaTime;
            m_CharController.Move(m_MovementDirection);
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
            MeleeWeapon.UpdateAttack(AttackStatus == 1);
        }

        private void Combat()
        {
            m_Animator.ResetTrigger(m_HashedMeleeAttack);
            if (m_PlayerInput.IsAttacking)
            {
                m_Animator.SetTrigger(m_HashedMeleeAttack);
            }
        }

    }
}
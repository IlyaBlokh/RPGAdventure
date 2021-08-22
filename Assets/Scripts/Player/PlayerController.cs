using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
    public class PlayerController : MonoBehaviour
    {
        const float k_Acceleration = 20.0f;
        const float k_Deceleration = 40.0f;

        [SerializeField]
        float MaxMovementSpeed = 8.0f;

        [SerializeField]
        float MinRotationSpeed = 400.0f;

        [SerializeField]
        float MaxRotationSpeed = 1200.0f;

        //Components
        private CharacterController m_CharController;
        private CameraController m_CameraController;
        private PlayerInput m_PlayerInput;
        private Animator m_Animator;

        //Movement
        private float m_DesiredForwardSpeed;
        private float m_ForwardSpeed = .0f;
        private readonly int m_HashedForwardSpeed = Animator.StringToHash("ForwardSpeed");

        //Rotation
        private Quaternion m_MovementRotation;
        private Vector3 m_CameraDirection;
        private Quaternion m_TargetRotation;
        private float m_RotationSpeed;

        private void Awake()
        {
            m_CharController = GetComponent<CharacterController>();
            m_PlayerInput = GetComponent<PlayerInput>();
            m_Animator = GetComponent<Animator>();
            m_CameraController = Camera.main.GetComponent<CameraController>();
        }

        private void FixedUpdate()
        {
            ComputeMovement();
            ComputeRotation();
        }

        private void ComputeMovement()
        {
            m_DesiredForwardSpeed = m_PlayerInput.MoveInput.magnitude * MaxMovementSpeed;
            m_ForwardSpeed = Mathf.MoveTowards(
                m_ForwardSpeed, 
                m_DesiredForwardSpeed, 
                (m_PlayerInput.IsMoving? k_Acceleration : k_Deceleration) * Time.fixedDeltaTime);
            m_Animator.SetFloat(m_HashedForwardSpeed, m_ForwardSpeed);
        }

        private void OnAnimatorMove()
        {
            m_CharController.Move(m_Animator.deltaPosition);
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
    }
}
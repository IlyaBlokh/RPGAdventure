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

        private void Awake()
        {
            m_CharController = GetComponent<CharacterController>();
            m_PlayerInput = GetComponent<PlayerInput>();
            m_Animator = GetComponent<Animator>();
            m_CameraController = GetComponent<CameraController>();
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

        private void ComputeRotation()
        {
            m_MovementRotation = Quaternion.FromToRotation(Vector3.forward, m_PlayerInput.MoveInput);
            m_CameraDirection = Quaternion.Euler(0, m_CameraController.m_FreeLookCamera.m_XAxis.Value, 0) * Vector3.forward;
            m_TargetRotation = Quaternion.LookRotation(m_MovementRotation * m_CameraDirection);
            if (m_PlayerInput.IsMoving)
            {
                transform.rotation = m_TargetRotation;
            }
        }
    }
}
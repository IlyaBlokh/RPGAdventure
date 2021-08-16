using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        float MovementSpeed;

        [SerializeField]
        float RotationSpeed;

        //GameObject
        private Rigidbody m_Rb;

        //Movement
        private float m_HorizonalInput;
        private float m_VerticalInput;
        private Vector3 m_MovementDirection;
        private Vector3 m_CameraDirection;
        private Vector3 m_TargetDirection;

        private void Awake()
        {
            m_Rb = GetComponent<Rigidbody>();
        }

        void Start()
        {
        }

        void FixedUpdate()
        {
            m_HorizonalInput = Input.GetAxis("Horizontal");
            m_VerticalInput = Input.GetAxis("Vertical");
            m_MovementDirection = new Vector3(m_HorizonalInput, 0, m_VerticalInput);

            /*if (m_MovementDirection.Equals(Vector3.zero))
                return;

            m_CameraDirection = m_FollowingCamera.transform.rotation * m_MovementDirection;
            m_TargetDirection = m_CameraDirection;
            m_TargetDirection.y = .0f;

            if (m_MovementDirection.z >= 0)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(m_TargetDirection),
                    0.1f);
            }

            m_Rb.MovePosition(m_Rb.position + m_TargetDirection.normalized * MovementSpeed * Time.fixedDeltaTime);*/
            m_Rb.MovePosition(m_Rb.position + m_MovementDirection.normalized * MovementSpeed * Time.fixedDeltaTime);
        }
    }
}
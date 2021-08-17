using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        float MovementSpeed;

        //GameObject
        private Rigidbody m_Rb;
        private Camera m_MainCamera;

        //Movement
        private float m_HorizonalInput;
        private float m_VerticalInput;
        private Vector3 m_MovementDirection;
        private Vector3 m_targetDirecton;
        private Quaternion m_CameraRotation;

        private void Awake()
        {
            m_Rb = GetComponent<Rigidbody>();
            m_MainCamera = Camera.main;
        }

        void FixedUpdate()
        {
            m_HorizonalInput = Input.GetAxis("Horizontal");
            m_VerticalInput = Input.GetAxis("Vertical");
            m_MovementDirection.Set(m_HorizonalInput, 0, m_VerticalInput);

            m_CameraRotation = m_MainCamera.transform.rotation;
            m_targetDirecton = m_CameraRotation * m_MovementDirection;
            m_targetDirecton.y = 0;
            m_targetDirecton.Normalize();

            m_Rb.MoveRotation(Quaternion.Euler(0, m_CameraRotation.eulerAngles.y, 0));
            m_Rb.MovePosition(m_Rb.position + m_targetDirecton * MovementSpeed * Time.fixedDeltaTime);
        }
    }
}
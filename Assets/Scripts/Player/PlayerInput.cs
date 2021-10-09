using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
    public class PlayerInput : MonoBehaviour
    {
        private Vector3 m_PlayerInput;
        private bool m_IsAttacking;

        public Vector3 MoveInput
        {
            get { return m_PlayerInput.normalized; }
        }

        public bool IsAttacking
        {
            get { return m_IsAttacking; }
        }

        public bool IsMoving
        {
            get { return !Mathf.Approximately(m_PlayerInput.magnitude, 0); }
        }

        void Update()
        {
            m_PlayerInput.Set(
                Input.GetAxis("Horizontal"),
                0,
                Input.GetAxis("Vertical"));

            if (Input.GetButtonDown("Fire1") && !m_IsAttacking)
            {
                StartCoroutine(AttackAndWait());
            }
        }

        private IEnumerator AttackAndWait()
        {
            m_IsAttacking = true;
            yield return new WaitForSeconds(0.03f);
            m_IsAttacking = false;
        }
    }
}
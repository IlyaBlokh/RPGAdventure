using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
    public class PlayerInput : MonoBehaviour
    {
        private Vector3 m_PlayerInput;
        private bool m_IsAttacking;
        private bool m_IsInteracting;

        public Vector3 MoveInput { get => m_PlayerInput.normalized; }

        public bool IsAttacking { get => m_IsAttacking; }

        public bool IsMoving{ get => !Mathf.Approximately(m_PlayerInput.magnitude, 0); }

        public bool IsInteracking { get => m_IsInteracting; }

        void Update()
        {
            m_PlayerInput.Set(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            if (Input.GetButtonDown("Fire1"))
                HandlePrimaryAction();

            if (Input.GetButtonDown("Fire2"))
                HandleSecondaryAction();
        }

        private void HandlePrimaryAction()
        {
            if (!m_IsAttacking)
                StartCoroutine(AttackAndWait());
        }

        private void HandleSecondaryAction()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool hasHit = Physics.Raycast(ray, out RaycastHit hitInfo);
            var clickableObject = hitInfo.collider.GetComponent<Clickable>();
            if (hasHit && clickableObject)
            {
                m_IsInteracting = clickableObject.HandleClick();
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
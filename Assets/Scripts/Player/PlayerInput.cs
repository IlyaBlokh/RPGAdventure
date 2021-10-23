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

            bool isPrimaryActionTriggered = Input.GetButtonDown("Fire1");
            bool issecondaryActionTriggered = Input.GetButtonDown("Fire2");

            if (isPrimaryActionTriggered && !m_IsAttacking)
            {
                StartCoroutine(AttackAndWait());
            }

            if (issecondaryActionTriggered)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                bool hasHit = Physics.Raycast(ray, out RaycastHit hitInfo);
                if (hasHit && hitInfo.collider.GetComponent<Clickable>())
                {
                    Debug.Log(hitInfo.collider.name);
                }
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
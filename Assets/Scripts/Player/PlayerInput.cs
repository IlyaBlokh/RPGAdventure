using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPGAdventure
{
    public class PlayerInput : MonoBehaviour
    {
        public static PlayerInput Instance { get => s_Instance; }

        private static PlayerInput s_Instance;
        private Vector3 m_PlayerInput;
        private bool m_IsAttacking;
        private bool m_IsPlayerControllerInputBlocked;
        private Clickable m_ClickableObject;

        public Vector3 MoveInput { get => m_PlayerInput.normalized; }

        public bool IsAttacking { get => m_IsAttacking; }

        public bool IsMoving{ get => !Mathf.Approximately(m_PlayerInput.magnitude, 0); }

        public Clickable GetClickableObject { get => m_ClickableObject; }
        public bool IsPlayerControllerInputBlocked { get => m_IsPlayerControllerInputBlocked; set => m_IsPlayerControllerInputBlocked = value; }

        private void Awake()
        {
            s_Instance = this;
        }

        void Update()
        {
            if (!IsPlayerControllerInputBlocked)
            {
                m_PlayerInput.Set(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

                if (Input.GetButtonDown("Fire1"))
                    HandlePrimaryAction();

                if (Input.GetButtonDown("Fire2"))
                    HandleSecondaryAction();
            }
        }

        private void HandlePrimaryAction()
        {
            if (!m_IsAttacking && !IsClickOverUI())
                StartCoroutine(TriggerAttack());
        }

        private bool IsClickOverUI()
        {
            var data = new PointerEventData(EventSystem.current) { 
                position = Input.mousePosition
            };
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(data, results);
            return results.Count > 0;
        }

        private void HandleSecondaryAction()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool hasHit = Physics.Raycast(ray, out RaycastHit hitInfo);
            var clickableObject = hitInfo.collider.GetComponent<Clickable>();
            if (hasHit && clickableObject)
            {
                StartCoroutine(TriggerInteract(clickableObject));
            }
        }
        private IEnumerator TriggerAttack()
        {
            m_IsAttacking = true;
            yield return new WaitForSeconds(0.03f);
            m_IsAttacking = false;
        }

        private IEnumerator TriggerInteract(Clickable clickableObject)
        {
            m_ClickableObject = clickableObject.CheckClickCondition();
            yield return new WaitForSeconds(0.03f);
            m_ClickableObject = null;
        }
    }
}
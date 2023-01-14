using System.Collections;
using System.Collections.Generic;
using Graphics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        public static PlayerInput Instance => instance;

        private static PlayerInput instance;
        private Vector3 playerInput;
        private GameObject lastTextUnderPointer;

        public Vector3 MoveInput => playerInput.normalized;
        public bool IsAttacking { get; private set; }
        public bool IsMoving => !Mathf.Approximately(playerInput.magnitude, 0);
        public Clickable GetClickableObject { get; private set; }
        public bool IsPlayerControllerInputBlocked { get; set; }

        private void Awake()
        {
            instance = this;
        }

        private void Update()
        {
            if (IsPlayerControllerInputBlocked) 
                return;
            playerInput.Set(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            if (Input.GetButtonDown("Fire1"))
                HandlePrimaryAction();

            if (Input.GetButtonDown("Fire2"))
                HandleSecondaryAction();

            if (lastTextUnderPointer != null)
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    var UIObject = GetUIObjectUnderPointer();
                    if (!AreTextsEqual(UIObject, lastTextUnderPointer))
                    {
                        SetUITextColor(new Color(.19f, .19f, .19f));
                    }
                }
                else
                {
                    SetUITextColor(new Color(.19f, .19f, .19f));
                }
            }

            if (EventSystem.current.IsPointerOverGameObject())
            {
                GameObject UIObject = GetUIObjectUnderPointer();
                if (UIObject.CompareTag("option_text"))
                {
                    lastTextUnderPointer = UIObject;
                    SetUITextColor(Color.yellow);
                }
            }
        }

        private bool AreTextsEqual(GameObject UIObject1, GameObject UIObject2)
        {
            var text1 = UIObject1.GetComponent<Text>();
            var text2 = UIObject2.GetComponent<Text>();
            if (text1 == null || text2 == null) return false;
            return text1.text.Equals(text2.text);
        }

        private void SetUITextColor(Color color)
        {
            lastTextUnderPointer.GetComponent<Text>().color = color;
        }

        private void HandlePrimaryAction()
        {
            if (!IsAttacking && GetUIObjectUnderPointer() == null)
                StartCoroutine(TriggerAttack());
        }

        private GameObject GetUIObjectUnderPointer()
        {
            var data = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(data, results);
            return results.Count > 0 ? results[0].gameObject : null;
        }


        private void HandleSecondaryAction()
        {
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
            bool hasHit = Physics.Raycast(ray, out RaycastHit hitInfo);
            var clickableObject = hitInfo.collider.GetComponent<Clickable>();
            if (hasHit && clickableObject)
            {
                StartCoroutine(TriggerInteract(clickableObject));
            }
        }
        private IEnumerator TriggerAttack()
        {
            IsAttacking = true;
            yield return new WaitForSeconds(0.03f);
            IsAttacking = false;
        }

        private IEnumerator TriggerInteract(Clickable clickableObject)
        {
            GetClickableObject = clickableObject.CheckClickCondition();
            yield return new WaitForSeconds(0.03f);
            GetClickableObject = null;
        }
    }
}
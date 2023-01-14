using System.Collections;
using System.Collections.Generic;
using Camera;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        public static PlayerInput Instance => instance;

        private static PlayerInput instance;
        private Vector3 playerInput;
        private GameObject lastTextUnderPointer;
        private CameraController cameraController;

        public Vector3 MoveInput => playerInput.normalized;
        public bool IsAttacking { get; private set; }
        public bool IsMoving => !Mathf.Approximately(playerInput.magnitude, 0);
        public bool IsPlayerControllerInputBlocked { get; set; }

        [Inject]
        private void Construct(CameraController cameraController)
        {
            this.cameraController = cameraController;
        }
        
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

            if (Mouse.current.rightButton.wasPressedThisFrame)
                cameraController.SwitchToFixedView();
            if (Mouse.current.rightButton.wasReleasedThisFrame)
                cameraController.SwitchToFreeLookView();

            if (lastTextUnderPointer != null)
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    GameObject UIObject = GetUIObjectUnderPointer();
                    if (!AreTextsEqual(UIObject, lastTextUnderPointer)) 
                        SetUITextColor(new Color(.19f, .19f, .19f));
                }
                else
                    SetUITextColor(new Color(.19f, .19f, .19f));
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

        private IEnumerator TriggerAttack()
        {
            IsAttacking = true;
            yield return new WaitForSeconds(0.03f);
            IsAttacking = false;
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
    }
}
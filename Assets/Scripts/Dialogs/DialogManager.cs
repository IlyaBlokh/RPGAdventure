using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPGAdventure
{
    public class DialogManager : MonoBehaviour
    {
        [SerializeField]
        GameObject DialogUI;
        [SerializeField]
        Text DialogHeaderText;

        private void Awake()
        {
            DialogUI.SetActive(false);
        }
        private void Update()
        {
            if (PlayerInput.Instance != null && PlayerInput.Instance.IsInteracking)
            {
                StartDialog();
            }
        }

        private void StartDialog()
        {
            DialogUI.SetActive(true);
            DialogHeaderText.text = "TEST";
        }
    }
}

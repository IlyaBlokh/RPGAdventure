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
        [SerializeField]
        Text DialogWelcomeText;

        private PlayerInput m_PlayerInput;
        private GameObject m_NPC;
        private Dialog m_ActiveDialog;

        private void Awake()
        {
            DialogUI.SetActive(false);
        }

        private void Start()
        {
            m_PlayerInput = PlayerInput.Instance;
        }

        private void Update()
        {
            if (!DialogUI.activeSelf && 
                m_PlayerInput.GetClickableObject)
            {
                m_NPC = m_PlayerInput.GetClickableObject.gameObject;
                StartDialog();
            }

            if (DialogUI.activeSelf &&
                m_NPC.GetComponent<Clickable>().CheckEndInteractCondition())
            {
                StopDialog();
            }
        }

        private void StartDialog()
        {
            DialogHeaderText.text = m_NPC.name;
            m_ActiveDialog = m_NPC.GetComponent<QuestOwner>().Dialog;
            DialogWelcomeText.text = m_ActiveDialog.welcomeText;
            DialogUI.SetActive(true);
        }

        private void StopDialog()
        {
            DialogUI.SetActive(false);
            m_NPC = null;
            m_ActiveDialog = null;
        }
    }
}

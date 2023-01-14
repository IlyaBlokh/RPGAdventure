using System;
using Graphics;
using Player;
using QuestSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Dialogs
{
    public class DialogManager : MonoBehaviour
    {
        [SerializeField]
        GameObject DialogUI;
        [SerializeField]
        Text DialogHeaderText;
        [SerializeField]
        Text DialogAnswerText;
        [SerializeField]
        GameObject QueryOptionsList;
        [SerializeField]
        Button QueryOptionPrefab;
        [SerializeField]
        float TimeToShowDialogOptions = 2.0f;

        private QuestManager m_QuestManager;
        private PlayerInput m_PlayerInput;
        private GameObject m_NPC;
        private Dialog m_ActiveDialog;
        private float m_OptionTopPosition;
        private float m_TimerForDialogOptions;
        private bool m_ForceDialogQuit;

        const float c_OptionIndend = 44.0f;

        private void Awake()
        {
            DialogUI.SetActive(false);
        }

        private void Start()
        {
            m_QuestManager = FindObjectOfType<QuestManager>();
            m_PlayerInput = PlayerInput.Instance;
            m_ForceDialogQuit = false;
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

            //Timer is triggered
            if (m_TimerForDialogOptions > 0)
            {
                m_TimerForDialogOptions += Time.deltaTime;
                if (m_TimerForDialogOptions >= TimeToShowDialogOptions)
                {
                    if (m_ForceDialogQuit)
                    {
                        StopDialog();
                    }
                    else
                    {
                        m_TimerForDialogOptions = .0f;
                        DisplayDialogOptions();
                    }
                }
            }
        }

        private void StartDialog()
        {
            DialogHeaderText.text = m_NPC.name;
            m_ActiveDialog = m_NPC.GetComponent<QuestOwner>().Dialog;
            DialogUI.SetActive(true);
            CleanupDialogOptionList();
            DisplayAnswerText(m_ActiveDialog.welcomeText);
            TriggerDisplayDialogOptions();
        }

        private void DisplayAnswerText(String answerText)
        {
            DialogAnswerText.gameObject.SetActive(true);
            DialogAnswerText.text = answerText;
        }

        private void TriggerDisplayDialogOptions()
        {
            m_TimerForDialogOptions = 0.001f;
        }

        private void DisplayDialogOptions()
        {
            HideAnswerText();
            InitDialogOptionsList();
        }

        private void HideAnswerText()
        {
            DialogAnswerText.gameObject.SetActive(false);
        }

        private void InitDialogOptionsList()
        {
            var queries = Array.FindAll(m_ActiveDialog.queries, query => !query.isAsked || query.shouldAlwaysAsk);
            foreach (var query in queries)
            {
                InitOptionInstance(query);
            }
        }

        private void CleanupDialogOptionList()
        {
            m_OptionTopPosition = .0f;
            foreach (Transform child in QueryOptionsList.transform)
                Destroy(child.gameObject);
        }

        private void InitOptionInstance(DialogQuery dialogOption)
        {
            m_OptionTopPosition += c_OptionIndend;
            var OptionButtonInstance = Instantiate(QueryOptionPrefab, QueryOptionsList.transform);
            OptionButtonInstance.GetComponentInChildren<Text>().text = dialogOption.queryText;
            OptionButtonInstance.GetComponentInChildren<Text>().tag = "option_text";
            RectTransform btnRectTransform = OptionButtonInstance.GetComponent<RectTransform>();
            btnRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, m_OptionTopPosition, btnRectTransform.rect.height);
            RegisterOptionClickHandler(OptionButtonInstance, dialogOption);
        }

        private void RegisterOptionClickHandler(Button optionBtn, DialogQuery query)
        {
            EventTrigger eventTrigger = optionBtn.gameObject.AddComponent<EventTrigger>();
            var clickDownEvent = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
            clickDownEvent.callback.AddListener((e) => {
                if (!String.IsNullOrEmpty(query.dialogAnswer.questId))
                {
                    m_PlayerInput.GetComponent<QuestLog>().AddQuest(m_QuestManager.GetQuest(query.dialogAnswer.questId));
                }
                m_ForceDialogQuit = query.dialogAnswer.shouldForceExit;
                query.isAsked = true;
                CleanupDialogOptionList();
                DisplayAnswerText(query.dialogAnswer.answerText);
                TriggerDisplayDialogOptions();
            });
            eventTrigger.triggers.Add(clickDownEvent);
        }

        private void StopDialog()
        {
            DialogUI.SetActive(false);
            m_NPC = null;
            m_ActiveDialog = null;
            m_ForceDialogQuit = false;
            m_TimerForDialogOptions = .0f;
        }
    }
}

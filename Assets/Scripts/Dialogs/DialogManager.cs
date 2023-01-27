using System;
using Graphics;
using Interaction;
using Player;
using QuestSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Dialogs
{
    public class DialogManager : MonoBehaviour
    {
        [SerializeField] private GameObject DialogUI;
        [SerializeField] private Text DialogHeaderText;
        [SerializeField] private Text DialogAnswerText;
        [SerializeField] private GameObject QueryOptionsList;
        [SerializeField] private Button QueryOptionPrefab;
        [SerializeField] private float TimeToShowDialogOptions = 2.0f;

        private QuestManager questManager;
        private InputController inputController;
        private GameObject npc;
        private Dialog activeDialog;
        private float optionTopPosition;
        private float timerForDialogOptions;
        private bool forceDialogQuit;

        private const float OptionIndend = 44.0f;

        private void Awake()
        {
            DialogUI.SetActive(false);
        }

        private void Start()
        {
            questManager = FindObjectOfType<QuestManager>();
            inputController = InputController.Instance;
            forceDialogQuit = false;
        }
        
        public void StartDialogWith(DialogInteractable dialogInteractable)
        {
            npc = dialogInteractable.gameObject;
            DialogHeaderText.text = npc.name;
            activeDialog = npc.GetComponent<QuestOwner>().Dialog;
            DialogUI.SetActive(true);
            CleanupDialogOptionList();
            DisplayAnswerText(activeDialog.welcomeText);
            TriggerDisplayDialogOptions();
        }

        public void StopDialog()
        {
            DialogUI.SetActive(false);
            npc = null;
            activeDialog = null;
            forceDialogQuit = false;
            timerForDialogOptions = .0f;
        }

        private void DisplayAnswerText(String answerText)
        {
            DialogAnswerText.gameObject.SetActive(true);
            DialogAnswerText.text = answerText;
        }

        private void TriggerDisplayDialogOptions()
        {
            timerForDialogOptions = 0.001f;
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
            DialogQuery[] queries = Array.FindAll(activeDialog.queries, query => !query.isAsked || query.shouldAlwaysAsk);
            foreach (DialogQuery query in queries)
            {
                InitOptionInstance(query);
            }
        }

        private void CleanupDialogOptionList()
        {
            optionTopPosition = .0f;
            foreach (Transform child in QueryOptionsList.transform)
                Destroy(child.gameObject);
        }

        private void InitOptionInstance(DialogQuery dialogOption)
        {
            optionTopPosition += OptionIndend;
            Button OptionButtonInstance = Instantiate(QueryOptionPrefab, QueryOptionsList.transform);
            OptionButtonInstance.GetComponentInChildren<Text>().text = dialogOption.queryText;
            OptionButtonInstance.GetComponentInChildren<Text>().tag = "option_text";
            RectTransform btnRectTransform = OptionButtonInstance.GetComponent<RectTransform>();
            btnRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, optionTopPosition, btnRectTransform.rect.height);
            RegisterOptionClickHandler(OptionButtonInstance, dialogOption);
        }

        private void RegisterOptionClickHandler(Button optionBtn, DialogQuery query)
        {
            EventTrigger eventTrigger = optionBtn.gameObject.AddComponent<EventTrigger>();
            var clickDownEvent = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
            clickDownEvent.callback.AddListener((e) => {
                if (!string.IsNullOrEmpty(query.dialogAnswer.questId))
                {
                    inputController.GetComponent<QuestLog>().AddQuest(questManager.GetQuest(query.dialogAnswer.questId));
                }
                forceDialogQuit = query.dialogAnswer.shouldForceExit;
                query.isAsked = true;
                CleanupDialogOptionList();
                DisplayAnswerText(query.dialogAnswer.answerText);
                TriggerDisplayDialogOptions();
            });
            eventTrigger.triggers.Add(clickDownEvent);
        }
    }
}

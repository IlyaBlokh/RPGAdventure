using System.Collections.Generic;
using Core;
using Dialogs;
using UnityEngine;

namespace QuestSystem
{
    public class QuestOwner : MonoBehaviour
    {
        [SerializeField] private List<Quest> Quests;
        [SerializeField] private Dialog dialog;

        public Dialog Dialog => dialog;

        private void Start()
        {
            var questManager = FindObjectOfType<QuestManager>();
            string uid = GetComponent<UniqueID>().Uid;
            if (questManager != null && uid != null)
                Quests = questManager.AssignQuests(uid);
        }
    }
}
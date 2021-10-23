using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
    public class QuestOwner : MonoBehaviour
    {
        [SerializeField]
        List<Quest> Quests;

        private void Start()
        {
            var questManager = FindObjectOfType<QuestManager>();
            var uid = GetComponent<UniqueID>().Uid;
            if (questManager != null && uid != null)
                Quests = questManager.AssignQuests(uid);
        }
    }
}
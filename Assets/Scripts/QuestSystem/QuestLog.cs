using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
    public enum QuestStatus
    {
        ACTIVE,
        FAILED,
        COMPLETED
    }

    public class AcceptedQuest : Quest
    {
        public QuestStatus status;
    }

    public class QuestLog : MonoBehaviour
    {
        [SerializeField]
        List<AcceptedQuest> Quests;
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    public class QuestLog : MonoBehaviour
    {
        [SerializeField]
        List<AcceptedQuest> AcceptedQuests = new List<AcceptedQuest>();

        public List<AcceptedQuest> Quests { get => AcceptedQuests; }

        public void AddQuest(Quest quest)
        {
            AcceptedQuests.Add(new AcceptedQuest(quest));
        }
    }
}

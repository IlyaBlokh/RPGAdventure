using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    public class QuestLog : MonoBehaviour
    {
        [SerializeField]
        private List<AcceptedQuest> AcceptedQuests = new();

        public List<AcceptedQuest> Quests => AcceptedQuests;

        public void AddQuest(Quest quest)
        {
            AcceptedQuests.Add(new AcceptedQuest(quest));
        }
    }
}

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

    [System.Serializable]
    public class AcceptedQuest : Quest
    {
        public QuestStatus status;

        public AcceptedQuest(Quest quest)
        {
            uid = quest.uid;
            owner = quest.owner;
            title = quest.title;
            description = quest.description;
            type = quest.type;
            experienceReward = quest.experienceReward;
            goldReward = quest.goldReward;
            huntGoalAmount = quest.huntGoalAmount;
            huntGoalAllowedIds = quest.huntGoalAllowedIds;
            gatherGoalAmount = quest.gatherGoalAmount;
            gatherGoalAllowedIds = quest.gatherGoalAllowedIds;
            explorePoint = quest.explorePoint;
            talkToId = quest.talkToId;
            status = QuestStatus.ACTIVE;
    }
    }

    public class QuestLog : MonoBehaviour
    {
        [SerializeField]
        List<AcceptedQuest> Quests = new List<AcceptedQuest>();

        public void AddQuest(Quest quest)
        {
            Quests.Add(new AcceptedQuest(quest));
        }
    }
}

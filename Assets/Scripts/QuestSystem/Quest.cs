using UnityEngine;

namespace QuestSystem
{
    public enum QuestType
    {
        HUNT,
        GATHER,
        EXPLORE,
        TALK
    }

    [System.Serializable]
    public class Quest
    {
        public string uid;
        public string owner;
        public string title;
        public string description;
        public QuestType type;
        public int experienceReward;
        public int goldReward;

        public int huntGoalAmount;
        public string[] huntGoalAllowedIds;

        public int gatherGoalAmount;
        public string[] gatherGoalAllowedIds;

        public Vector3 explorePoint;

        public string talkToId;
    }
}
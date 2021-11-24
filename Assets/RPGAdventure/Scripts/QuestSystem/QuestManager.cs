using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RPGAdventure
{
    public class QuestManager : MonoBehaviour, IMessageReceiver
    {
        [SerializeField]
        List<Quest> Quests;

        private PlayerStats m_PlayerStats;

        private void Awake()
        {
            UploadQuestsFromDB();
            m_PlayerStats = FindObjectOfType<PlayerStats>();
        }

       private void UploadQuestsFromDB()
       {
            using StreamReader reader = new StreamReader("Assets/RPGAdventure/DB/quests.json");
            string jsonStr = reader.ReadToEnd();
            Quests = JsonProcessor.JsonToList<Quest>(jsonStr);
       }
    
        public List<Quest> AssignQuests(string ownerUid)
        {
            List<Quest> ownerQuests = new List<Quest>();
            foreach(var quest in Quests)
            {
                if (quest.owner.Equals(ownerUid))
                    ownerQuests.Add(quest);
            }
            return ownerQuests;
        }

        public Quest GetQuest(string questId)
        {
            foreach (var quest in Quests)
            {
                if (quest.uid.Equals(questId))
                    return quest;
            }
            return null;
        }

        public void OnMessageReceive(IMessageReceiver.MessageType messageType, object messageData)
        {
            if (messageType == IMessageReceiver.MessageType.DEAD)
            {
                CheckForHuntQuestCompetion((Damageable.DamageData)messageData);
            }
        }

        private void CheckForHuntQuestCompetion(Damageable.DamageData damageData)
        {
            var questLog = damageData.DamageSender.GetComponent<QuestLog>();
            if (questLog == null) return;

            foreach (var quest in questLog.Quests)
            {
                if (quest.status == QuestStatus.ACTIVE &&
                    quest.type == QuestType.HUNT &&
                    Array.Exists(quest.huntGoalAllowedIds, (allowedId) => allowedId.Equals(damageData.DamageReceiver.GetComponent<UniqueID>().Uid)))
                {
                    quest.huntGoalAmount--;
                    if (quest.huntGoalAmount == 0)
                    {
                        quest.status = QuestStatus.COMPLETED;
                        m_PlayerStats.OnMessageReceive(IMessageReceiver.MessageType.QUEST_COMPLETE, quest);
                        Debug.Log("quest " + quest.uid + " is completed");
                    }
                }
            }
        }

    }
}

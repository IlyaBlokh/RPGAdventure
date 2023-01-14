using System;
using System.Collections.Generic;
using Core;
using DamageSystem;
using Player;
using UnityEngine;
using Utils;

namespace QuestSystem
{
    public class QuestManager : MonoBehaviour, IMessageReceiver
    {
        [SerializeField] private List<Quest> Quests;

        private PlayerStats playerStats;

        private void Awake()
        {
            UploadQuestsFromDB();
            playerStats = FindObjectOfType<PlayerStats>();
        }

       private void UploadQuestsFromDB()
       {
            string questsTextFile = Resources.Load<TextAsset>("DB/quests").text;
            Quests = JsonProcessor.JsonToList<Quest>(questsTextFile);
       }
    
        public List<Quest> AssignQuests(string ownerUid)
        {
            List<Quest> ownerQuests = new List<Quest>();
            foreach (Quest quest in Quests)
            {
                if (quest.owner.Equals(ownerUid))
                    ownerQuests.Add(quest);
            }
            return ownerQuests;
        }

        public Quest GetQuest(string questId)
        {
            foreach (Quest quest in Quests)
            {
                if (quest.uid.Equals(questId))
                    return quest;
            }
            return null;
        }

        public void OnMessageReceive(IMessageReceiver.MessageType messageType, object messageData)
        {
            if (messageType == IMessageReceiver.MessageType.Dead) 
                CheckForHuntQuestCompetion((Damageable.DamageData)messageData);
        }

        private void CheckForHuntQuestCompetion(Damageable.DamageData damageData)
        {
            var questLog = damageData.DamageSender.GetComponent<QuestLog>();
            if (questLog == null) return;

            foreach (AcceptedQuest quest in questLog.Quests)
            {
                if (quest.status == QuestStatus.ACTIVE &&
                    quest.type == QuestType.HUNT &&
                    Array.Exists(quest.huntGoalAllowedIds, (allowedId) => allowedId.Equals(damageData.DamageReceiver.GetComponent<UniqueID>().Uid)))
                {
                    quest.huntGoalAmount--;
                    if (quest.huntGoalAmount == 0)
                    {
                        quest.status = QuestStatus.COMPLETED;
                        playerStats.OnMessageReceive(IMessageReceiver.MessageType.QuestComplete, quest);
                        Debug.Log("quest " + quest.uid + " is completed");
                    }
                }
            }
        }

    }
}

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

        private void Awake()
        {
            UploadQuestsFromDB();
        }

       private void UploadQuestsFromDB()
       {
            using StreamReader reader = new StreamReader("Assets/DB/quests.json");
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

        public void OnMessageReceive(IMessageReceiver.MessageType messageType, object damageData)
        {
            if (messageType == IMessageReceiver.MessageType.DEAD)
            {
                CheckForHuntQuestCompetion((Damageable.DamageData)damageData);
            }
        }

        private void CheckForHuntQuestCompetion(Damageable.DamageData damageData)
        {
            Debug.Log(damageData.DamageReceiver);
            Debug.Log(damageData.DamageSender);
        }
    }
}

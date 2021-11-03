using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RPGAdventure
{
    public class QuestManager : MonoBehaviour, IDamageMessageReceiver
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

        public void OnDamageMessageReceive(IDamageMessageReceiver.DamageMessageType messageType)
        {
            if (messageType == IDamageMessageReceiver.DamageMessageType.DEAD)
            {
                CheckForHuntQuestCompetion();
            }
        }

        private void CheckForHuntQuestCompetion()
        {
            Debug.Log("Check if some HUNT quest is done");
        }
    }
}

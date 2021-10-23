using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RPGAdventure
{
    public class QuestManager : MonoBehaviour
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
    }
}
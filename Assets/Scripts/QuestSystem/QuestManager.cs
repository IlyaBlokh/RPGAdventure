using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RPGAdventure
{
    public class QuestManager : MonoBehaviour
    {
        [SerializeField]
        Quest[] Quests;

        private void Awake()
        {
            UploadQuestsFromDB();
        }

       private void UploadQuestsFromDB()
       {
            using StreamReader reader = new StreamReader("Assets/DB/quests.json");
            string jsonStr = reader.ReadToEnd();
            Quests = JsonProcessor.JsonToArray<Quest>(jsonStr);
       }
    }
}

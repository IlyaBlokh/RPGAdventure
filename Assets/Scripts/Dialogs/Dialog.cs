using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
    [System.Serializable]
    public class Dialog {

        [System.Serializable]
        public struct DialogQuery
        {
            [System.Serializable]
            public struct DialogAnswer
            {
                [TextArea(3, 15)]
                public string answerText;
                public bool isForceExited;
                public string questId;
            }

            [TextArea(3, 15)]
            public string queryText;
            public bool isAsked;
            public DialogAnswer dialogAnswer;
        }


        [TextArea(3,15)]
        public string welcomeText;
        public List<DialogQuery> queries;
    }
}

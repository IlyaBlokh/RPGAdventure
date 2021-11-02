using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{

    [System.Serializable]
    public class DialogAnswer
    {
        [TextArea(3, 15)]
        public string answerText;
        public bool shouldForceExit;
        public string questId;
    }

    [System.Serializable]
    public class DialogQuery
    {
        [TextArea(3, 15)]
        public string queryText;
        public bool isAsked;
        public bool shouldAlwaysAsk;
        public DialogAnswer dialogAnswer;
    }

    [System.Serializable]
    public class Dialog {
        [TextArea(3,15)]
        public string welcomeText;
        public DialogQuery[] queries;
    }
}

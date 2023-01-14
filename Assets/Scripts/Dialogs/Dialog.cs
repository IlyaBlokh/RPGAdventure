using UnityEngine;

namespace Dialogs
{
    [System.Serializable]
    public class Dialog {
        [TextArea(3,15)]
        public string welcomeText;
        public DialogQuery[] queries;
    }
}

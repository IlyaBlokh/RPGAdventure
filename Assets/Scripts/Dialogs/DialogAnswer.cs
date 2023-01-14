using UnityEngine;

namespace Dialogs
{
  [System.Serializable]
  public class DialogAnswer
  {
    [TextArea(3, 15)]
    public string answerText;
    public bool shouldForceExit;
    public string questId;
  }
}
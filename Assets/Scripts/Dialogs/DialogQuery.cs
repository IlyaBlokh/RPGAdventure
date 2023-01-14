using UnityEngine;

namespace Dialogs
{
  [System.Serializable]
  public class DialogQuery
  {
    [TextArea(3, 15)]
    public string queryText;
    public bool isAsked;
    public bool shouldAlwaysAsk;
    public DialogAnswer dialogAnswer;
  }
}
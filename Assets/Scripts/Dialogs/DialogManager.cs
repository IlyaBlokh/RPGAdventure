using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
    public class DialogManager : MonoBehaviour
    {
        private void Update()
        {
            if (PlayerInput.Instance != null && PlayerInput.Instance.IsInteracking)
            {
                Debug.Log("Start interact");
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
    public class PlayerStats : MonoBehaviour, IMessageReceiver
    {
        [SerializeField]
        int currentLevel;
        [SerializeField]
        int experience;
        [SerializeField]
        int maxLevel;
        [SerializeField]
        int[] availableLevels;

        private void Awake()
        {
            availableLevels = new int[maxLevel];
            for (var i = 0; i < maxLevel; i++)
            {
                availableLevels[i] = maxLevel * i * i;
            }
        }

        public void OnMessageReceive(IMessageReceiver.MessageType messageType, object messageData)
        {
            if (messageType == IMessageReceiver.MessageType.DEAD)
            {
                GainExperience(100);
            }
        }

        private void GainExperience(int exp)
        {
            experience += exp;
        }
    }
}
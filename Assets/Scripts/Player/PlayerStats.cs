using System;
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
                availableLevels[i] = maxLevel * Convert.ToInt32(Mathf.Pow(i, 2));
            }
        }

        public void OnMessageReceive(IMessageReceiver.MessageType messageType, object messageData)
        {
            if (messageType == IMessageReceiver.MessageType.DEAD)
            {
                int exp = ((Damageable.DamageData)messageData).DamageReceiver.ExperienceForKill;
                GainExperience(exp);
            }
        }

        private void GainExperience(int exp)
        {
            experience += exp;
            if (currentLevel == maxLevel) return;
            for (int i = currentLevel + 1; i < maxLevel; i++)
            {
                if (experience >= availableLevels[i])
                    currentLevel++;
                else
                    break;
            }
        }
    }
}
using System;
using DamageSystem;
using QuestSystem;
using UnityEngine;

namespace Player
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
            var exp = 0;
            switch (messageType)
            {
                case IMessageReceiver.MessageType.DEAD:
                    exp = ((Damageable.DamageData)messageData).DamageReceiver.ExperienceForKill;
                    break;
                case IMessageReceiver.MessageType.QUEST_COMPLETE:
                    exp = ((AcceptedQuest)messageData).experienceReward;
                    break;
                default: break;
            }
            GainExperience(exp);
        }

        public void GainExperience(int exp)
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
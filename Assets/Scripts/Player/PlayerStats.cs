using System;
using DamageSystem;
using QuestSystem;
using UnityEngine;

namespace Player
{
    public class PlayerStats : MonoBehaviour, IMessageReceiver
    {
        [SerializeField] private int currentLevel;
        [SerializeField] private int experience;
        [SerializeField] private int maxLevel;
        [SerializeField] private int[] availableLevels;

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
                case IMessageReceiver.MessageType.Dead:
                    exp = ((Damageable.DamageData)messageData).DamageReceiver.ExperienceForKill;
                    break;
                case IMessageReceiver.MessageType.QuestComplete:
                    exp = ((AcceptedQuest)messageData).experienceReward;
                    break;
            }
            GainExperience(exp);
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
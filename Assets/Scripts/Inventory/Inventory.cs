using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPGAdventure {
    
    public class Inventory : MonoBehaviour
    {
        [SerializeField]
        int size;

        [SerializeField]
        List<InventorySlotHandler> inventory;

        [SerializeField]
        UnityEvent<int, string> onSlotTaken;

        private int currentVolume = 0;
        private InventoryUIManager m_UIManager;

        public int Size { get => size; set => size = value; }

        private void Awake()
        {
            inventory = new List<InventorySlotHandler>();
            for (int i = 0; i < size; i++)
            {
                inventory.Add(new InventorySlotHandler(i));
            }

            if (tag == "Player")
            {
                m_UIManager = FindObjectOfType<InventoryUIManager>();
                onSlotTaken.AddListener(m_UIManager.onSlotTaken);
            }
        }

        public void OnItemPickup(GameObject item)
        {
            //We don't allow to have 2 identic items
            if (!ContainsItem(item.GetComponent<UniqueID>().Uid) &&
                currentVolume < size)
            {
                AddItem(item);
            }
        }

        private bool ContainsItem(string itemId)
        {
            var slot = inventory.Find(slot => slot.ContainsItemInSlot(itemId));
            return !string.IsNullOrEmpty(slot?.ItemID);
        }

        private void AddItem(GameObject item)
        {
            inventory[currentVolume].ItemID = item.GetComponent<UniqueID>().Uid;
            onSlotTaken.Invoke(currentVolume, item.name);
            Destroy(item);
            currentVolume++;

            //TODO: if melee weapon -> take into hands if empty
            GameObject weaponToGet = FindObjectOfType<InventoryManager>().GetWeaponOut(inventory[currentVolume-1].ItemID);
            DrawWeapon(weaponToGet);
            //TODO: disable player attack if disarmed
        }

        private void DrawWeapon(GameObject weapon)
        {
/*            Instantiate(weapon, transform);*/
        }
    }
}
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
            Debug.Log("volume:" + currentVolume);
            Debug.Log("try to pick up:" + item.GetComponent<UniqueID>().Uid);
            for (int i = 0; i < currentVolume; i++)
                Debug.Log("inventory slot " + i + " is " + inventory[i].Item);
            if (!ContainsItem(item.GetComponent<UniqueID>().Uid) &&
                currentVolume < size)
            {
                AddItem(item);
            }
        }

        private bool ContainsItem(string itemId)
        {
            for (int i = 0; i < currentVolume; i++)
                if (inventory[i].ItemId.Equals(itemId)) return true;
            return false;
        }

        private void AddItem(GameObject item)
        {
            inventory[currentVolume].Item = item;
            onSlotTaken.Invoke(currentVolume, item.name);
            //TODO removing item by destroy empties inventory. need other solution!
            //Destroy(item);
            currentVolume++;
            for (int i = 0; i < currentVolume; i++)
                Debug.Log("inventory slot " + i + " has id " + inventory[i].ItemId);
            //TODO: if melee weapon -> take into hands if empty
            //TODO: disable player attack if disarmed
        }
    }
}
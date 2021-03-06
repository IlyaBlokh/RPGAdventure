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
        List<InventorySlotHandler> inventory = new List<InventorySlotHandler>();

        [SerializeField]
        UnityEvent<int, Sprite> onSlotTaken;

        private int currentVolume = 0;
        private InventoryUIManager m_UIManager;
        private InventoryManager m_InventoryManager;

        public int Size { get => size; set => size = value; }

        private void Awake()
        {
            for (int i = 0; i < size; i++)
            {
                inventory.Add(new InventorySlotHandler(i));
            }

            if (tag == "Player")
            {
                m_InventoryManager = FindObjectOfType<InventoryManager>();
                m_UIManager = FindObjectOfType<InventoryUIManager>();
                onSlotTaken.AddListener(m_UIManager.onSlotTaken);
            }
        }

        public void OnItemPickup(GameObject item)
        {
            //We don't allow to have 2 identic items
            if (!ContainsItem(item.GetComponent<UniqueID>().Uid) &&
                HasNoOwner(item) &&
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

        private bool HasNoOwner(GameObject item)
        {
            return item.GetComponent<MeleeWeapon>()?.Owner == null;
        }

        private void AddItem(GameObject item)
        {
            var uid = item.GetComponent<UniqueID>().Uid;
            inventory[currentVolume].ItemID = uid;
            onSlotTaken.Invoke(currentVolume, m_InventoryManager.GetWeaponIcon(uid));
            Destroy(item);
            currentVolume++;
        }

        public void OnInventorySlotPick(int index)
        {
            if (tag == "Player")
            {
                var isInputBlocked = FindObjectOfType<PlayerInput>().IsPlayerControllerInputBlocked;
                if (!isInputBlocked)
                {
                    var itemID = inventory[index].ItemID;
                    if (!string.IsNullOrEmpty(itemID))
                    {
                        var item = m_InventoryManager.GetItem(itemID);
                        PlayerController.Instance.EquipItem(item);
                    }
                }
            }
        }
    }
}
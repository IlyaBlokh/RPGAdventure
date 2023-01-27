using System.Collections.Generic;
using Core;
using Player;
using UnityEngine;
using UnityEngine.Events;
using Weapons;

namespace Inventory {
    
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private int size;
        [SerializeField] private List<InventorySlotHandler> inventory = new();
        [SerializeField] private UnityEvent<int, Sprite> onSlotTaken;

        private int currentVolume;
        private InventoryUIManager interfaceManager;
        private InventoryManager inventoryManager;

        public int Size => size;

        private void Awake()
        {
            for (int i = 0; i < size; i++) 
                inventory.Add(new InventorySlotHandler(i));

            if (CompareTag("Player"))
            {
                inventoryManager = FindObjectOfType<InventoryManager>();
                interfaceManager = FindObjectOfType<InventoryUIManager>();
                onSlotTaken.AddListener(interfaceManager.OnSlotTaken);
            }
        }

        public void OnItemPickup(GameObject item)
        {
            //We don't allow to have 2 identic items
            if (!ContainsItem(item.GetComponent<UniqueID>().Uid) &&
                HasNoOwner(item) &&
                currentVolume < size) 
                AddItem(item);
        }

        private bool ContainsItem(string itemId)
        {
            InventorySlotHandler slot = inventory.Find(slot => slot.ContainsItemInSlot(itemId));
            if (slot == null) return false;
            return !string.IsNullOrEmpty(slot.ItemID);
        }

        private bool HasNoOwner(GameObject item) => 
            item.GetComponent<MeleeWeapon>()?.Owner == null;

        private void AddItem(GameObject item)
        {
            string uid = item.GetComponent<UniqueID>().Uid;
            inventory[currentVolume].ItemID = uid;
            onSlotTaken.Invoke(currentVolume, inventoryManager.GetWeaponIcon(uid));
            Destroy(item);
            currentVolume++;
        }

        public void OnInventorySlotPick(int index)
        {
            if (CompareTag("Player"))
            {
                bool isInputBlocked = FindObjectOfType<InputController>().IsPlayerControllerInputBlocked;
                if (isInputBlocked) 
                    return;
                string itemID = inventory[index].ItemID;
                if (!string.IsNullOrEmpty(itemID))
                {
                    GameObject item = inventoryManager.GetItem(itemID);
                    PlayerController.Instance.EquipItem(item);
                }
            }
        }
    }
}
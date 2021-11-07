using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
    public class InventorySlotHandler : MonoBehaviour
    {
        public readonly int Index;
        private GameObject item;
        public GameObject Item { get => item; set => item = value; }

        public string ItemId { get => Item.GetComponent<UniqueID>().Uid; }

        public InventorySlotHandler(int index)
        {
            Index = index;
        }
    }
}

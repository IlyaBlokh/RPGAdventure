using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
    public class InventorySlotHandler : MonoBehaviour
    {
        public readonly int Index;
        private UniqueID itemID;
        public UniqueID ItemId { get => itemID; set => itemID = value; }

        public InventorySlotHandler(int index)
        {
            Index = index;
        }
    }
}

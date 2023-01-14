using UnityEngine;

namespace Inventory
{
    public class InventorySlotHandler : MonoBehaviour
    {
        public readonly int Index;
        private string itemID;

        public string ItemID { get => itemID; set => itemID = value; }

        public InventorySlotHandler(int index)
        {
            Index = index;
            itemID = "";
        }

        public bool ContainsItemInSlot(string iID)
        {
            return itemID == iID;
        }
    }
}

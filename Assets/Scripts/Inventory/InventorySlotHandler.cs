using UnityEngine;

namespace Inventory
{
    public class InventorySlotHandler : MonoBehaviour
    {
        public readonly int Index;

        public string ItemID { get; set; }

        public InventorySlotHandler(int index)
        {
            Index = index;
            ItemID = "";
        }

        public bool ContainsItemInSlot(string iID)
        {
            return ItemID == iID;
        }
    }
}

using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Inventory {
    public class InventoryUIManager : MonoBehaviour
    {
        [SerializeField] private Button InventorySlot;
        [SerializeField] private RectTransform InventoryPanel;
        [SerializeField] private UnityEvent<int> onInventorySlotPick;
        
        private List<Button> slots = new();
        private float slotLeftPosition;
        private void Start()
        {
            onInventorySlotPick.AddListener(FindObjectOfType<InventoryManager>().GetPlayerInventory().OnInventorySlotPick);
            InitInventoryUI();        
        }

        private void InitInventoryUI()
        {
            int playerInventorySize = FindObjectOfType<PlayerController>().GetComponent<Inventory>().Size;
            float slotWidth = InventoryPanel.rect.width / playerInventorySize;
            for (int i = 0; i < playerInventorySize; i++)
            {
                Button SlotInstance = Instantiate(InventorySlot, InventoryPanel.transform);
                RectTransform btnRectTransform = SlotInstance.GetComponent<RectTransform>();
                btnRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, slotLeftPosition, slotWidth);
                btnRectTransform.GetComponent<Button>().onClick.AddListener(() =>
                {
                    int index = slots.FindIndex(slot => slot == SlotInstance);
                    onInventorySlotPick.Invoke(index);
                });
                slots.Add(SlotInstance);
                slotLeftPosition += slotWidth;
            }
        }

        public void OnSlotTaken(int index, Sprite icon)
        {
            slots[index].GetComponentInChildren<Image>().sprite = icon;
        }
    }
}
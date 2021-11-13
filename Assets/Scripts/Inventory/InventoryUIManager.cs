using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RPGAdventure {
    public class InventoryUIManager : MonoBehaviour
    {
        [SerializeField]
        Button InventorySlot;

        [SerializeField]
        RectTransform InventoryPanel;

        [SerializeField]
        UnityEvent<int> onInventorySlotPick;

        private List<Button> m_slots = new List<Button>();

        private float m_SlotLeftPosition = .0f;
        private void Awake()
        {
            onInventorySlotPick.AddListener(FindObjectOfType<InventoryManager>().GetPlayerInventory().OnInventorySlotPick);
            InitInventoryUI();        
        }

        private void InitInventoryUI()
        {
            var playerInventorySize = FindObjectOfType<PlayerController>().GetComponent<Inventory>().Size;
            var slotWidth = InventoryPanel.rect.width / playerInventorySize;
            for (int i = 0; i < playerInventorySize; i++)
            {
                var SlotInstance = Instantiate(InventorySlot, InventoryPanel.transform);
                RectTransform btnRectTransform = SlotInstance.GetComponent<RectTransform>();
                btnRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, m_SlotLeftPosition, slotWidth);
                btnRectTransform.GetComponent<Button>().onClick.AddListener(() =>
                {
                    var index = m_slots.FindIndex(slot => slot == SlotInstance);
                    onInventorySlotPick.Invoke(index);
                });
                m_slots.Add(SlotInstance);
                m_SlotLeftPosition += slotWidth;
            }
        }

        public void onSlotTaken(int index, string itemName)
        {
            m_slots[index].GetComponentInChildren<Text>().text = itemName;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPGAdventure {
    public class InventoryUIManager : MonoBehaviour
    {
        [SerializeField]
        Button InventorySlot;

        [SerializeField]
        RectTransform InventoryPanel;

        private List<Button> m_slots = new List<Button>();

        private float m_SlotLeftPosition = .0f;
        private void Awake()
        {
            var playerInventorySize = FindObjectOfType<PlayerController>().GetComponent<Inventory>().Size;
            var slotWidth = InventoryPanel.rect.width / playerInventorySize; 
            for (int i = 0; i < playerInventorySize; i++)
            {
                var SlotInstance = Instantiate(InventorySlot, InventoryPanel.transform);
                RectTransform btnRectTransform = SlotInstance.GetComponent<RectTransform>();
                btnRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, m_SlotLeftPosition, slotWidth);
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
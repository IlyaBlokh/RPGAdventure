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
        GameObject InventoryPanel;

        private float m_SlotLeftPosition = .0f;
        private void Awake()
        {
            var playerInventorySize = FindObjectOfType<PlayerController>().GetComponent<Inventory>().Size;
            for (int i = 0; i < playerInventorySize; i++)
            {
                var SlotInstance = Instantiate(InventorySlot, InventoryPanel.transform);
                RectTransform btnRectTransform = SlotInstance.GetComponent<RectTransform>();
                btnRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, m_SlotLeftPosition, btnRectTransform.rect.width);
                m_SlotLeftPosition += btnRectTransform.rect.width;
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure {
    
    public class Inventory : MonoBehaviour
    {
        [SerializeField]
        int size;

        [SerializeField]
        Dictionary<UniqueID, GameObject> inventory;

        private void Awake()
        {
            inventory = new Dictionary<UniqueID, GameObject>();
        }

        public void AddItem(GameObject item)
        {
            if (inventory.Count == size) return;
            inventory.Add(item.GetComponent<UniqueID>(), item);
            Debug.Log(item.name);
        }

        public bool HasItem(UniqueID id)
        {
            return inventory.ContainsKey(id);
        }
    }
}
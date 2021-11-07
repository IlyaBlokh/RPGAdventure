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
            //We don't allow to have 2 identic items
            if (!inventory.ContainsKey(item.GetComponent<UniqueID>()))
            {
                if (inventory.Count == size) return;
                inventory.Add(item.GetComponent<UniqueID>(), item);
                Debug.Log(item.name);
            }
        }
    }
}
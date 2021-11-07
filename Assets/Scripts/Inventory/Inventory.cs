using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure {
    
    public class Inventory : MonoBehaviour
    {
        [SerializeField]
        int size;

        [SerializeField]
        Dictionary<string, GameObject> inventory;

        private void Awake()
        {
            inventory = new Dictionary<string, GameObject>();
        }

        public void OnItemPickup(GameObject item)
        {
            //We don't allow to have 2 identic items
            if (!inventory.ContainsKey(item.GetComponent<UniqueID>().Uid) &&
                inventory.Count < size)
            {
                AddItem(item);
            }
        }

        private void AddItem(GameObject item)
        {
            inventory.Add(item.GetComponent<UniqueID>().Uid, item);
            Destroy(item);
            Debug.Log(item.GetComponent<UniqueID>().Uid);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField]
        List<GameObject> WeaponList = new List<GameObject>();

        public GameObject GetItem(string itemID)
        {
            return WeaponList.Find(weapon => weapon.GetComponent<UniqueID>().Uid == itemID);
        }

        public Inventory GetPlayerInventory()
        {
            return PlayerController.Instance.GetComponent<Inventory>();
        }
    }
}

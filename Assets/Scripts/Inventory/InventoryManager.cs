using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField]
        GameObject[] WeaponList;

        public GameObject GetItem(string itemID)
        {
            foreach (var weapon in WeaponList)
            {
                if (weapon.GetComponent<UniqueID>().Uid == itemID)
                    return weapon;
            }
            return null;
        }

        public Inventory GetPlayerInventory()
        {
            return PlayerController.Instance.GetComponent<Inventory>();
        }
    }
}

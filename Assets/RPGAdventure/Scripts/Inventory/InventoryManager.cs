using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField]
        List<GameObject> WeaponList = new List<GameObject>();

        [SerializeField]
        List<Sprite> WeaponIcons = new List<Sprite>();

        public GameObject GetItem(string itemID)
        {
            return WeaponList.Find(weapon => weapon.GetComponent<UniqueID>().Uid == itemID);
        }

        public Sprite GetWeaponIcon(string itemID)
        {
            var index = WeaponList.FindIndex(weapon => weapon.GetComponent<UniqueID>().Uid == itemID);
            return WeaponIcons[index];
        }

        public Inventory GetPlayerInventory()
        {
            return PlayerController.Instance.GetComponent<Inventory>();
        }
    }
}

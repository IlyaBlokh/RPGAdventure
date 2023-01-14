using System.Collections.Generic;
using Core;
using Player;
using UnityEngine;

namespace Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> WeaponList = new();
        [SerializeField] private List<Sprite> WeaponIcons = new();

        public GameObject GetItem(string itemID) => 
            WeaponList.Find(weapon => weapon.GetComponent<UniqueID>().Uid == itemID);

        public Sprite GetWeaponIcon(string itemID) => 
            WeaponIcons[WeaponList.FindIndex(weapon => weapon.GetComponent<UniqueID>().Uid == itemID)];

        public Inventory GetPlayerInventory() => 
            PlayerController.Instance.GetComponent<Inventory>();
    }
}

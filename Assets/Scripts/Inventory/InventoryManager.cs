using System.Collections.Generic;
using Core;
using Player;
using UnityEngine;
using Zenject;

namespace Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> WeaponList = new();
        [SerializeField] private List<Sprite> WeaponIcons = new();
        private PlayerController playerController;

        [Inject]
        private void Construct(PlayerController playerController)
        {
            this.playerController = playerController;
        }
        
        public GameObject GetItem(string itemID) => 
            WeaponList.Find(weapon => weapon.GetComponent<UniqueID>().Uid == itemID);

        public Sprite GetWeaponIcon(string itemID) => 
            WeaponIcons[WeaponList.FindIndex(weapon => weapon.GetComponent<UniqueID>().Uid == itemID)];

        public Inventory GetPlayerInventory() => 
            playerController.GetComponent<Inventory>();
    }
}

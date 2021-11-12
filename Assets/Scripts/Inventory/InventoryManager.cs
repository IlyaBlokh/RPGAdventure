using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField]
        GameObject[] WeaponList;

        public GameObject GetWeaponOut(UniqueID weaponID)
        {
            foreach (var weapon in WeaponList)
            {
                if (weapon.GetComponent<UniqueID>().Uid == weaponID.Uid)
                    return weapon;
            }
            return null;
        }
    }
}

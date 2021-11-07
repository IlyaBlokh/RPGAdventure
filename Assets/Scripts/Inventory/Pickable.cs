using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPGAdventure
{
    public class Pickable : MonoBehaviour
    {
        [SerializeField]
        LayerMask allowedOwnerLayers;

        [SerializeField]
        UnityEvent<GameObject> onItemPickup;

        private void Awake()
        {
            Inventory[] inventories = FindObjectsOfType<Inventory>();
            foreach(var inventory in inventories)
                onItemPickup.AddListener(inventory.OnItemPickup);
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((allowedOwnerLayers.value & 1 << other.gameObject.layer) != 0)
            {
                onItemPickup.Invoke(gameObject);
            }
        }
    }
}

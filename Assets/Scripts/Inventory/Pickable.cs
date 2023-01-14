using Core;
using UnityEngine;
using UnityEngine.Events;

namespace Inventory
{
    [RequireComponent(typeof(UniqueID))]
    public class Pickable : MonoBehaviour
    {
        [SerializeField] private LayerMask allowedOwnerLayers;
        [SerializeField] private UnityEvent<GameObject> onItemPickup;

        private void Awake()
        {
            Inventory[] inventories = FindObjectsOfType<Inventory>();
            foreach(Inventory inventory in inventories)
                onItemPickup.AddListener(inventory.OnItemPickup);
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((allowedOwnerLayers.value & 1 << other.gameObject.layer) != 0) 
                onItemPickup.Invoke(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
    public class Pickable : MonoBehaviour
    {
        [SerializeField]
        LayerMask allowedOwnerLayers;

        private void OnTriggerEnter(Collider other)
        {
            if ((allowedOwnerLayers.value & 1 << other.gameObject.layer) != 0 &&
                other.GetComponent<Inventory>() &&
                !other.GetComponent<Inventory>().HasItem(GetComponent<UniqueID>()))
            {
                other.GetComponent<Inventory>().AddItem(gameObject);
                Destroy(gameObject);
            }
        }
    }
}

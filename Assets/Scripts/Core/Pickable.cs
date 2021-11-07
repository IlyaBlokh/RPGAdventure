using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    [SerializeField]
    LayerMask allowedOwnerLayers;

    private void OnTriggerEnter(Collider other)
    {
        //TODO: check if is not in inventory already
        if ((allowedOwnerLayers.value & 1 << other.gameObject.layer) != 0)
        {
            Debug.Log(other.name);
        }
    }
}

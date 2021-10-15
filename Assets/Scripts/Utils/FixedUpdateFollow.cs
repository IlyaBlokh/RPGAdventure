using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
    public class FixedUpdateFollow : MonoBehaviour
    {
        [SerializeField]
        Transform ToFollow;

        private void FixedUpdate()
        {
            transform.position = ToFollow.position;
            transform.rotation = ToFollow.rotation;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
    public class FixedUpdateFollow : MonoBehaviour
    {
        [SerializeField]
        Transform ToFollow;

        private void Awake()
        {
            if (ToFollow != null)
            {
                FollowParent(ToFollow);
            }
        }

        public void FollowParent(Transform toFollow)
        {
            transform.position = toFollow.position;
            transform.rotation = toFollow.rotation;
            transform.SetParent(toFollow);
        }
    }
}

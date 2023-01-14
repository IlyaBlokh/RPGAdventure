using UnityEngine;

namespace Utils
{
    public class FixedUpdateFollow : MonoBehaviour
    {
        [SerializeField] private Transform ToFollow;

        private void Awake()
        {
            if (ToFollow != null) 
                FollowParent(ToFollow);
        }

        public void FollowParent(Transform toFollow)
        {
            transform.position = toFollow.position;
            transform.rotation = toFollow.rotation;
            transform.SetParent(toFollow);
        }
    }
}

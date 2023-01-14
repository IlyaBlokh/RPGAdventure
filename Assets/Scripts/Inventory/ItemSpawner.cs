using UnityEngine;

namespace Inventory
{
    public class ItemSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject itemToSpawn;
        private void Start()
        {
            Instantiate(itemToSpawn, transform.position, Quaternion.identity);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            UnityEditor.Handles.color = new Color(.0f, .8f, 0.8f, 0.4f);
            UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, transform.position, 360, 0.5f);
        }
#endif
    }
}
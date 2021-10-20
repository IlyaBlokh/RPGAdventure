using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure {
    public class RagdollReplacer : MonoBehaviour
    {
        [SerializeField]
        GameObject RagdollPrefab;
        public void ReplaceWithRagdoll()
        {
            GameObject ragdoll = Instantiate(RagdollPrefab, transform.position, transform.rotation);
            CopyTransform(transform, ragdoll.transform);
            Destroy(gameObject);
        }

        public void CopyTransform(Transform original, Transform ragdoll)
        {
            ragdoll.position = original.position;
            ragdoll.rotation = original.rotation;
            for (var i = 0; i < original.childCount; i++)
            {
                CopyTransform(original.GetChild(i), ragdoll.GetChild(i));
            }
        }
    }
}

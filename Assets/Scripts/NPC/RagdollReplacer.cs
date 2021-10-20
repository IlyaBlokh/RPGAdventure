using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure {
    public class RagdollReplacer : MonoBehaviour
    {
        [SerializeField]
        GameObject RagdollPrefab;

        [SerializeField]
        GameObject DeadVFX;
        public void ReplaceWithRagdoll()
        {
            GameObject ragdoll = Instantiate(RagdollPrefab, transform.position, transform.rotation);
            CopyTransform(transform, ragdoll.transform);

            GameObject deadVFX = Instantiate(DeadVFX);
            deadVFX.transform.transform.position = ragdoll.transform.position;
            deadVFX.transform.parent = ragdoll.transform;

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

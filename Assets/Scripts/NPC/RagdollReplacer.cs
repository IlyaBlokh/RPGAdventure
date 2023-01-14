using DamageSystem;
using UnityEngine;

namespace NPC {
    public class RagdollReplacer : MonoBehaviour
    {
        [SerializeField]
        GameObject RagdollPrefab;

        [SerializeField]
        GameObject DeadVFX;
        public void ReplaceWithRagdoll()
        {
            var UI = GetComponent<DamageableUI>().HealthUI;
            if (UI)
            {
                UI.transform.SetParent(null);
                Destroy(UI);
            }

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

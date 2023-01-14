using DamageSystem;
using UnityEngine;

namespace NPC {
    public class RagdollReplacer : MonoBehaviour
    {
        [SerializeField] private GameObject RagdollPrefab;
        [SerializeField] private GameObject DeadVFX;
        public void ReplaceWithRagdoll()
        {
            GameObject healthUI = GetComponent<DamageableUI>().HealthUI;
            if (healthUI)
            {
                healthUI.transform.SetParent(null);
                Destroy(healthUI);
            }

            GameObject ragdoll = Instantiate(RagdollPrefab, transform.position, transform.rotation);
            CopyTransform(transform, ragdoll.transform);

            GameObject deadVFX = Instantiate(DeadVFX);
            deadVFX.transform.transform.position = ragdoll.transform.position;
            deadVFX.transform.parent = ragdoll.transform;

            Destroy(gameObject);
        }

        private void CopyTransform(Transform original, Transform ragdoll)
        {
            ragdoll.position = original.position;
            ragdoll.rotation = original.rotation;
            for (var i = 0; i < original.childCount; i++) 
                CopyTransform(original.GetChild(i), ragdoll.GetChild(i));
        }
    }
}

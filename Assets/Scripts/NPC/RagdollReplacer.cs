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

            Destroy(gameObject);
        }
    }
}

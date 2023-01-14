using System.Collections;
using UnityEngine;

namespace Graphics
{
    public class Dissolve : MonoBehaviour
    {
        [SerializeField]
        float DissolveTime;

        private void Awake()
        {
            StartCoroutine(DissolveSelf());
        }

        private IEnumerator DissolveSelf()
        {
            yield return new WaitForSeconds(DissolveTime);
            Destroy(gameObject);
        }
    }
}
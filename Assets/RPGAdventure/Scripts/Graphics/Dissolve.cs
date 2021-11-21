using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
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
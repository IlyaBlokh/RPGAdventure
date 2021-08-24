using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
    public class BanditBehaviour : MonoBehaviour
    {
        [SerializeField]
        float DetectionRange = 10.0f;

        private float m_CurrentDistance;

        private void Update()
        {
            LookForPlayer();
        }

        private PlayerController LookForPlayer()
        {
            if (PlayerController.Instance == null)
            {
                return null;
            }

            m_CurrentDistance = (PlayerController.Instance.transform.position - transform.position).magnitude;
            if (m_CurrentDistance <= DetectionRange)
            {
                return PlayerController.Instance;
            }
            else
            {
                return null;
            }
        }
    }
}
using System;
using UnityEngine;

namespace Core
{
    [System.Serializable]
    public class UniqueID : MonoBehaviour
    {
        [SerializeField] private string m_uid = Guid.NewGuid().ToString();

        public string Uid => m_uid;
    }
}

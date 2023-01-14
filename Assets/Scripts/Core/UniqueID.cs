using System;
using UnityEngine;

namespace Core
{
    [System.Serializable]
    public class UniqueID : MonoBehaviour
    {
        [SerializeField]
        string m_uid = Guid.NewGuid().ToString();

        public string Uid { get => m_uid; }
    }
}

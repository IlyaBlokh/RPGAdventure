using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
    public class UniqueID : MonoBehaviour
    {
        [SerializeField]
        string m_uid = Guid.NewGuid().ToString();

        public string Uid { get => m_uid; }
    }
}

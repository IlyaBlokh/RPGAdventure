using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
    public class PlayerInput : MonoBehaviour
    {
        private Vector3 m_PlayerInput;

        public Vector3 MoveInput
        {
            get { return m_PlayerInput.normalized; }
        }

        public bool IsMoving
        {
            get { return !Mathf.Approximately(m_PlayerInput.magnitude, 0); }
        }

        void Update()
        {
            m_PlayerInput.Set(
                Input.GetAxis("Horizontal"),
                0,
                Input.GetAxis("Vertical"));
        }
    }
}
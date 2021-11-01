using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
    public class Clickable : MonoBehaviour
    {
        [SerializeField]
        Texture2D CursorImage;

        [SerializeField]
        CursorMode cursorMode = CursorMode.Auto;

        [SerializeField]
        float minClickDistance;

        [SerializeField]
        float maxInteractDistance;

        private Vector2 m_Hotspot;

        private void Awake()
        {
            m_Hotspot = new Vector2(CursorImage.width / 2, CursorImage.height / 2);
        }

        private void OnMouseEnter()
        {
            Cursor.SetCursor(CursorImage, m_Hotspot, cursorMode);
        }

        private void OnMouseExit()
        {
            Cursor.SetCursor(null, m_Hotspot, cursorMode);
        }

        /// <summary>
        /// Handles click action over the gameobject
        /// </summary>
        /// <returns>Clickable object if action is acceptable, null otherwise</returns>
        public Clickable CheckClickCondition()
        {
            var PlayerPosition = FindObjectOfType<PlayerController>().transform.position;
            var distanceToPlayer = Vector3.Distance(PlayerPosition, transform.position);
            if (distanceToPlayer <= minClickDistance) 
                return this;
            else
                return null;
        }

        public bool CheckEndInteractCondition()
        {
            var PlayerPosition = FindObjectOfType<PlayerController>().transform.position;
            var distanceToPlayer = Vector3.Distance(PlayerPosition, transform.position);
            if (distanceToPlayer >= maxInteractDistance)
                return true;
            else
                return false;
        }
    }
}

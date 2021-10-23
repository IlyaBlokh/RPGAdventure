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
    }
}

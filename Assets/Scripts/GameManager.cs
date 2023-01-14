using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Texture2D CursorImage;

    [SerializeField]
    CursorMode cursorMode = CursorMode.Auto;

    private Vector2 m_Hotspot;

    private void Awake()
    {
        m_Hotspot = new Vector2(0, 0);
    }

    void Start()
    {
        SetCursor();
    }

    public void SetCursor()
    {
        Cursor.SetCursor(CursorImage, m_Hotspot, cursorMode);
    }

}

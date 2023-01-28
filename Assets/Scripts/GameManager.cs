using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Texture2D CursorImage;
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;

    private Vector2 hotspot;

    [Inject]
    private void Construct()
    {
        hotspot = new Vector2(0, 0);
        SetCursor();
    }
    
    public void SetCursor()
    {
        Cursor.SetCursor(CursorImage, hotspot, cursorMode);
        DisableCursor();
    }

    public void EnableCursor() => 
        Cursor.visible = true;
    public void DisableCursor() => 
        Cursor.visible = false;

}

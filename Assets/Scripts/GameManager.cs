using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Texture2D CursorImage;
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;

    private Vector2 hotspot;

    private void Awake()
    {
        hotspot = new Vector2(0, 0);
    }

    private void Start()
    {
        SetCursor();
    }

    public void SetCursor()
    {
        Cursor.SetCursor(CursorImage, hotspot, cursorMode);
    }

}

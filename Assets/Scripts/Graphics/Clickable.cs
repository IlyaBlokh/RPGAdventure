using Player;
using UnityEngine;

namespace Graphics
{
    public class Clickable : MonoBehaviour
    {
        [SerializeField] private Texture2D CursorImage;
        [SerializeField] private CursorMode cursorMode = CursorMode.Auto;
        [SerializeField] private float minClickDistance;
        [SerializeField] private float maxInteractDistance;

        private Vector2 hotspot;
        private GameManager gameManager;

        private void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
            hotspot = new Vector2(0, 0);
        }

        private void OnMouseEnter()
        {
            Cursor.SetCursor(CursorImage, hotspot, cursorMode);
        }

        private void OnMouseExit()
        {
            gameManager.SetCursor();
        }

        /// <summary>
        /// Handles click action over the gameobject
        /// </summary>
        /// <returns>Clickable object if action is acceptable, null otherwise</returns>
        public Clickable CheckClickCondition()
        {
            Vector3 PlayerPosition = FindObjectOfType<PlayerController>().transform.position;
            float distanceToPlayer = Vector3.Distance(PlayerPosition, transform.position);
            if (distanceToPlayer <= minClickDistance) 
                return this;
            return null;
        }

        public bool CheckEndInteractCondition()
        {
            Vector3 playerPosition = FindObjectOfType<PlayerController>().transform.position;
            float distanceToPlayer = Vector3.Distance(playerPosition, transform.position);
            if (distanceToPlayer >= maxInteractDistance)
                return true;
            return false;
        }
    }
}

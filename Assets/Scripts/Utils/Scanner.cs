using Player;
using UnityEngine;
using Zenject;

namespace Utils
{
    [System.Serializable]
    public class PlayerScanner
    {
        [SerializeField] private float DetectionRange = 10.0f;
        [SerializeField][Range(0, 360f)] private float DetectionAngle = 90.0f;
        [SerializeField] private float MeleeDetectionRange = 2.0f;
        private PlayerController playerController;

        [Inject]
        private void Construct(PlayerController playerController)
        {
            this.playerController = playerController;
        }
        
        public float ScannerDetectionRange => DetectionRange;
        public float ScannerDetectionAngle => DetectionAngle;
        public float ScannerMeleeDetectionRange => MeleeDetectionRange;

        public PlayerController Search(Transform detector)
        {
            if (playerController == null)
                return null;

            Vector3 lookAtPlayer = playerController.transform.position - detector.position;
            lookAtPlayer.y = 0;
            float distanceToPlayer = lookAtPlayer.magnitude;

            if (distanceToPlayer <= ScannerMeleeDetectionRange)
                return playerController;

            if (distanceToPlayer <= ScannerDetectionRange)
            {
                if (Vector3.Dot(detector.forward, lookAtPlayer.normalized) > Mathf.Cos(Mathf.Deg2Rad * ScannerDetectionAngle / 2))
                    return playerController;
            }
            return null;
        }
    }
}

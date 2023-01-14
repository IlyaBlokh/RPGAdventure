using Player;
using UnityEngine;

namespace Utils
{
    [System.Serializable]
    public class PlayerScanner
    {
        [SerializeField] private float DetectionRange = 10.0f;
        [SerializeField][Range(0, 360f)] private float DetectionAngle = 90.0f;
        [SerializeField] private float MeleeDetectionRange = 2.0f;

        public float ScannerDetectionRange => DetectionRange;
        public float ScannerDetectionAngle => DetectionAngle;
        public float ScannerMeleeDetectionRange => MeleeDetectionRange;

        public PlayerController Search(Transform detector)
        {
            if (PlayerController.Instance == null)
                return null;

            Vector3 lookAtPlayer = PlayerController.Instance.transform.position - detector.position;
            lookAtPlayer.y = 0;
            float distanceToPlayer = lookAtPlayer.magnitude;

            if (distanceToPlayer <= ScannerMeleeDetectionRange)
                return PlayerController.Instance;

            if (distanceToPlayer <= ScannerDetectionRange)
            {
                if (Vector3.Dot(detector.forward, lookAtPlayer.normalized) > Mathf.Cos(Mathf.Deg2Rad * ScannerDetectionAngle / 2))
                    return PlayerController.Instance;
            }
            return null;
        }
    }
}

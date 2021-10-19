using UnityEngine;
using RPGAdventure;

[System.Serializable]
public class PlayerScanner
{
    [SerializeField]
    float DetectionRange = 10.0f;
    [SerializeField][Range(0, 360f)]
    float DetectionAngle = 90.0f;
    [SerializeField]
    float MeleeDetectionRange = 2.0f;

    public PlayerController Search(Transform detector)
    {
        if (PlayerController.Instance == null)
        {
            return null;
        }

        Vector3 m_LookAtPlayer = PlayerController.Instance.transform.position - detector.position;
        m_LookAtPlayer.y = 0;
        float m_DistanceToPlayer = m_LookAtPlayer.magnitude;

        if (m_DistanceToPlayer <= MeleeDetectionRange)
            return PlayerController.Instance;

        if (m_DistanceToPlayer <= DetectionRange)
        {
            if (Vector3.Dot(detector.forward, m_LookAtPlayer.normalized) > Mathf.Cos(Mathf.Deg2Rad * DetectionAngle / 2))
            {
                return PlayerController.Instance;
            }
        }
        return null;
    }
}

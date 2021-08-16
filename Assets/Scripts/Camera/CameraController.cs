using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform Target;

    private float m_currentRotationAngle;
    private float m_desiredRotationAngle;
    private Quaternion m_rotationQuaternion;
    private Vector3 m_rotatedPosition;

    void LateUpdate()
    {
        if (!Target) return;

        m_currentRotationAngle = transform.eulerAngles.y;
        m_desiredRotationAngle = Target.eulerAngles.y;

        m_currentRotationAngle = Mathf.LerpAngle(
            m_currentRotationAngle,
            m_desiredRotationAngle,
            0.1f);

        m_rotationQuaternion = Quaternion.Euler(.0f, m_currentRotationAngle, .0f);
        m_rotatedPosition = m_rotationQuaternion * Vector3.forward;

        transform.position = new Vector3(Target.position.x, 5.0f, Target.position.z);
        transform.position -= m_rotatedPosition * 10;
        transform.LookAt(Target);
    }
}

using Camera;
using UnityEngine;

namespace Utils
{
    public class FaceToCamera : MonoBehaviour
    {
        private CameraController m_CameraController;
        private Vector3 m_CameraDirection;
        private Quaternion m_TargetRotation;

        void Awake()
        {
            m_CameraController = UnityEngine.Camera.main.GetComponent<CameraController>();
        }

        void Update()
        {
            m_CameraDirection = Quaternion.Euler(0, m_CameraController.FreeLookCamera.m_XAxis.Value, 0) * Vector3.forward;
            m_TargetRotation = Quaternion.LookRotation(-m_CameraDirection);
            transform.rotation = m_TargetRotation;
        }
    }
}

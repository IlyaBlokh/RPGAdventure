using Camera;
using UnityEngine;
using Zenject;

namespace Utils
{
    public class FaceToCamera : MonoBehaviour
    {
        private CameraController cameraController;
        private Vector3 cameraDirection;
        private Quaternion targetRotation;

        [Inject]
        private void Construct(CameraController cameraController)
        {
            this.cameraController = cameraController;
        }

        void Update()
        {
            /*cameraDirection = Quaternion.Euler(0, cameraController.FreeLookCamera.m_XAxis.Value, 0) * Vector3.forward;
            targetRotation = Quaternion.LookRotation(-cameraDirection);
            transform.rotation = targetRotation;*/
        }
    }
}

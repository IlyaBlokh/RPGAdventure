using Cinemachine;
using UnityEngine;

namespace Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] CinemachineFreeLook freeLookCamera;
        [SerializeField] private CinemachineVirtualCamera fixedLookCamera;
        public CinemachineFreeLook FreeLookCamera => freeLookCamera;
        public CinemachineVirtualCamera FixedLookCamera => fixedLookCamera;

        public void SwitchToFixedView()
        {
            freeLookCamera.gameObject.SetActive(false);
            fixedLookCamera.gameObject.SetActive(true);
        }
 
        public void SwitchToFreeLookView()
        {
            freeLookCamera.gameObject.SetActive(true);
            fixedLookCamera.gameObject.SetActive(false);
        }
    }
}

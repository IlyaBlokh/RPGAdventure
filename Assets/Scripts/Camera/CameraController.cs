using Cinemachine;
using UnityEngine;

namespace Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera followCamera;
        [SerializeField] private CinemachineVirtualCamera fixedLookCamera;
        public CinemachineVirtualCamera FollowCamera => followCamera;
        public CinemachineVirtualCamera FixedLookCamera => fixedLookCamera;

        public void SwitchToFixedView()
        {
            followCamera.gameObject.SetActive(false);
            fixedLookCamera.gameObject.SetActive(true);
        }
 
        public void SwitchToFreeLookView()
        {
            followCamera.gameObject.SetActive(true);
            fixedLookCamera.gameObject.SetActive(false);
        }
    }
}

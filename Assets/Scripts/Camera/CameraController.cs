using Cinemachine;
using Player;
using UnityEngine;
using Zenject;

namespace Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera followCamera;
        [SerializeField] private CinemachineVirtualCamera aimCamera;

        [Inject]
        private void Construct(PlayerController playerController)
        {
        }
        public Quaternion RawOrientation => 
            followCamera.Priority > aimCamera.Priority ? followCamera.State.RawOrientation : aimCamera.State.RawOrientation;
        public void SwitchToFixedView()
        {
            followCamera.Priority = 0;
            aimCamera.Priority = 1;
        }
 
        public void SwitchToFreeLookView()
        {
            followCamera.Priority = 1;
            aimCamera.Priority = 0;
        }
    }
}

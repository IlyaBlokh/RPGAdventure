using Cinemachine;
using Player;
using UnityEngine;
using Zenject;

namespace Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera followCamera;
        [SerializeField] private CinemachineVirtualCamera zoomedCamera;
        private PlayerController playerController;

        [Inject]
        private void Construct(PlayerController playerController)
        {
            this.playerController = playerController;
        }
        public Quaternion RawOrientation => 
            followCamera.Priority > zoomedCamera.Priority ? followCamera.State.RawOrientation : zoomedCamera.State.RawOrientation;
        public void SwitchToFixedView()
        {
            followCamera.Priority = 0;
            zoomedCamera.Priority = 1;
        }
 
        public void SwitchToFreeLookView()
        {
            followCamera.Priority = 1;
            zoomedCamera.Priority = 0;
        }
    }
}

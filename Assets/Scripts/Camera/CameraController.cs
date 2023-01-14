using Cinemachine;
using UnityEngine;

namespace Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] CinemachineFreeLook freeLookCamera;
        public CinemachineFreeLook FreeLookCamera => freeLookCamera;
    }
}

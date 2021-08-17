using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    CinemachineFreeLook FreeLookCamera;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            FreeLookCamera.m_YAxis.m_MaxSpeed = 10;
            FreeLookCamera.m_XAxis.m_MaxSpeed = 650;
        }
        if (Input.GetMouseButtonUp(1))
        {
            FreeLookCamera.m_YAxis.m_MaxSpeed = 0;
            FreeLookCamera.m_XAxis.m_MaxSpeed = 0;
        }
    }
}

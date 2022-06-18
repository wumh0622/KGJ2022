using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    public CinemachineVirtualCamera camera;

    Vector3 targetOffset;
    Vector3 currentOffset;
    CinemachineTransposer transposer;

    private void Awake()
    {
        instance = this;
        transposer = camera.GetCinemachineComponent<CinemachineTransposer>();
    }

    // Update is called once per frame
    void Update()
    {
        currentOffset = Vector3.Lerp(currentOffset, targetOffset, Time.deltaTime * 5);
        transposer.m_FollowOffset = new Vector3(currentOffset.x, currentOffset.y, transposer.m_FollowOffset.z);
    }

    public void ResetCameraOffset()
    {
        targetOffset = Vector3.zero;
    }

    public void SetCameraOffset(Vector3 offset)
    {
        targetOffset = offset;
    }
}

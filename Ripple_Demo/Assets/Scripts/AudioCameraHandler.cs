using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCameraHandler : MonoBehaviour
{
    public Camera[] allCameras;
    public Transform audioListener;
    public int renderCam;
    public void SetCamera(int camNum)
    {
        allCameras[renderCam].enabled = false;
        allCameras[camNum].enabled = true;
        audioListener.parent = allCameras[camNum].transform;

        audioListener.localPosition = Vector3.zero;
        audioListener.localRotation = Quaternion.identity;

        renderCam = camNum;
    }
}

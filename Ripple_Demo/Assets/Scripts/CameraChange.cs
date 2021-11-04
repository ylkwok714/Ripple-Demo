using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    public GameObject ThirdCamera; //0
    public GameObject FirstCamera; //1
    public GameObject Stone;
    public int CameraMode;

    public bool manualChange = false;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Camera") || manualChange)
        {
            if(CameraMode == 1)
            {
                CameraMode = 0;
            }
            else
            {
                CameraMode = 1;
            }
            //Debug.Log(CameraMode);
            StartCoroutine(ChangeCameraMode(CameraMode));

        }
    }

    IEnumerator ChangeCameraMode(int mode)
    {
        yield return new WaitForSeconds(0.01f);

        if (mode == 0)
        {
            ThirdCamera.SetActive(true);
            FirstCamera.SetActive(false);
        }
        if (mode == 1)
        {
            ThirdCamera.SetActive(false);
            FirstCamera.SetActive(true);
            manualChange = false;

        }

    }
}

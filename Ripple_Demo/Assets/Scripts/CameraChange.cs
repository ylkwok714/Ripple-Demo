using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    public GameObject ThirdCamera; //0
    public GameObject FirstCamera; //1
    public int CameraMode;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Camera"))
        {
            if(CameraMode == 1)
            {
                CameraMode = 0;
            }
            else
            {
                CameraMode = 1;
            }
            Debug.Log(CameraMode);
            StartCoroutine(ChangeCameraMode());

        }
    }

    IEnumerator ChangeCameraMode()
    {
        yield return new WaitForSeconds(0.01f);

        if (CameraMode == 0)
        {
            ThirdCamera.SetActive(true);
            FirstCamera.SetActive(false);
        }
        if (CameraMode == 1)
        {
            ThirdCamera.SetActive(false);
            FirstCamera.SetActive(true);
        }
    }
}

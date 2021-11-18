using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PostCutscene : MonoBehaviour
{
    //public GameObject cameraController;
    //public GameObject oceanLevel;
    public void ReloadScene()
    {

        //SceneManager.LoadScene("RippleOceanLevel");
        //GameObject.Find("OceanLevel").SetActive(true);
        SceneManager.UnloadSceneAsync("WhaleCutscene");
        //cameraController.GetComponent<CameraChange>().ChangeCameraMode(0);

    }
}

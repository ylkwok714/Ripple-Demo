using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneData : MonoBehaviour
{
    public GameObject cameraController;

    public IEnumerator SceneTransition(int sceneNumber)
    {
        //oceanLevel.SetActive(false);
        cameraController.GetComponent<CameraChange>().CameraMode = 100;
        AsyncOperation cutsceneLoad = SceneManager.LoadSceneAsync(sceneNumber, LoadSceneMode.Additive);
        while (!cutsceneLoad.isDone)
        {
            yield return null;
        }
        Debug.Log("Cutscene finished");
        //cameraController.GetComponent<CameraChange>().ThirdCamera.SetActive(true);

        //StartCoroutine(cameraController.GetComponent<CameraChange>().ChangeCameraMode(0));
        //yield return new WaitForSeconds(2f);
        //oceanLevel.SetActive(true);
    }
}

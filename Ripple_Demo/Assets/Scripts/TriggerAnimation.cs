using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerAnimation : MonoBehaviour
{
    public int cutsceneNumber;
    public GameObject triggerParameter;
    bool hasVisited = false;

    public GameObject gameManager;
    

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Have set off trigger");
        if (!hasVisited && (collision.transform.name == triggerParameter.transform.name))
        {
            Debug.Log("Have found cutscene trigger");

            hasVisited = true;
            StartCoroutine(gameManager.GetComponent<SceneData>().SceneTransition(cutsceneNumber));
            //gameManager.GetComponent<SceneData>().SceneTransition(cutsceneNumber);

        }
    }

   
}

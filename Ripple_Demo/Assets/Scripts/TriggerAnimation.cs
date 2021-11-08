using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerAnimation : MonoBehaviour
{
    public int cutsceneNumber;
    public GameObject triggerParameter;
    bool hasVisited = false;
    

    private void OnTriggerEnter(Collider collision)
    {
        if (!hasVisited )
        {
            hasVisited = true;
            SceneManager.LoadScene(cutsceneNumber);
        }
    }
}

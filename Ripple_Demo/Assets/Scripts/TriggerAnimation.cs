using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    public Animator cutscene;
    public GameObject triggerParameter;
    bool hasVisited = false;
    

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasVisited && collision.transform.name == triggerParameter.name)
        {
            hasVisited = true;
            cutscene.SetBool(triggerParameter.name, true);
        }
    }
}

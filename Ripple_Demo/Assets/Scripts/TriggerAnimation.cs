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
    public GameObject whale;
    public GameObject audioListenerObject;
    public Animator WhaleCutsceneCameraAnimator;

    public Animator WhaleAnimator;
    public Animator TailRippleAnimator;
    public Animator WhaleRippleAnimator;
    public Animator Spout1Animator;
    public Animator Spout2Animator;
    public Animator Spout3Animator;
    public Animator Bird1Animator;
    public Animator Bird2Animator;
    public Animator Bird3Animator;
    public Animator Bird4Animator;
    public Animator Bird5Animator;
    public Animator Bird6Animator;
    public Animator Bird7Animator;
    public Animator Bird8Animator;
    public Animator Bird9Animator;
    public Animator Bird10Animator;
    public Animator Bird11Animator;
    public Animator Bird12Animator;
    public Animator Bird13Animator;
    public Animator Bird14Animator;
    public Animator Bird15Animator;
    public Animator Bird16Animator;


    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Have set off trigger");
        if (!hasVisited && (collision.transform.name == triggerParameter.transform.name))
        {
            Debug.Log("Have found cutscene trigger");

            hasVisited = true;

            //StartCoroutine(gameManager.GetComponent<SceneData>().SceneTransition(cutsceneNumber));
            //gameManager.GetComponent<SceneData>().SceneTransition(cutsceneNumber);
            if(triggerParameter.transform.name == "WhaleTrigger")
            {
                //WhaleCutsceneAnimator.SetBool("playCutscene", true);
                //GameObject w = Instantiate(whale, triggerParameter.transform);
                //StartCoroutine(PlayWhaleCutscene());
                PlayWhaleCutscene();


            }
        }
    }

    public void PlayWhaleCutscene()
    {
        //Animator whaleAnimator = whale.GetComponent<Animator>();
        //whaleAnimator.SetTrigger("playWhale");
        //whaleAnimator.Play("WhaleAnimation");
            WhaleCutsceneCameraAnimator.SetTrigger("whale");
            WhaleAnimator.SetTrigger("whale");
    TailRippleAnimator.SetTrigger("whale");
    WhaleRippleAnimator.SetTrigger("whale");
    Spout1Animator.SetTrigger("whale");
    Spout2Animator.SetTrigger("whale");
    Spout3Animator.SetTrigger("whale");
    Bird1Animator.SetTrigger("whale");
    Bird2Animator.SetTrigger("whale");
    Bird3Animator.SetTrigger("whale");
    Bird4Animator.SetTrigger("whale");
    Bird5Animator.SetTrigger("whale");
    Bird6Animator.SetTrigger("whale");
    Bird7Animator.SetTrigger("whale");
    Bird8Animator.SetTrigger("whale");
    Bird9Animator.SetTrigger("whale");
    Bird10Animator.SetTrigger("whale");
    Bird11Animator.SetTrigger("whale");
    Bird12Animator.SetTrigger("whale");
    Bird13Animator.SetTrigger("whale");
    Bird14Animator.SetTrigger("whale");
    Bird15Animator.SetTrigger("whale");
    Bird16Animator.SetTrigger("whale");


    //yield return new WaitForSeconds(30.0f);
        //Destroy(whale);
    }
   
}

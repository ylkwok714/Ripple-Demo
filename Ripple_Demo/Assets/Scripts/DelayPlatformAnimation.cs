using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayPlatformAnimation : MonoBehaviour
{
    Animator animator;
    public float delayTime = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(DelayedAnimation());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator DelayedAnimation()
    {
        yield return new WaitForSeconds(delayTime);
        animator.Play("RockRippleAnimation");

    }
}

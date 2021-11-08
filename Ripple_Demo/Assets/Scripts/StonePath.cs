using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StonePath : MonoBehaviour
{
    public GameObject objectToJump;
    private static GameObject targetPosition;
    private static int skipCode;
    public float jumpSpeed = 10.0f;

    private bool movingToTarget = false;

    // Start is called before the first frame update
    void Start()
    {
        //targetPosition = Throw.targetPlatform;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            movingToTarget = true;
        }
        if (movingToTarget)
        {
            //Vector3 newPostion = new Vector3(targetPosition.transform.position.x, targetPosition.transform.position.y, targetPosition.transform.position.z);
            //objectToJump.transform.position = Vector3.MoveTowards(objectToJump.transform.position, newPostion, Time.deltaTime * jumpSpeed);

        }
        
        //Vector3 newPostion = new Vector3(targetPosition.transform.position.x, targetPosition.transform.position.y, targetPosition.transform.position.z);
        //objectToJump.transform.position = Vector3.MoveTowards(objectToJump.transform.position, newPostion, Time.deltaTime * jumpSpeed);


    }
}

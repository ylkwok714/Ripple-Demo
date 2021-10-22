using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public float turnSpeed = 4.0f;
    public GameObject player;
    private float targetDistance;
    public float minTurnAngle = -90.0f;
    public float maxTurnAngle = 0.0f;
    private float rotationX;


    // Start is called before the first frame update
    void Start()
    {
        targetDistance = Vector3.Distance(transform.position, player.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        // get the mouse inputs
        float y = Input.GetAxis("Mouse X") * turnSpeed;
        rotationX += Input.GetAxis("Mouse Y") * turnSpeed;
        // clamp the vertical rotation
        rotationX = Mathf.Clamp(rotationX, minTurnAngle, maxTurnAngle);
        // rotate the camera
        transform.eulerAngles = new Vector3(-rotationX, transform.eulerAngles.y + y, 0);
        // move the camera position
        transform.position = player.transform.position - (transform.forward * targetDistance);
    }
}

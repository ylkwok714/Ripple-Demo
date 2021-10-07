using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]

public class Throw : MonoBehaviour
{

    private Vector3 mousePressDownPos;
    private Vector3 mouseReleasePos;

    private Vector3 StartStonePosition;
    private Quaternion StartStoneRotate;


    public float forceMultiplier = 10;
    public float spawnY = 0f;
    //public GameObject Platform;
    public GameObject[] platformOptions;
    private Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartStonePosition = transform.position;
        StartStoneRotate = transform.rotation;

    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            transform.position = StartStonePosition;
            transform.rotation = StartStoneRotate;

        }
    }

    private void OnMouseDown()
    {
        
        //mousePressDownPos = Input.mousePosition;

    }

    private void OnMouseUp()
    {
        //mouseReleasePos = Input.mousePosition;
        //Shoot(mouseReleasePos - mousePressDownPos);
        Shoot();
    }

    void Shoot()
    {

        if (rb.constraints == RigidbodyConstraints.FreezePositionY)
        {
            rb.constraints = RigidbodyConstraints.None;

        }
        //transform.position = StartStonePosition;
        //transform.rotation = StartStoneRotate;

        //rb.AddForce(new Vector3(Force.x, Force.y, Force.y) * forceMultiplier);
        rb.AddForce(0,0,forceMultiplier, ForceMode.Impulse);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == "Water")
        {
            Vector3 offsetY = new Vector3(0f, spawnY, 0f);
            int platformIndex = Random.Range(0, platformOptions.Length);
            //Debug.Log("You collided at " + collision.contacts[0].point + ".");
            Instantiate(platformOptions[platformIndex], collision.contacts[0].point + offsetY, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
        }


    }

}

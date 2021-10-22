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
    public float skipSpeed = 3.0f;
    public static GameObject targetPlatform;
    public GameObject[] platformOptions;
    private Rigidbody rb;
    Queue<Vector3> contactPoints = new Queue<Vector3>();
    public Camera firstPersonCamera;
    public Transform player;

    private float timer = 0;
    public static int throwCode;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //StartStonePosition = transform.position;
        StartStoneRotate = transform.rotation;

    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            transform.rotation = StartStoneRotate;
        }
        if (Input.GetMouseButtonDown(0))
        {
            timer = Time.time;
        }
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (contactPoints.Count > 0)
            {
                for(int i = 0; i < contactPoints.Count; i++)
                {
                    ContactPoint cp = contactPoints.Dequeue();
                    //StartCoroutine(PlayerJumping(cp));
                    PlayerJumping(cp);

                }
            }

        }*/

        if (Input.GetMouseButtonUp(0))
        {
            float timePassed = Time.time - timer;
            SkipIntensity(timePassed);
            Shoot();
        }
    }

    void SkipIntensity(float timeChange)
    {
        throwCode = (int) Mathf.Floor(timeChange);
        if(throwCode > 3)
        {
            throwCode = 3;
        }

        if(throwCode == 0)
        {
            throwCode = 1;
        }
    }
    /*
    void PlayerJumping(ContactPoint cp)
    {
        //player.transform.position = cp.point;

        float xPosition = cp.point.x;
        float zPosition = cp.point.z;
        //Vector3 offsetY = new Vector3(0f, 1.0f, 0f);

        //Debug.Log("x, z " + xPosition + ", " + zPosition);

        player.position = new Vector3(cp.point.x, cp.point.y + 1.0f, cp.point.z);

        //Debug.Log(player.position);

    }*/

    void Shoot()
    {
        int intensity = throwCode * 3;
        //Vector3 newPostion = new Vector3(firstPersonCamera.transform.forward.x+intensity, firstPersonCamera.transform.forward.y + intensity, firstPersonCamera.transform.forward.z + intensity);
        Vector3 newPosition = firstPersonCamera.transform.forward * intensity;

        if (rb.constraints == RigidbodyConstraints.FreezePositionY)
        {
            rb.constraints = RigidbodyConstraints.None;

        }
        rb.AddForce(newPosition, ForceMode.Impulse);
        //transform.position = transform.position + firstPersonCamera.transform.forward * throwCode *5 * Time.deltaTime;
        //transform.position = Vector3.MoveTowards(transform.position, newPosition, Time.deltaTime * skipSpeed);

    }

    Vector3 firstPoint;
    Vector3 currentPoint;
    private bool hasCollidedOnce = false;
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.transform.name == "Ocean" && !hasCollidedOnce)
        {
            hasCollidedOnce = true;
            Vector3 offsetY = new Vector3(0f, spawnY, 0f);
            int platformIndex = Random.Range(0, platformOptions.Length);
            firstPoint = collision.contacts[0].point;
            currentPoint = firstPoint;
            //Debug.Log("You collided at " + collision.contacts[0].point + ".");
            targetPlatform = Instantiate(platformOptions[platformIndex], collision.contacts[0].point + offsetY, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
            //GetComponent<JumpPath>().targetPosition = targetPlatform;
            contactPoints.Enqueue(collision.contacts[0].point);

            if(throwCode > 1)
            {
                SpawnPlatforms(throwCode - 1);
            }
        }


    }

    private void SpawnPlatforms(int numPlatforms)
    {

        Vector3 firstDistance = firstPoint - transform.position;
        Debug.Log(firstDistance);
        Debug.Log(throwCode);
        Vector3 rootPosition = targetPlatform.transform.position;
        Vector3 furtherPosition = new Vector3(rootPosition.x + firstDistance.x, rootPosition.y, rootPosition.z + firstDistance.z);
        int platformIndex = Random.Range(0, platformOptions.Length);

        for (int i = 0; i < numPlatforms; i++)
        {
            if(i == 0)
            {
                Instantiate(platformOptions[platformIndex], furtherPosition , Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
                contactPoints.Enqueue(furtherPosition);

            }
            else
            {
                Instantiate(platformOptions[platformIndex], furtherPosition*(i+1), Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
                contactPoints.Enqueue(furtherPosition * (i+1));

            }
        }
    }

}

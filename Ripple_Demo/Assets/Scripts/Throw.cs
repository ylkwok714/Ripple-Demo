using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]

public class Throw : MonoBehaviour
{

    private Vector3 StartStonePosition;
    private Quaternion StartStoneRotate;

    public float forceMultiplier = 10;
    public float spawnY = 0f;
    public float skipSpeed = 3.0f;
    //public static GameObject targetPlatform;
    public GameObject[] platformOptions;
    private Rigidbody rb;
    Queue<Vector3> contactPoints = new Queue<Vector3>();
    Queue<GameObject> formedPlatforms = new Queue<GameObject>();
    public Camera firstPersonCamera;
    public Transform player;
    public GameObject cameraModeController;
    public GameObject parabolaHolder;
    public Transform stoneHoldPosition;
    public GameObject skippingStone;
    public Transform oceanObjectHolder;

    private float timer = 0;
    //private 

    public static int throwCode;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //contactPoints.Enqueue(player.position);
        StartStonePosition = transform.position;
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
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //StartCoroutine(PlayerJumpingSequence());
            //Debug.Log(contactPoints.Count);
            //for(int i = 0; i < contactPoints.Count; i++)
            //{
            //Debug.Log(cpClone.Dequeue());
            //if(cpClone.Count > 0)
                PlayerJumpingSequence();

            //}
        }

        if (Input.GetMouseButtonUp(0))
        {
            float timePassed = Time.time - timer;
            SkipIntensity(timePassed);
            Shoot();
        }
        if (Input.GetButtonDown("Camera") && hasCollidedOnce)
        {
            int numToDestroy = formedPlatforms.Count;
            for(int i = 0; i < numToDestroy-1; i++)
            {
                GameObject desPlatform = formedPlatforms.Dequeue();
                Destroy(desPlatform);
            }
            contactPoints.Clear();
            GameObject newStone = Instantiate(gameObject);
            newStone.GetComponent<Renderer>().enabled = true;
            newStone.transform.SetParent(player);
            newStone.transform.position = stoneHoldPosition.position;
            hasCollidedOnce = false;
            //newStone.transform.position = stoneHoldPosition.position;
            Destroy(gameObject);
        }
    }

    void SkipIntensity(float timeChange)
    {
        throwCode = (int) Mathf.Floor(timeChange);
        if(throwCode > 5)
        {
            throwCode = 5;
        }

        if(throwCode == 0)
        {
            throwCode = 1;
        }
    }
    
    public void PlayerJumpingSequence()
    {
        //Queue<Vector3> cpClone = new Queue<Vector3>(contactPoints);
        //cpClone = new Queue<Vector3>(contactPoints);
        //contactPoints.Dequeue();
        //iterate through queue
        if (contactPoints.Count > 0)
        {
            Vector3 pointA = player.transform.position;
            Vector3 pointC = contactPoints.Dequeue();

            Vector3 pointB = (pointA + pointC) / 2;
            pointB.y += 2.0f;

            parabolaHolder.transform.GetChild(0).position = pointA;
            parabolaHolder.transform.GetChild(1).position = pointB;
            parabolaHolder.transform.GetChild(2).position = pointC;

           
            player.gameObject.GetComponent<ParabolaController>().FollowParabola();
        }
        

    }

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
        
        if (collision.transform.IsChildOf(oceanObjectHolder) && !hasCollidedOnce)
        {
            hasCollidedOnce = true;
            GetComponent<Renderer>().enabled = false;
            Vector3 offsetY = new Vector3(0f, spawnY, 0f);
            int platformIndex = Random.Range(0, platformOptions.Length);
            //Debug.Log("You collided at " + collision.contacts[0].point + ".");
            //targetPlatform = Instantiate(platformOptions[platformIndex], collision.contacts[0].point + offsetY, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
            GameObject targetPlatform = Instantiate(platformOptions[platformIndex], collision.contacts[0].point + offsetY, Quaternion.identity);
            targetPlatform.transform.Rotate(-90,0,0);
            //GetComponent<JumpPath>().targetPosition = targetPlatform;
            Transform landPos = targetPlatform.transform.Find("LandingPosition");
            //firstPoint = landPos.position;
            currentPoint = landPos.position;

            Vector3 targetLanding = new Vector3(landPos.position.x, landPos.position.y, landPos.position.z);
            formedPlatforms.Enqueue(targetPlatform);
            contactPoints.Enqueue(targetLanding);

            if(throwCode > 1)
            {
                contactPoints.Dequeue();
                formedPlatforms.Dequeue();
                SpawnPlatforms(throwCode , targetLanding);
                Destroy(targetPlatform);
            }

            //force player to go back to 3rd person camera
            cameraModeController.GetComponent<CameraChange>().manualChange = true ;
        }

        transform.position = StartStonePosition;
        rb.constraints = RigidbodyConstraints.FreezePositionY;


    }

    private void SpawnPlatforms(int numPlatforms, Vector3 targetPos)
    {
        Vector3 firstDistance = currentPoint - player.transform.position;
        //Debug.Log(firstDistance);
        Debug.Log(throwCode);
        Vector3 rootPosition = player.transform.position;
        //Vector3 furtherPosition = new Vector3(rootPosition.x + firstDistance.x, spawnY, rootPosition.z + firstDistance.z);
        Vector3 furtherPosition = new Vector3(rootPosition.x + firstDistance.x, spawnY, rootPosition.z + firstDistance.z);
        int platformIndex = Random.Range(0, platformOptions.Length);

        for (int i = 0; i < numPlatforms; i++)
        {
            Vector3 cameraDirection = firstPersonCamera.transform.forward;

            Vector3 spawnPoint = new Vector3(furtherPosition.x +(firstDistance.x*i), spawnY, furtherPosition.z + (firstDistance.z * i));
            //Vector3 spawnPoint = new Vector3(cameraDirection.x * (i + throwCode), spawnY, cameraDirection.z * (i + throwCode));
            //GameObject p = Instantiate(platformOptions[platformIndex], furtherPosition * (i + 1), Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
            GameObject p = Instantiate(platformOptions[platformIndex], spawnPoint, Quaternion.identity);
            p.transform.Rotate(-90, 0, 0);

            formedPlatforms.Enqueue(p);
            Transform landPos = p.transform.Find("LandingPosition");
            Vector3 targetLanding = new Vector3(landPos.position.x, landPos.position.y, landPos.position.z);
            //contactPoints.Enqueue(furtherPosition * (i + 1));
            contactPoints.Enqueue(targetLanding);
        }


    }

}

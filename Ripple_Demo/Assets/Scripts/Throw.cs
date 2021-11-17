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
    public GameObject stoneParabolaHolder;
    public Transform stoneHoldPosition;
    public GameObject skippingStone;
    public Transform oceanObjectHolder;
    //public GameObject fakeStoneForPathing;

    private float timer = 0;
    //private 

    public static int throwCode;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //fakeStoneForPathing.GetComponent<Renderer>().enabled = false;
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
        

        if (Input.GetKeyDown(KeyCode.Space) )
        {
           
            if(cameraModeController.GetComponent<CameraChange>().CameraMode == 1)
                cameraModeController.GetComponent<CameraChange>().manualChange = true;

            
            
            StartCoroutine(PlayerJumpingSequence());


        }

        if (Input.GetMouseButtonUp(0))
        {
            float timePassed = Time.time - timer;
            SkipIntensity(timePassed);
            Shoot();
        }
        if (Input.GetButtonDown("Camera") && hasCollidedOnce && onLastPlatform)
        {
            int numToDestroy = formedPlatforms.Count;
            for(int i = 0; i < numToDestroy-1; i++)
            {
                GameObject desPlatform = formedPlatforms.Dequeue();
                Animator anim = desPlatform.GetComponent<Animator>();
                anim.SetBool("readyToSink", true);
                Destroy(desPlatform,40f);
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
    
    public IEnumerator PlayerJumpingSequence()
    {
        //Queue<Vector3> cpClone = new Queue<Vector3>(contactPoints);
        //cpClone = new Queue<Vector3>(contactPoints);
        //contactPoints.Dequeue();
        //iterate through queue
        Animator a = player.Find("Avatar").GetComponent<Animator>();
        //a.Play("Jump_Character");
        //a.SetBool("preJump", true);
        //a.SetTrigger("jump");
        if (contactPoints.Count > 0)
        {
            Vector3 pointA = player.transform.position;
            Vector3 pointC = contactPoints.Dequeue();

            Vector3 pointB = (pointA + pointC) / 2;
            pointB.y += 2.0f;

            parabolaHolder.transform.GetChild(0).position = pointA;
            parabolaHolder.transform.GetChild(1).position = pointB;
            parabolaHolder.transform.GetChild(2).position = pointC;
            a.Play("Jump_Character");
            yield return new WaitForSeconds(1.0f);
            //if (player.transform.position != pointB)
            //{
            //    a.Play("Jump_Character");
            //    a.Play("Rise_Character");
            //}

            player.gameObject.GetComponent<ParabolaController>().FollowParabola();
            
        }
        if(contactPoints.Count == 0)
        {
            onLastPlatform = true;
        }

    }

    void Shoot()
    {
        int intensity = throwCode * 5;
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
    Vector3 stoneCurrentLocation;
    private bool hasCollidedOnce = false;
    private bool onLastPlatform = false;
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
            GameObject targetPlatform = Instantiate(platformOptions[platformIndex], collision.contacts[0].point + offsetY, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
            targetPlatform.transform.Rotate(-90,0,0);
            //GetComponent<JumpPath>().targetPosition = targetPlatform;
            Transform landPos = targetPlatform.transform.Find("LandingPosition");
            //firstPoint = landPos.position;
            currentPoint = landPos.position;
            stoneCurrentLocation = player.transform.position;

            //StoneTravelPath(currentPoint);
            Vector3 targetLanding = new Vector3(landPos.position.x, landPos.position.y, landPos.position.z);
            formedPlatforms.Enqueue(targetPlatform);
            contactPoints.Enqueue(targetLanding);

            if(throwCode >= 1)
            {
                contactPoints.Dequeue();
                formedPlatforms.Dequeue();
                Destroy(targetPlatform);
                StartCoroutine(SpawnPlatforms(throwCode , targetLanding));
                //SpawnPlatforms(throwCode , targetLanding);
            }

            //force player to go back to 3rd person camera
            //cameraModeController.GetComponent<CameraChange>().manualChange = true ;
        }

        transform.position = StartStonePosition;
        rb.constraints = RigidbodyConstraints.FreezePositionY;


    }

    private IEnumerator SpawnPlatforms(int numPlatforms, Vector3 targetPos)
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
            //StoneTravelPath(targetLanding);

            //contactPoints.Enqueue(furtherPosition * (i + 1));
            contactPoints.Enqueue(targetLanding);
            yield return new WaitForSeconds(1.0f);
        }


    }
    /*
    void StoneTravelPath(Vector3 stoneTargetLocation)
    {
        Vector3 pointA = stoneCurrentLocation;
        Vector3 pointC = stoneTargetLocation;

        Vector3 pointB = (pointA + pointC) / 2;
        pointB.y += 1.0f;

        stoneParabolaHolder.transform.GetChild(0).position = pointA;
        stoneParabolaHolder.transform.GetChild(1).position = pointB;
        stoneParabolaHolder.transform.GetChild(2).position = pointC;

        fakeStoneForPathing.GetComponent<Renderer>().enabled = true;
        fakeStoneForPathing.gameObject.GetComponent<ParabolaController>().FollowParabola();
        stoneCurrentLocation = stoneTargetLocation;
    }*/

}

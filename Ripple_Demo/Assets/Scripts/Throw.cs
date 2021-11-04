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
    public GameObject cameraModeController;
    public GameObject parabolaHolder;

    private float timer = 0;
    //private 

    public static int throwCode;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //contactPoints.Enqueue(player.position);
        StartStonePosition = transform.position;
        StartStoneRotate = transform.rotation;
        //player.gameObject.AddComponent<ParabolaController>();

        //player.gameObject.GetComponent<ParabolaController>().ParabolaRoot = null;
        //player.gameObject.GetComponent<ParabolaController>().Speed = 1.5f;


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

            //player.transform.position = contactPoints.Dequeue();
            //yield return new WaitForSeconds(3);
            //spawn empty object with start point (character current position), mid point(apex), end point(next posiiton)
            //GameObject paraRoot = new GameObject("paraRoot");
            //GameObject startPoint = new GameObject("A");
            //GameObject midPoint = new GameObject("B");
            //GameObject endPoint = new GameObject("C");
            //paraRoot.transform.position = player.position;
            //startPoint.transform.position = pointA;
            //midPoint.transform.position = pointB;
            //endPoint.transform.position = pointC;
            //startPoint.transform.SetParent(paraRoot.transform); 
            //midPoint.transform.SetParent(paraRoot.transform); 
            //endPoint.transform.SetParent(paraRoot.transform);

            //player.gameObject.AddComponent<ParabolaController>();

            //player.gameObject.GetComponent<ParabolaController>().ParabolaRoot = paraRoot;
            //player.gameObject.GetComponent<ParabolaController>().Speed = 1;
            player.gameObject.GetComponent<ParabolaController>().FollowParabola();
        }
        //player animation with parabola
        //paraRoot.GetComponent<ParabolaController>().Autostart = true;
        //delete empty game object parent
        //Destroy(paraRoot);


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
                contactPoints.Dequeue();
                SpawnPlatforms(throwCode - 1);
                Destroy(targetPlatform);
            }

            //force player to go back to 3rd person camera
            cameraModeController.GetComponent<CameraChange>().manualChange = true ;
        }

        transform.position = StartStonePosition;
        rb.constraints = RigidbodyConstraints.FreezePositionY;


    }

    private void SpawnPlatforms(int numPlatforms)
    {

        Vector3 firstDistance = firstPoint - transform.position;
        Debug.Log(firstDistance);
        Debug.Log(throwCode);
        Vector3 rootPosition = targetPlatform.transform.position;
        //Vector3 furtherPosition = new Vector3(rootPosition.x + firstDistance.x, spawnY, rootPosition.z + firstDistance.z);
        Vector3 furtherPosition = new Vector3(rootPosition.x + firstDistance.x, spawnY, rootPosition.z + firstDistance.z);
        int platformIndex = Random.Range(0, platformOptions.Length);

        for (int i = 0; i < numPlatforms; i++)
        {

            Instantiate(platformOptions[platformIndex], furtherPosition * (i + 1), Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
            contactPoints.Enqueue(furtherPosition * (i + 1));

            /*
            if(i == 0)
            {
                Instantiate(platformOptions[platformIndex], furtherPosition , Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
                contactPoints.Enqueue(furtherPosition);

            }
            else
            {
                Instantiate(platformOptions[platformIndex], furtherPosition*(i+1), Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
                contactPoints.Enqueue(furtherPosition * (i+1));

            }*/
        }


    }

}

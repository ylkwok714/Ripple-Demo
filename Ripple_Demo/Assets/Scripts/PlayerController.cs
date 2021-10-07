using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform playerCamera = null;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] bool lockCursor = true;
    [SerializeField] float walkSpeed = 6.0f;
    [SerializeField] float gravity = -13.0f;
    [SerializeField] [Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField] [Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;

    //[SerializeField] GameObject ThirdCamera; //0
    //[SerializeField] GameObject FirstCamera; //1
    //private Transform playerCamera = null;
    public int CameraMode;

    float camPitch = 0.0f;
    float velocityY = 0.0f;
    CharacterController controller = null;

    Vector2 currentDirection = Vector2.zero;
    Vector2 currentDirectionVelocity = Vector2.zero;
    Vector2 currentMouseDirection = Vector2.zero;
    Vector2 currentMouseDirectionVelocity = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
             Cursor.visible = true;
        }
    }

    // Update is called once per frame
    void Update()
    {


        MouseUpdateLook();
        UpdateMovement();
    }

    void MouseUpdateLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        currentMouseDirection = Vector2.SmoothDamp(currentMouseDirection, targetMouseDelta, ref currentMouseDirectionVelocity, mouseSmoothTime);

        camPitch -= currentMouseDirection.y * mouseSensitivity;
        camPitch = Mathf.Clamp(camPitch, -90.0f, 90f);
        playerCamera.localEulerAngles = Vector3.right * camPitch;
        transform.Rotate(Vector3.up * currentMouseDirection.x* mouseSensitivity);
    }

    void UpdateMovement()
    {
        Vector2 targetDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDirection.Normalize();

        currentDirection = Vector2.SmoothDamp(currentDirection, targetDirection, ref currentDirectionVelocity, moveSmoothTime);

        if (controller.isGrounded)
        {
            velocityY = 0.0f;
        }

        velocityY += gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDirection.y + transform.right * currentDirection.x) * walkSpeed + Vector3.up*velocityY;
        controller.Move(velocity * Time.deltaTime);
    }


}

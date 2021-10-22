using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField] Transform playerCamera = null;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] bool lockCursor = true;
    //[SerializeField] float walkSpeed = 6.0f;
    //[SerializeField] float gravity = -13.0f;
    //[SerializeField] [Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField] [Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;

    float camPitch = 0.0f;
    //float velocityY = 0.0f;
    CharacterController controller = null;

    //Vector2 currentDirection = Vector2.zero;
    //Vector2 currentDirectionVelocity = Vector2.zero;
    Vector2 currentMouseDirection = Vector2.up;
    Vector2 currentMouseDirectionVelocity = Vector2.up;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.lockState = CursorLockMode.None;

            Cursor.visible = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        ReloadLevel();
        MouseUpdateLook();
        //UpdateMovement();

    }

    void ReloadLevel()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void MouseUpdateLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        currentMouseDirection = Vector2.SmoothDamp(currentMouseDirection, targetMouseDelta, ref currentMouseDirectionVelocity, mouseSmoothTime);

        camPitch -= currentMouseDirection.y * mouseSensitivity;
        camPitch = Mathf.Clamp(camPitch, -40.0f, 0.0f);
        playerCamera.localEulerAngles = Vector3.right * camPitch;
        transform.Rotate(Vector3.up * currentMouseDirection.x * mouseSensitivity);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// From DDU Projekt 03 with minor modifications to remove extra unused features
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private float mouseSensitivity;

    [SerializeField]
    private Transform playerCamera;
   
    private Rigidbody rb;

    public static GameObject playerReference;

    private void Awake()
    {
        playerReference = gameObject;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    void Update()
    {
        // Player Movement
        float x = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        float y = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        Vector3 movementVector = transform.forward * y + transform.right*x;
        rb.velocity = new Vector3(movementVector.x,rb.velocity.y,movementVector.z);

        // Camera Rotation
        float mouseY = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseX = Input.GetAxis("Mouse Y") * mouseSensitivity;
        transform.Rotate(0,mouseY,0);
        playerCamera.localEulerAngles = playerCamera.localEulerAngles + new Vector3(-mouseX,0,0);
        if(playerCamera.eulerAngles.z > 100) 
        {
            playerCamera.eulerAngles = new Vector3(playerCamera.eulerAngles.x, playerCamera.eulerAngles.y - 180, 0); // Prevents the camera from going crazy if you move the camera too far up
        }
        
    }
}

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasicMovement : MonoBehaviour
{
    public Vector2 moveInput;
    public Vector2 lookInput;
    public Vector3 movement;
    public bool menuInteracting;
    [SerializeField] private CharacterController charCon;
    public Camera playerCam;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float lookSpeed;
    [SerializeField] private Vector2 lookDir;
    [SerializeField] private float gravity;
    [SerializeField] private float vertSpeed;
    [SerializeField] private float jumpForce;
    

    private void Start()
    {
        charCon = GetComponent<CharacterController>();
    }
    
    private void FixedUpdate()
    {
        VertForceCalc();
        Movement();
    }

    private void Update()
    {
        CameraManagement();
    }
    

    private void Movement()
    {
        moveInput = InputSystem.actions["Move"].ReadValue<Vector2>();
        float speed = Mathf.Lerp(walkSpeed, runSpeed, InputSystem.actions["Sprint"].ReadValue<float>());
        float moveForwards = moveInput.y * speed * Time.deltaTime;
        float moveSide = moveInput.x * speed * Time.deltaTime;
        movement = (transform.forward*moveForwards)+(transform.right*moveSide)
            +vertSpeed*transform.up;
        charCon.Move(movement);
    }

    private float VertForceCalc()
    {
        if (charCon.isGrounded) //Holy shit game changer
        {
            vertSpeed = 0f;
            vertSpeed += InputSystem.actions["Jump"].ReadValue<float>()*jumpForce;
        }
        else
        {
            vertSpeed -= gravity * Time.deltaTime;
        }
        return vertSpeed;
    }
    
    void CameraManagement()
    {
        if (!menuInteracting)
        {
            lookInput = InputSystem.actions["Look"].ReadValue<Vector2>();
            lookDir.x += lookInput.x * Time.deltaTime * lookSpeed;
            lookDir.y -= lookInput.y * Time.deltaTime * lookSpeed;
            lookDir.y = Mathf.Clamp(lookDir.y, -90f, 90f);
            transform.localRotation = Quaternion.Euler(0f, lookDir.x, 0f);
            playerCam.transform.localRotation = Quaternion.Euler(lookDir.y, 0f, 0f);
        }
    }

    public void MenuInteraction(bool interacting)
    {
        menuInteracting = interacting;
        if (interacting)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Script Ref")]
    InputManager inputManager;

    [Header("Movement")]
    Vector3 moveDirection;
    public Transform camObject;
    Rigidbody playerRigidbody;
    public float walkingSpeed = 2f;
    public float runningSpeed = 5f;

    public bool isMoving;
    public bool isRunning;
    public float rotationSpeed = 12f;

    public bool isGrounded;

    [Header("Gravity")]
    public float gravity = -40f;
    public float fallSpeed = 4f;


    void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    public void HandleAllMovement()
    {
        HandleMovement();
        HandleRotation();
        ApplyGravity();
    }

    void FixedUpdate()
    {
        HandleAllMovement();
    }

    void HandleMovement()
    {
        moveDirection = camObject.forward * inputManager.verticalInput;
        moveDirection = moveDirection + camObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
        
        if (isRunning)
        {
        moveDirection *= runningSpeed;
        }
        else
        {
        moveDirection *= walkingSpeed;
        }

        Vector3 movementVelocity = moveDirection;
        movementVelocity.y = playerRigidbody.velocity.y;
        playerRigidbody.velocity = movementVelocity;
    }

    
    void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = camObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + camObject.right * inputManager.horizontalInput;
        
        targetDirection.y = 0;
        
            if (targetDirection == Vector3.zero)
        {
          return;
        }

        targetDirection.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;

    }

    void ApplyGravity()
    {
        if (!isGrounded)
        {
            Vector3 currentVelocity = playerRigidbody.velocity;
            currentVelocity.y += gravity * fallSpeed * Time.deltaTime;
            playerRigidbody.velocity = currentVelocity;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

}
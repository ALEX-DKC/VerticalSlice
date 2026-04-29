using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Playercontrol playercontrols;

    AnimatorManager animatorManager;
    PlayerMovement playerMovement;

    private float moveAmount;

    public Vector2 movenmentInput;

    public float verticalInput;
    public float horizontalInput;

    private Vector2 cameraInput;
    public float CameraInputX;
    public float CameraInputY;

    [Header("Input Button Flag")]
    public bool shiftInput;
    public bool shootInput;
    public bool scopeInput;
    public bool reloadInput;
    public bool changeRifleInput;
    public bool pauseInput;
    public bool canMove = true;

    void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        if (playercontrols == null)
        {
            playercontrols = new Playercontrol();

            playercontrols.PlayerMovement.Movement.performed += ctx => movenmentInput = ctx.ReadValue<Vector2>();
            playercontrols.PlayerMovement.Movement.canceled += ctx => movenmentInput = Vector2.zero;

            playercontrols.PlayerMovement.CameraMovement.performed += ctx => cameraInput = ctx.ReadValue<Vector2>();
            playercontrols.PlayerMovement.CameraMovement.canceled += ctx => cameraInput = Vector2.zero;

            playercontrols.PlayerActions.Shift.performed += ctx => shiftInput = true;
            playercontrols.PlayerActions.Shift.canceled += ctx => shiftInput = false;

            playercontrols.PlayerActions.Shoot.performed += ctx => shootInput = true;
            playercontrols.PlayerActions.Shoot.canceled += ctx => shootInput = false;

            playercontrols.PlayerActions.Scope.performed += ctx => scopeInput = true;
            playercontrols.PlayerActions.Scope.canceled += ctx => scopeInput = false;

            playercontrols.PlayerActions.Reload.performed += ctx => reloadInput = true;
            playercontrols.PlayerActions.Reload.canceled += ctx => reloadInput = false;

            playercontrols.PlayerActions.C.performed += ctx => changeRifleInput = true;

            playercontrols.PlayerActions.Pause.performed += ctx => pauseInput = true;
        }

        playercontrols.Enable();
    }

    private void OnDisable()
    {
        playercontrols.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
        HandleChangeRifleInput();
        HandlePauseInput();
    }

    private void HandleMovementInput()
    {
        if (canMove)
    {
        verticalInput = movenmentInput.y;
        horizontalInput = movenmentInput.x;
    }
    else
    {
        verticalInput = 0;
        horizontalInput = 0;
    }
        CameraInputX = cameraInput.x;
        CameraInputY = cameraInput.y;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));

        animatorManager.UpdateAnimValues(0, moveAmount, playerMovement.isRunning);
    }

    private void HandleSprintingInput()
    {
        if (shiftInput && moveAmount > 0.5f)
        {
            playerMovement.isRunning = true;
        }
        else
        {
            playerMovement.isRunning = false;
        }
    }

    private void HandleChangeRifleInput()
    {
        if (changeRifleInput)
        {
            changeRifleInput = false;
        }
    }

    private void HandlePauseInput()
    {
        if (pauseInput)
        {
            pauseInput = false;
        }
    }
}
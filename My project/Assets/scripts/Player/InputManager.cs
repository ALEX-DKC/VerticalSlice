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
    public bool pauseInput;
    public bool canMove = true;

    [Header("Reload Cooldown")]
    public float reloadInputCooldown = 2f;
    private float nextReloadInputTime = 0f;

    [Header("Assassination")]
    public bool assassinateInput;
    public bool isUnarmedState = true;

    [Header("Weapon Select")]
    public bool unarmedInput;
    public bool pistolInput;
    public bool rifleInput;

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

            playercontrols.PlayerActions.Scope.performed += ctx => scopeInput = true;
            playercontrols.PlayerActions.Scope.canceled += ctx => scopeInput = false;

            playercontrols.PlayerActions.Reload.performed += ctx =>
            {
                if (Time.time >= nextReloadInputTime)
                {
                    reloadInput = true;
                    nextReloadInputTime = Time.time + reloadInputCooldown;
                    Debug.Log("Reload input accepted.");
                }
                else
                {
                    reloadInput = false;
                    Debug.Log("Reload input ignored because it is on cooldown.");
                }
            };

            playercontrols.PlayerActions.SelectUnarmed.performed += ctx =>
            {
                unarmedInput = true;
                ClearShootInput();
            };

            playercontrols.PlayerActions.SelectPistol.performed += ctx =>
            {
                pistolInput = true;
                ClearShootInput();
            };

            playercontrols.PlayerActions.SelectRifle.performed += ctx =>
            {
                rifleInput = true;
                ClearShootInput();
            };

            playercontrols.PlayerActions.Assassinate.performed += ctx => assassinateInput = true;

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

        if (animatorManager != null && playerMovement != null)
        {
            animatorManager.UpdateAnimValues(horizontalInput, verticalInput, playerMovement.isRunning);
        }

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
            verticalInput = 0f;
            horizontalInput = 0f;
        }

        CameraInputX = cameraInput.x;
        CameraInputY = cameraInput.y;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
    }

    private void HandleSprintingInput()
    {
        if (playerMovement == null) return;

        if (canMove && shiftInput && moveAmount > 0.5f)
        {
            playerMovement.isRunning = true;
        }
        else
        {
            playerMovement.isRunning = false;
        }
    }

    private void HandlePauseInput()
    {
        if (pauseInput)
        {
            pauseInput = false;
        }
    }

    private void ClearShootInput()
    {
        shootInput = false;
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
    }

    public bool IsAiming()
    {
        return scopeInput;
    }

    public void ResetAimInput()
    {
        scopeInput = false;
    }

    public bool IsReloadPressed()
    {
        return reloadInput;
    }

    public void ResetReloadInput()
    {
        reloadInput = false;
    }

    public bool IsShootPressed()
    {
        if (!shootInput)
        {
            return false;
        }

        if (!scopeInput)
        {
            shootInput = false;
            return false;
        }

        return true;
    }

    public void ResetShootInput()
    {
        shootInput = false;
    }

    public bool IsUnarmedPressed()
    {
        return unarmedInput;
    }

    public bool IsPistolPressed()
    {
        return pistolInput;
    }

    public bool IsRiflePressed()
    {
        return rifleInput;
    }

    public void ResetUnarmedInput()
    {
        unarmedInput = false;
        ClearShootInput();
    }

    public void ResetPistolInput()
    {
        pistolInput = false;
        ClearShootInput();
    }

    public void ResetRifleInput()
    {
        rifleInput = false;
        ClearShootInput();
    }

    public bool HasMovementInput()
    {
        return Mathf.Abs(movenmentInput.x) > 0.1f || Mathf.Abs(movenmentInput.y) > 0.1f;
    }

    public bool IsAssassinatePressed()
    {
        return assassinateInput;
    }

    public void ResetAssassinateInput()
    {
        assassinateInput = false;
    }

    public void SetUnarmedState(bool value)
    {
        isUnarmedState = value;
    }

    public bool IsUnarmedState()
    {
        return isUnarmedState;
    }
}
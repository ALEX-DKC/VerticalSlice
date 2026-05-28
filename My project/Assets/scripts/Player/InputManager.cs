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
    public bool scopeInput;          // 右键按住
    public bool reloadInput;         // R
    public bool pauseInput;
    public bool canMove = true;

    [Header("Assassination")]
    public bool assassinateInput;    // F
    public bool isUnarmedState = true;

    [Header("Weapon Select")]
    public bool unarmedInput;        // 1
    public bool pistolInput;         // 2
    public bool rifleInput;          // 3

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

            playercontrols.PlayerActions.Reload.performed += ctx => reloadInput = true;

            playercontrols.PlayerActions.SelectUnarmed.performed += ctx => unarmedInput = true;
            playercontrols.PlayerActions.SelectPistol.performed += ctx => pistolInput = true;
            playercontrols.PlayerActions.SelectRifle.performed += ctx => rifleInput = true;

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

    // ===== 给 Visual Scripting 用的方法 =====

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
        return shootInput;
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
    }

    public void ResetPistolInput()
    {
        pistolInput = false;
    }

    public void ResetRifleInput()
    {
        rifleInput = false;
    }

    public bool HasMovementInput()
    {
        return Mathf.Abs(movenmentInput.x) > 0.1f || Mathf.Abs(movenmentInput.y) > 0.1f;
    }

    // ===== Assassination Methods =====

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
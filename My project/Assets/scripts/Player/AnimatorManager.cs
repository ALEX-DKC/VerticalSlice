using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimatorManager : MonoBehaviour
{
    private Animator animator;

    private int horizontal;
    private int vertical;
    private int isAiming;
    private int rifleAiming;
    private int moveAmount;
    private int reload;
    private int rifleReload;
    private int shootTrigger;
    private int rifleTrigger;

    // NEW: assassination trigger
    private int assassinate;

    void Awake()
    {
        animator = GetComponent<Animator>();

        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
        moveAmount = Animator.StringToHash("moveAmount");

        isAiming = Animator.StringToHash("isAiming");
        rifleAiming = Animator.StringToHash("rifleAiming");

        shootTrigger = Animator.StringToHash("ShootTrigger");
        rifleTrigger = Animator.StringToHash("RifleTrigger");

        reload = Animator.StringToHash("Reload");
        rifleReload = Animator.StringToHash("rifleReload");

        // NEW
        assassinate = Animator.StringToHash("Assassinate");
    }

    public void UpdateAnimValues(float horizontalMovement, float verticalMovement, bool isSprinting)
    {
        float snappedHorizontal;
        float snappedVertical;

        if (horizontalMovement > 0f && horizontalMovement < 0.55f)
            snappedHorizontal = 0.5f;
        else if (horizontalMovement > 0.55f)
            snappedHorizontal = 1f;
        else if (horizontalMovement < 0f && horizontalMovement > -0.55f)
            snappedHorizontal = -0.5f;
        else if (horizontalMovement < -0.55f)
            snappedHorizontal = -1f;
        else
            snappedHorizontal = 0f;

        if (verticalMovement > 0f && verticalMovement < 0.55f)
            snappedVertical = 0.5f;
        else if (verticalMovement > 0.55f)
            snappedVertical = 1f;
        else if (verticalMovement < 0f && verticalMovement > -0.55f)
            snappedVertical = -0.5f;
        else if (verticalMovement < -0.55f)
            snappedVertical = -1f;
        else
            snappedVertical = 0f;

        if (isSprinting)
        {
            snappedHorizontal = horizontalMovement;
            snappedVertical = 2f;
        }

        animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
        animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);

        bool isMoving = Mathf.Abs(horizontalMovement) > 0.1f || Mathf.Abs(verticalMovement) > 0.1f;
        animator.SetBool(moveAmount, isMoving);
    }

    public void SetAiming(bool value)
    {
        animator.SetBool(isAiming, value);
    }

    public void SetRifleAiming(bool value)
    {
        animator.SetBool(rifleAiming, value);
    }

    public void TriggerShoot()
    {
        animator.SetTrigger(shootTrigger);
    }

    public void TriggerRifleShoot()
    {
        animator.SetTrigger(rifleTrigger);
    }

    public void TriggerReload()
    {
        animator.SetTrigger(reload);
    }

    public void TriggerRifleReload()
    {
        animator.SetTrigger(rifleReload);
    }

    // NEW: call this when player presses F
    public void TriggerAssassinate()
    {
        animator.SetTrigger(assassinate);
    }
}
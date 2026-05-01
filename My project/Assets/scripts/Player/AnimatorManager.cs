using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    Animator animator;

    private int horizontal;
    private int vertical;
    private int isAiming;

    void Awake()
    {
        animator = GetComponent<Animator>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
        isAiming = Animator.StringToHash("isAiming");
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
        else if (verticalMovement > 0f)
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
    }

    public void SetAiming(bool value)
    {
        animator.SetBool(isAiming, value);
    }
}

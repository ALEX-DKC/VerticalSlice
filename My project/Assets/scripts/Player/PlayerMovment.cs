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
    Animator animator;

    [Header("Damage Post Effect")]
    public DamagePostEffectController damagePostEffectController;

    [Header("Game Over")]
    public GameOverManager gameOverManager;

    [Header("Health")]
    public float characterHealth = 9f;
    public float presentHealth;

    [Header("Movement")]
    Vector3 moveDirection;
    public Transform camObject;
    Rigidbody playerRigidbody;
    public float walkingSpeed = 2f;
    public float runningSpeed = 5f;

    public bool isMoving;
    public bool isRunning;
    public float rotationSpeed = 12f;
    public float aimingRotationSpeed = 15f;

    public bool isGrounded;

    [Header("Gravity")]
    public float gravity = -40f;
    public float fallSpeed = 4f;

    [Header("State")]
    public bool isDead = false;

    void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        presentHealth = characterHealth;
    }

    void Update()
    {
        if (camObject != null)
        {
            Debug.DrawRay(camObject.position, camObject.forward * 100f, Color.red);
        }

        // 测试死亡：按 K 直接死亡
        if (Input.GetKeyDown(KeyCode.K))
        {
            characterHitDamage(999f);
        }

        // 测试受伤：按 H 掉 1 格血
        if (Input.GetKeyDown(KeyCode.H))
        {
            characterHitDamage(1f);
        }
    }

    void FixedUpdate()
    {
        HandleAllMovement();
    }

    public void HandleAllMovement()
    {
        if (isDead)
        {
            return;
        }

        HandleMovement();

        if (inputManager != null && inputManager.IsAiming())
        {
            HandleAimingRotation();
        }
        else
        {
            HandleRotation();
        }

        ApplyGravity();
    }

    void HandleMovement()
    {
        if (inputManager == null || camObject == null || playerRigidbody == null)
        {
            return;
        }

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
        if (inputManager == null || camObject == null)
        {
            return;
        }

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
        Quaternion playerRotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );

        transform.rotation = playerRotation;
    }

    void HandleAimingRotation()
    {
        if (camObject == null)
        {
            return;
        }

        Vector3 targetDirection = camObject.forward;
        targetDirection.y = 0f;

        if (targetDirection == Vector3.zero)
        {
            return;
        }

        targetDirection.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            aimingRotationSpeed * Time.deltaTime
        );

        transform.rotation = playerRotation;
    }

    void ApplyGravity()
    {
        if (playerRigidbody == null)
        {
            return;
        }

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

    public void characterHitDamage(float takeDamage)
    {
        if (isDead)
        {
            return;
        }

        presentHealth -= takeDamage;
        presentHealth = Mathf.Clamp(presentHealth, 0f, characterHealth);

        Debug.Log("Player took damage: " + takeDamage);
        Debug.Log("Player current health: " + presentHealth);

        // 玩家被打音效
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayPlayerHit();
        }

        // 玩家受伤屏幕特效
        if (damagePostEffectController != null)
        {
            damagePostEffectController.TriggerDamageEffect();
        }

        if (presentHealth <= 0)
        {
            presentHealth = 0;
            isDead = true;

            // 玩家死亡音效
            if (SoundManager.instance != null)
            {
                SoundManager.instance.PlayPlayerDeath();
            }

            if (animator != null)
            {
                animator.SetBool("Die", true);
            }

            characterDie();
        }
    }

    void characterDie()
    {
        Debug.Log("Player Died");

        if (inputManager != null)
        {
            inputManager.SetCanMove(false);
        }

        isRunning = false;

        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector3.zero;
        }

        if (gameOverManager != null)
        {
            gameOverManager.ShowGameOver();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Character Info")]
    public float movingSpeed = 2f;
    public float runningSpeed = 8f;
    public float turningSpeed = 300f;

    [Header("Health")]
    public float maxHealth = 300f;
    public float presentHealth;

    [Header("Destination Var")]
    public Animator animator;
    public List<Transform> waypoints;
    private int currentWaypointIndex = 0;

    [Header("Boss AI")]
    public GameObject playerBody;
    public LayerMask PlayerLayer;

    public float visionRadius = 12f;
    public float visionAngle = 90f;

    public bool isAlerted = false;
    public bool playerDetected = false;
    public bool playerInVisionRadius = false;

    [Header("Melee Attack")]
    public float attackRange = 2.2f;
    public float attackDamage = 25f;
    public float timeBetweenAttack = 1.2f;
    private bool alreadyAttacked = false;

    [Header("Character Controller and Gravity")]
    public CharacterController characterController;
    public float gravity = 9.81f;
    private Vector3 velocity;

    [Header("State")]
    public bool isDead = false;

    void Start()
    {
        // 游戏开始时，当前血量 = 最大血量
        presentHealth = maxHealth;

        playerBody = GameObject.Find("Player");

        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (characterController == null)
        {
            Debug.LogWarning("Boss: No CharacterController found on " + gameObject.name);
        }

        if (animator == null)
        {
            Debug.LogWarning("Boss: No Animator found on " + gameObject.name);
        }
    }

    void Update()
    {
        if (isDead)
        {
            SetAnimatorBool("Walk", false);
            SetAnimatorBool("Run", false);
            SetAnimatorBool("Attack", false);
            return;
        }

        ApplyGravity();

        playerInVisionRadius = CanSeePlayer();

        if (playerInVisionRadius)
        {
            isAlerted = true;
            playerDetected = true;
        }

        if (!isAlerted)
        {
            Patrol();
        }
        else
        {
            ChaseOrAttackPlayer();
        }
    }

    bool CanSeePlayer()
    {
        if (playerBody == null) return false;

        Vector3 eyePosition = transform.position + Vector3.up * 1.5f;
        Vector3 playerPosition = playerBody.transform.position + Vector3.up * 1.0f;

        Vector3 directionToPlayer = playerPosition - eyePosition;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > visionRadius)
        {
            return false;
        }

        Vector3 flatDirection = directionToPlayer;
        flatDirection.y = 0f;

        float angle = Vector3.Angle(transform.forward, flatDirection);

        if (angle > visionAngle * 0.5f)
        {
            return false;
        }

        if (Physics.Raycast(eyePosition, directionToPlayer.normalized, out RaycastHit hit, visionRadius))
        {
            if (hit.transform.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    void Patrol()
    {
        if (waypoints == null || waypoints.Count == 0) return;
        if (characterController == null) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];

        Vector3 directionToWaypoint = targetWaypoint.position - transform.position;
        directionToWaypoint.y = 0f;

        float distanceToWaypoint = directionToWaypoint.magnitude;

        if (distanceToWaypoint > 0.05f)
        {
            directionToWaypoint.Normalize();

            Vector3 moveVector = directionToWaypoint * movingSpeed * Time.deltaTime;
            characterController.Move(moveVector);

            RotateToward(directionToWaypoint);
        }

        SetAnimatorBool("Walk", true);
        SetAnimatorBool("Run", false);
        SetAnimatorBool("Attack", false);

        if (distanceToWaypoint < 0.3f)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Count)
            {
                currentWaypointIndex = 0;
            }
        }
    }

    void ChaseOrAttackPlayer()
    {
        if (playerBody == null) return;
        if (characterController == null) return;

        Vector3 directionToPlayer = playerBody.transform.position - transform.position;
        directionToPlayer.y = 0f;

        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer < 0.01f)
        {
            return;
        }

        directionToPlayer.Normalize();

        RotateToward(directionToPlayer);

        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
        else
        {
            ChasePlayer(directionToPlayer);
        }
    }

    void ChasePlayer(Vector3 directionToPlayer)
    {
        Vector3 moveVector = directionToPlayer * runningSpeed * Time.deltaTime;
        characterController.Move(moveVector);

        SetAnimatorBool("Walk", false);
        SetAnimatorBool("Run", true);
        SetAnimatorBool("Attack", false);
    }

    void AttackPlayer()
    {
        SetAnimatorBool("Walk", false);
        SetAnimatorBool("Run", false);
        SetAnimatorBool("Attack", true);

        if (!alreadyAttacked)
        {
            if (SoundManager.instance != null)
            {
                SoundManager.instance.PlayBossPunch();
            }

            PlayerMovement player = playerBody.GetComponent<PlayerMovement>();

            if (player == null)
            {
                player = playerBody.GetComponentInParent<PlayerMovement>();
            }

            if (player != null)
            {
                player.characterHitDamage(attackDamage);
                Debug.Log("Boss punched player for " + attackDamage + " damage.");
            }

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttack);
        }
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void characterHitDamage(float takeDamage)
    {
        if (isDead)
        {
            return;
        }

        Debug.Log("Boss took damage: " + takeDamage);

        isAlerted = true;
        playerDetected = true;

        presentHealth -= takeDamage;
        presentHealth = Mathf.Clamp(presentHealth, 0f, maxHealth);

        Debug.Log("Boss current health: " + presentHealth);

        if (presentHealth <= 0)
        {
            BossDie();
        }
        else
        {
            if (SoundManager.instance != null)
            {
                SoundManager.instance.PlayBossHit();
            }
        }
    }

    void BossDie()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
        presentHealth = 0f;

        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayBossDeath();
        }

        Debug.Log("Boss died");

        SetAnimatorBool("Walk", false);
        SetAnimatorBool("Run", false);
        SetAnimatorBool("Attack", false);
        SetAnimatorBool("Die", true);

        characterDie();
    }

    void characterDie()
    {
        isAlerted = false;
        playerDetected = false;
        playerInVisionRadius = false;

        CancelInvoke(nameof(ResetAttack));
        alreadyAttacked = true;

        if (characterController != null)
        {
            characterController.enabled = false;
        }
    }

    void RotateToward(Vector3 direction)
    {
        if (direction == Vector3.zero) return;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(direction),
            Time.deltaTime * turningSpeed
        );
    }

    void SetAnimatorBool(string parameterName, bool value)
    {
        if (animator != null)
        {
            animator.SetBool(parameterName, value);
        }
    }

    void ApplyGravity()
    {
        if (characterController == null) return;
        if (!characterController.enabled) return;

        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y -= gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}
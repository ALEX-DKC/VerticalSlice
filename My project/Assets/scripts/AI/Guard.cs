using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Guard : MonoBehaviour
{
    [Header("Character Info")]
    public float movingSpeed;
    public float runningSpeed;
    private float CurrentmovingSpeed;
    public float turningSpeed = 300f;

    [Header("Health")]
    public float maxHealth = 100f;
    public float presentHealth;

    [Header("Destination Var")]
    public Animator animator;
    public List<Transform> waypoints;
    private int currentWaypointIndex = 0;
    private bool movingForward = true;

    [Header("Guard AI")]
    public GameObject playerBody;
    public LayerMask PlayerLayer;
    public float visionRadius;
    public float visionAngle = 60f;
    public float shootingRadius;
    public bool playerInvisionRadius;
    public bool playerInshootingRadius;
    public bool playerDetected = false;

    [Header("Guard Shooting Var")]
    public float giveDamageOf = 3f;
    public float shootingRange = 100f;
    public GameObject ShootingRaycastArea;
    public float timebtwShoot = 1.5f;
    bool previouslyShoot;

    [Header("Character Controller and Gravity")]
    public CharacterController characterController;
    public float gravity = 9.81f;
    private Vector3 velocity;

    [Header("State")]
    public bool isAlerted = false;
    public bool isDead = false;

    void Start()
    {
        CurrentmovingSpeed = movingSpeed;

        // 游戏开始时：当前血量 = 最大血量
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
            Debug.LogWarning("Guard: No CharacterController found on " + gameObject.name);
        }

        if (animator == null)
        {
            Debug.LogWarning("Guard: No Animator found on " + gameObject.name);
        }
    }

    void Update()
    {
        if (isDead)
        {
            SetAnimatorBool("Walk", false);
            SetAnimatorBool("Run", false);
            SetAnimatorBool("Shoot", false);
            return;
        }

        ApplyGravity();

        playerInvisionRadius = CanSeePlayer();
        playerInshootingRadius = Physics.CheckSphere(transform.position, shootingRadius, PlayerLayer);

        if (playerInvisionRadius)
        {
            isAlerted = true;
            playerDetected = true;
        }

        if (!isAlerted)
        {
            Walk();
        }
        else
        {
            if (playerInshootingRadius)
            {
                ShootPlayer();
            }
            else if (playerDetected)
            {
                ChasePlayer();
            }
        }
    }

    void SetAnimatorBool(string parameterName, bool value)
    {
        if (animator != null)
        {
            animator.SetBool(parameterName, value);
        }
    }

    bool CanSeePlayer()
    {
        if (isDead) return false;
        if (playerBody == null) return false;

        Vector3 eyePosition = transform.position + Vector3.up * 1.5f;
        Vector3 playerPosition = playerBody.transform.position + Vector3.up * 1.0f;

        Vector3 directionToPlayer = playerPosition - eyePosition;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > visionRadius)
        {
            return false;
        }

        Vector3 flatDirectionToPlayer = directionToPlayer;
        flatDirectionToPlayer.y = 0f;

        float angle = Vector3.Angle(transform.forward, flatDirectionToPlayer);

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

    private void Walk()
    {
        if (isDead) return;
        if (characterController == null) return;
        if (waypoints == null || waypoints.Count == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];

        Vector3 directionToWaypoint = targetWaypoint.position - transform.position;
        directionToWaypoint.y = 0f;

        float distanceToWaypoint = directionToWaypoint.magnitude;

        if (distanceToWaypoint > 0.01f)
        {
            directionToWaypoint.Normalize();

            Vector3 moveVector = directionToWaypoint * movingSpeed * Time.deltaTime;
            characterController.Move(moveVector);

            Vector3 lookDirection = new Vector3(directionToWaypoint.x, 0, directionToWaypoint.z);

            if (lookDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(lookDirection),
                    Time.deltaTime * turningSpeed
                );
            }
        }

        SetAnimatorBool("Run", false);
        SetAnimatorBool("Walk", true);
        SetAnimatorBool("Shoot", false);

        if (distanceToWaypoint < 0.2f)
        {
            if (movingForward)
            {
                currentWaypointIndex++;

                if (currentWaypointIndex >= waypoints.Count)
                {
                    currentWaypointIndex = waypoints.Count - 1;
                    movingForward = false;
                }
            }
            else
            {
                currentWaypointIndex--;

                if (currentWaypointIndex < 0)
                {
                    currentWaypointIndex = 0;
                    movingForward = true;
                }
            }
        }
    }

    void ChasePlayer()
    {
        if (isDead) return;
        if (playerBody == null) return;
        if (characterController == null) return;

        CurrentmovingSpeed = runningSpeed;

        Vector3 directionToPlayer = playerBody.transform.position - transform.position;
        directionToPlayer.y = 0f;

        if (directionToPlayer.magnitude < 0.01f)
        {
            return;
        }

        directionToPlayer.Normalize();

        Vector3 moveVector = directionToPlayer * CurrentmovingSpeed * Time.deltaTime;
        characterController.Move(moveVector);

        Vector3 lookDirection = new Vector3(directionToPlayer.x, 0, directionToPlayer.z);

        if (lookDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(lookDirection),
                Time.deltaTime * turningSpeed
            );
        }

        SetAnimatorBool("Run", true);
        SetAnimatorBool("Walk", false);
        SetAnimatorBool("Shoot", false);
    }

    void ShootPlayer()
    {
        if (isDead) return;
        if (playerBody == null) return;
        if (ShootingRaycastArea == null) return;

        CurrentmovingSpeed = 0f;

        Vector3 directionToPlayer = playerBody.transform.position - transform.position;
        directionToPlayer.y = 0f;

        if (directionToPlayer.magnitude > 0.01f)
        {
            directionToPlayer.Normalize();

            Vector3 lookDirection = new Vector3(directionToPlayer.x, 0, directionToPlayer.z);

            if (lookDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(lookDirection),
                    Time.deltaTime * turningSpeed
                );
            }
        }

        SetAnimatorBool("Run", false);
        SetAnimatorBool("Walk", false);
        SetAnimatorBool("Shoot", true);

        if (!previouslyShoot)
        {
            PlayEnemyShootSound();

            RaycastHit hit;

            if (Physics.Raycast(ShootingRaycastArea.transform.position, ShootingRaycastArea.transform.forward, out hit, shootingRange))
            {
                Debug.Log("Guard Hit " + hit.transform.name);

                PlayerMovement player = hit.transform.GetComponent<PlayerMovement>();

                if (player == null)
                {
                    player = hit.transform.GetComponentInParent<PlayerMovement>();
                }

                if (player != null)
                {
                    player.characterHitDamage(giveDamageOf);
                }
            }

            previouslyShoot = true;
            Invoke(nameof(ActiveShooting), timebtwShoot);
        }
    }

    void PlayEnemyShootSound()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayEnemyRifleShoot();
        }
        else
        {
            Debug.LogWarning("Guard: SoundManager instance is missing.");
        }
    }

    void PlayGuardHitSound()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayGuardHit();
        }
        else
        {
            Debug.LogWarning("Guard: SoundManager instance is missing.");
        }
    }

    void PlayGuardDeathSound()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayGuardDeath();
        }
        else
        {
            Debug.LogWarning("Guard: SoundManager instance is missing.");
        }
    }

    private void ActiveShooting()
    {
        previouslyShoot = false;
    }

    public void characterHitDamage(float takeDamage)
    {
        if (isDead)
        {
            return;
        }

        Debug.Log("Guard took damage: " + takeDamage);

        isAlerted = true;
        playerDetected = true;

        presentHealth -= takeDamage;
        presentHealth = Mathf.Clamp(presentHealth, 0f, maxHealth);

        Debug.Log("Guard current health: " + presentHealth);

        if (presentHealth <= 0)
        {
            GuardDie();
        }
        else
        {
            PlayGuardHitSound();
        }
    }

    void GuardDie()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
        presentHealth = 0f;

        PlayGuardDeathSound();

        Debug.Log("Guard died");

        SetAnimatorBool("Walk", false);
        SetAnimatorBool("Run", false);
        SetAnimatorBool("Shoot", false);
        SetAnimatorBool("Die", true);

        characterDie();
    }

    void characterDie()
    {
        CurrentmovingSpeed = 0f;
        shootingRange = 0f;
        shootingRadius = 0f;
        visionRadius = 0f;

        isAlerted = false;
        playerDetected = false;
        playerInvisionRadius = false;
        playerInshootingRadius = false;

        CancelInvoke(nameof(ActiveShooting));
        previouslyShoot = true;

        if (characterController != null)
        {
            characterController.enabled = false;
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

    void FootStep()
    {
    }
}
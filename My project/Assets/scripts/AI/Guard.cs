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
    private float characterHealth = 100f;
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
    public float timebtwShoot;
    bool previouslyShoot;

    [Header("Character Controller and Gravity")]
    public CharacterController characterController;
    public float gravity = 9.81f;
    private Vector3 velocity;

    public bool isAlerted = false;
    public bool isDead = false;

    void Start()
    {
        CurrentmovingSpeed = movingSpeed;
        presentHealth = characterHealth;
        playerBody = GameObject.Find("Player");
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead) return;

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

    bool CanSeePlayer()
    {
        if (playerBody == null) return false;

        Vector3 eyePosition = transform.position + Vector3.up * 1.5f;
        Vector3 playerPosition = playerBody.transform.position + Vector3.up * 1.0f;

        Vector3 directionToPlayer = playerPosition - eyePosition;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > visionRadius)
            return false;

        Vector3 flatDirectionToPlayer = directionToPlayer;
        flatDirectionToPlayer.y = 0f;

        float angle = Vector3.Angle(transform.forward, flatDirectionToPlayer);

        if (angle > visionAngle * 0.5f)
            return false;

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
        if (waypoints.Count == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 directionToWaypoint = (targetWaypoint.position - transform.position).normalized;
        Vector3 moveVector = directionToWaypoint * movingSpeed * Time.deltaTime;

        characterController.Move(moveVector);

        Vector3 lookDirection = new Vector3(directionToWaypoint.x, 0, directionToWaypoint.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * turningSpeed);

        animator.SetBool("Run", false);
        animator.SetBool("Walk", true);
        animator.SetBool("Shoot", false);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
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
        CurrentmovingSpeed = runningSpeed;

        Vector3 directionToPlayer = (playerBody.transform.position - transform.position).normalized;
        Vector3 moveVector = directionToPlayer * CurrentmovingSpeed * Time.deltaTime;

        characterController.Move(moveVector);

        Vector3 lookDirection = new Vector3(directionToPlayer.x, 0, directionToPlayer.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * turningSpeed);

        animator.SetBool("Run", true);
        animator.SetBool("Walk", false);
        animator.SetBool("Shoot", false);
    }

    void ShootPlayer()
    {
        CurrentmovingSpeed = 0f;

        Vector3 directionToPlayer = (playerBody.transform.position - transform.position).normalized;
        Vector3 lookDirection = new Vector3(directionToPlayer.x, 0, directionToPlayer.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * turningSpeed);

        animator.SetBool("Run", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Shoot", true);

        if (!previouslyShoot)
        {
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

    private void ActiveShooting()
    {
        previouslyShoot = false;
    }

    public void characterHitDamage(float takeDamage)
    {
        if (isDead) return;

        Debug.Log("Guard took damage: " + takeDamage);

        isAlerted = true;
        playerDetected = true;
        presentHealth -= takeDamage;

        Debug.Log("Guard current health: " + presentHealth);

        if (presentHealth <= 0)
        {
            presentHealth = 0;
            isDead = true;

            Debug.Log("Guard died");
            animator.SetBool("Die", true);
            characterDie();
        }
    }

    void characterDie()
    {
        CurrentmovingSpeed = 0f;
        shootingRange = 0f;
        isAlerted = false;
        playerDetected = false;

        if (characterController != null)
        {
            characterController.enabled = false;
        }
    }

    void FootStep()
    {
    }
}
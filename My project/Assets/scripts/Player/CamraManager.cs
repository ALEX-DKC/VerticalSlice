using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    InputManager inputManager;
    PlayerMovement playerMovement;

    public Transform playerTransform;
    public Transform cameraPivot;
    public Camera playerCamera;

    private Vector3 camFollowVelocity = Vector3.zero;

    [Header("Camera Movement and Rotation")]
    public float camFollowSpeed = 0.1f;
    public float camLookSpeed = 0.1f;
    public float camPivotSpeed = 0.1f;
    public float lookAngle;
    public float pivotAngle;
    public float minimumPivotAngle = -30f;
    public float maximumPivotAngle = 30f;

    [Header("Camera Positions")]
    public Vector3 normalCameraLocalPosition = new Vector3(0f, 0f, -4f);
    public Vector3 aimingCameraLocalPosition = new Vector3(0.7f, 0f, -2f);
    public float cameraPositionLerpSpeed = 10f;

    [Header("Camera FOV")]
    public float normalFOV = 60f;
    public float aimingFOV = 40f;
    public float fovLerpSpeed = 10f;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerTransform = FindObjectOfType<PlayerManager>().transform;
        inputManager = FindObjectOfType<InputManager>();

        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>();
        }
    }

    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
        HandleCameraPositionAndFOV();
    }

    void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(
            transform.position,
            playerTransform.position,
            ref camFollowVelocity,
            camFollowSpeed
        );

        transform.position = targetPosition;
    }

    void RotateCamera()
    {
        Vector3 rotation;
        Quaternion targetRotation;

        lookAngle = lookAngle + (inputManager.CameraInputX * camLookSpeed);
        pivotAngle = pivotAngle - (inputManager.CameraInputY * camPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, minimumPivotAngle, maximumPivotAngle);

        rotation = Vector3.zero;
        rotation.y = lookAngle;
        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }

    void HandleCameraPositionAndFOV()
    {
        if (playerCamera == null || inputManager == null) return;

        Vector3 targetLocalPosition = inputManager.IsAiming()
            ? aimingCameraLocalPosition
            : normalCameraLocalPosition;

        Transform camTransform = playerCamera.transform;

        camTransform.localPosition = Vector3.Lerp(
            camTransform.localPosition,
            targetLocalPosition,
            cameraPositionLerpSpeed * Time.deltaTime
        );

        float targetFOV = inputManager.IsAiming() ? aimingFOV : normalFOV;

        playerCamera.fieldOfView = Mathf.Lerp(
            playerCamera.fieldOfView,
            targetFOV,
            fovLerpSpeed * Time.deltaTime
        );
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
   InputManager inputManager;
    PlayerMovement playerMovement;
    CameraManager cameraManager;

    void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerMovement = GetComponent<PlayerMovement>();
        cameraManager = FindObjectOfType<CameraManager>();
    }

    void Update()
    {
        inputManager.HandleAllInputs();
        playerMovement.HandleAllMovement();
        cameraManager.HandleAllCameraMovement();
    }
}

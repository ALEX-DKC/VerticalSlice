using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CrosshairUI : MonoBehaviour
{
    public InputManager inputManager;
    public GameObject crosshair;

    void Update()
    {
        if (inputManager == null || crosshair == null) return;

        bool shouldShowCrosshair = inputManager.IsAiming() && !inputManager.IsUnarmedState();

        crosshair.SetActive(shouldShowCrosshair);
    }
}

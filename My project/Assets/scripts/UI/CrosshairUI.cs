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

        crosshair.SetActive(inputManager.IsAiming());
    }
}

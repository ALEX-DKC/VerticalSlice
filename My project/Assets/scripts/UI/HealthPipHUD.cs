using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPipHUD : MonoBehaviour
{
    [Header("Player")]
    public PlayerMovement playerMovement;

    [Header("Health Pips")]
    public Image[] healthPips;

    [Header("Colors")]
    public Color fullColor = Color.white;
    public Color emptyColor = new Color(1f, 1f, 1f, 0.15f);

    void Start()
    {
        if (playerMovement == null)
        {
            GameObject player = GameObject.Find("Player");

            if (player != null)
            {
                playerMovement = player.GetComponent<PlayerMovement>();

                if (playerMovement == null)
                {
                    playerMovement = player.GetComponentInParent<PlayerMovement>();
                }
            }
        }

        UpdateHealthHUD();
    }

    void Update()
    {
        UpdateHealthHUD();
    }

    void UpdateHealthHUD()
    {
        if (playerMovement == null)
        {
            return;
        }

        int currentHealth = Mathf.CeilToInt(playerMovement.presentHealth);

        for (int i = 0; i < healthPips.Length; i++)
        {
            if (healthPips[i] == null)
            {
                continue;
            }

            if (i < currentHealth)
            {
                healthPips[i].color = fullColor;
            }
            else
            {
                healthPips[i].color = emptyColor;
            }
        }
    }
}
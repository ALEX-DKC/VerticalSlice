using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleManager : MonoBehaviour
{
    InputManager inputManager;

    public GameObject rifle1;
    public GameObject rifle2;

    public Animator animator;

    private string rifle1AnimBool = "pistolActive";
    private string rifle2AnimBool = "rifleActive";

    private GameObject[] rifles;
    private string[] rifleAnimBools;

    private int currentRifleIndex = -1;

    void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
        rifles = new GameObject[] { rifle1, rifle2 };
        rifleAnimBools = new string[] { rifle1AnimBool, rifle2AnimBool };
    }

    void Update()
    {
        if (inputManager.changeRifleInput)
        {
            CycleRifles();
            inputManager.changeRifleInput = false;
        }
    }

    void CycleRifles()
    {
        DeactivateAllRifles();

        currentRifleIndex = (currentRifleIndex + 1) % (rifles.Length + 1);

        if (currentRifleIndex < rifles.Length)
        {
            GameObject currentRifle = rifles[currentRifleIndex];
            string currentAnimBool = rifleAnimBools[currentRifleIndex];

            currentRifle.SetActive(true);
            animator.SetBool(currentAnimBool, true);

            // 切到武器：禁止移动
            inputManager.canMove = false;
        }
        else
        {
            // 切回空手：允许移动
            inputManager.canMove = true;
        }
    }

    void DeactivateAllRifles()
    {
        foreach (var rifle in rifles)
        {
            rifle.SetActive(false);
        }

        foreach (var animBool in rifleAnimBools)
        {
            animator.SetBool(animBool, false);
        }
    }
}
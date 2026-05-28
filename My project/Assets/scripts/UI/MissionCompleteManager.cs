using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionCompleteManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject missionCompletePanel;

    [Header("Player")]
    public InputManager inputManager;

    private bool missionCompleted = false;

    void Start()
    {
        Time.timeScale = 1f;

        if (missionCompletePanel != null)
        {
            missionCompletePanel.SetActive(false);
        }
    }

    public void CompleteMission()
    {
        if (missionCompleted)
        {
            return;
        }

        missionCompleted = true;

        if (inputManager != null)
        {
            inputManager.SetCanMove(false);
        }

        if (missionCompletePanel != null)
        {
            missionCompletePanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("MissionCompleteManager: MissionCompletePanel is missing.");
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
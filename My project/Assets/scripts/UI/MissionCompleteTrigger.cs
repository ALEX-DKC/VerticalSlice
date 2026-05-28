using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MissionCompleteTrigger : MonoBehaviour
{
    public MissionCompleteManager missionCompleteManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (missionCompleteManager != null)
            {
                missionCompleteManager.CompleteMission();
            }
            else
            {
                Debug.LogWarning("MissionCompleteTrigger: MissionCompleteManager is missing.");
            }
        }
    }
}
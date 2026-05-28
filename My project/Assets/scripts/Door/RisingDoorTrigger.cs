using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingDoorTrigger : MonoBehaviour
{
    public Transform door;
    public Transform player;

    public float triggerDistance = 3f;
    public float riseDistance = 3f;
    public float riseSpeed = 3f;

    private Vector3 closedPos;
    private Vector3 openPos;

    void Start()
    {
        closedPos = door.position;
        openPos = closedPos + new Vector3(0, riseDistance, 0);
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= triggerDistance)
        {
            door.position = Vector3.Lerp(door.position, openPos, riseSpeed * Time.deltaTime);
        }
        else
        {
            door.position = Vector3.Lerp(door.position, closedPos, riseSpeed * Time.deltaTime);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoorTrigger : MonoBehaviour
{
    public Transform leftDoor;
    public Transform rightDoor;
    public Transform player;

    public float triggerDistance = 3f;
    public float slideDistance = 2f;
    public float slideSpeed = 3f;

    private Vector3 leftClosedPos;
    private Vector3 rightClosedPos;
    private Vector3 leftOpenPos;
    private Vector3 rightOpenPos;

    void Start()
    {
        leftClosedPos = leftDoor.position;
        rightClosedPos = rightDoor.position;

        leftOpenPos = leftClosedPos + new Vector3(0, 0, -slideDistance);
        rightOpenPos = rightClosedPos + new Vector3(0, 0, slideDistance);
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= triggerDistance)
        {
            leftDoor.position = Vector3.Lerp(leftDoor.position, leftOpenPos, slideSpeed * Time.deltaTime);
            rightDoor.position = Vector3.Lerp(rightDoor.position, rightOpenPos, slideSpeed * Time.deltaTime);
        }
        else
        {
            leftDoor.position = Vector3.Lerp(leftDoor.position, leftClosedPos, slideSpeed * Time.deltaTime);
            rightDoor.position = Vector3.Lerp(rightDoor.position, rightClosedPos, slideSpeed * Time.deltaTime);
        }
    }
}
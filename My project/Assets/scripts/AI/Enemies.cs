using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLookAtPlayer : MonoBehaviour
{
    public Transform player;

    void Update()
    {
        if (player == null) return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0f; // 不上下歪头，只左右转

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}

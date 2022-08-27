using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectle
{
    private void Awake()
    {
        if (moveDirection != Vector2.left)
        {
            transform.rotation=Quaternion.FromToRotation(Vector2.left, moveDirection);

        }
    }
}

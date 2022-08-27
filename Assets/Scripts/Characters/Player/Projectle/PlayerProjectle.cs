using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectle : Projectle
{
    TrailRenderer trail;

    private void Awake()
    {
        trail = GetComponentInChildren<TrailRenderer>();
        if (moveDirection != Vector2.right)
        {
            transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector2.right, moveDirection);
        }
    }

    private void OnDisable()
    {
        trail.Clear();
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerEnergy.Instance.Obtain(PlayerEnergy.PERCENT);
        base.OnCollisionEnter2D(collision);
    }
}

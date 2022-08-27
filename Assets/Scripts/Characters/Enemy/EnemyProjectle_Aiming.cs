using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectle_Aiming :Projectle
{
    private void Awake()
    {
        //target = GameObject.FindGameObjectWithTag("Player");
        SetTarget(GameObject.FindGameObjectWithTag("Player"));
    }
    protected override void OnEnable() 
    {
        StartCoroutine(nameof(MoveDirectionCoroutine));
        base.OnEnable();
    }

    IEnumerator MoveDirectionCoroutine()
    {
        yield return null;
        if(target.activeSelf)
        {
            moveDirection = (target.transform.position - transform.position).normalized;
        }
    }
}

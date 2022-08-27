using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContorller : MonoBehaviour
{
    [Header("-----移动----")]
    [SerializeField]float paddingX;
    [SerializeField]float paddingY;
    [SerializeField]float moveSpeed = 2f;
    [SerializeField] float moveRotationAngle = 35f;
    [SerializeField]GameObject[] projectiles;
    [Header("-----开火----")]
    [SerializeField] AudioData projectileLanchSFX;
    [SerializeField]Transform muzzle;
    [SerializeField] float maxFireInterval ;
    [SerializeField] float minFireInterval;
    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
  
    private void OnEnable()
    {
        StartCoroutine(nameof(RandomMovingCoroutine));
        StartCoroutine(nameof(RandomFireCoroutine));
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator RandomMovingCoroutine()//移动协程
    {
        transform.position = Viewport.Instance.RandomEnemySpawnPosition(paddingX, paddingY);

        Vector3 targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY);

        while (gameObject.activeSelf)
        {
            //if has not arrived targetPosition
            if (Vector3.Distance(transform.position, targetPosition) >= moveSpeed * Time.fixedDeltaTime)
            {
                //keep moving to target
                transform.position= Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
                transform.rotation = Quaternion.AngleAxis((targetPosition - transform.position).normalized.y * moveRotationAngle, Vector3.right);
            }
            else
            {
                //set new position
                targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY);
            }


            yield return waitForFixedUpdate;
        }
    }
    IEnumerator RandomFireCoroutine()
    {
        //WaitForSeconds waitForSeconds = new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));

            foreach(var projectile in projectiles)
            {
                PoolManager.Release(projectile, muzzle.position);
                
            }
            AudioManager.Instance.PlayRandomSFX(projectileLanchSFX);

        }
    }
}

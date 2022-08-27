using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGuidanceSystem : MonoBehaviour
{
    [SerializeField] Projectle projectle;
    [SerializeField] float minBallisticAngle = 50f;
    [SerializeField] float maxBallisticAngle = 75f;
    float ballisticAngle;
    Vector3 targetDirection;
    public IEnumerator HomingCoroutine(GameObject target)
    {
        ballisticAngle = Random.Range(minBallisticAngle, maxBallisticAngle);
        while (gameObject.activeSelf)
        {
            
            if (target.activeSelf)
            {
                //��Ŀ���ƶ�
                targetDirection = target.transform.position - transform.position;
                //��Ŀ����ת
               // var angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg, Vector3.forward);
                transform.rotation *= Quaternion.Euler(0f, 0f, ballisticAngle);
                //��Ŀ���ƶ�
                projectle.ProjectileMoveDirctly();
            }
            else
            {
                projectle.ProjectileMoveDirctly();
            }
            yield return null;
        }
    }
}

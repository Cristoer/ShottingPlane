using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] Pool[] enemyPools;
    [SerializeField] Pool[] playerProjectilePools;
    [SerializeField] Pool[] enemyProjectilePools;
    [SerializeField] Pool[] vFXPools;
    static Dictionary<GameObject, Pool> dictionary;



    void Awake()
    {
        dictionary = new Dictionary<GameObject, Pool>();
        Initialize(playerProjectilePools);
        Initialize(enemyProjectilePools);
        Initialize(vFXPools);
        Initialize(enemyPools);
    }
#if UNITY_EDITOR
    private void OnDestroy()
    {
        CheckPoolSize(playerProjectilePools);
        CheckPoolSize(enemyProjectilePools);
        CheckPoolSize(vFXPools);
        CheckPoolSize(enemyPools);
    }
#endif
    void CheckPoolSize(Pool[] pools)
    {
        foreach(var pool in pools)
        {
            if (pool.RuntimeSize > pool.Size)
            {
                Debug.LogWarning(
                    string.Format("Pool:{0} has a runtime size{1} bigger than its initial size{2}",
                    pool.Prefab.name,
                    pool.RuntimeSize,
                    pool.Size));
            }
        }
    }
    void Initialize(Pool[] pools)
    {
        foreach(var pool in pools)
        {
        #if UNITY_EDITOR
            if (dictionary.ContainsKey(pool.Prefab))
            { 
                Debug.LogError("���ֶ����ͬԤ����" + pool.Prefab.name);
                continue;
            }
        #endif
            dictionary.Add(pool.Prefab, pool);

            Transform poolParent= new GameObject("Pool:" + pool.Prefab.name).transform;

            poolParent.parent = transform;
            pool.Initialize(poolParent);
            
        }
    }

    /// <summary>
    /// <para>Return a specified<paramref name="prefab"/><paramef>gameObject in the pool.</para>
    /// <para>���ݴ����<paramref name="prefab"/><paramef>���������ض������Ԥ���õ���Ϸ����</para>
    /// </summary>
    /// <param name="prefab">
    /// <para>Specified gameObject predfab</para>
    /// <para>ָ������Ϸ����Ԥ����</para>
    /// </param>
    /// <returns>
    /// <para>Prepared gameObject in the pool</para>
    /// <para>�������Ԥ���õ���Ϸ����</para>>
    /// </returns>
    /// 
    public static GameObject Release(GameObject prefab)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("�޷��ҵ�Ԥ����" + prefab.name);

            return null;
        }
#endif
       return dictionary[prefab].PrepareObject();
    }
    #region Release�������
    /// <summary>
    /// <para>Return a specified<paramref name="prefab"/><paramef>gameObject in the pool.</para>
    /// <para>���ݴ����<paramref name="prefab"/><paramef>���������ض������Ԥ���õ���Ϸ����</para>
    /// </summary>
    /// <param name="prefab">   
    /// <para>Specified gameObject predfab</para>
    /// <para>ָ������Ϸ����Ԥ����</para>
    /// </param>
    /// <param name="position">
    /// <para>Specified release position</para>
    /// <para>ָ���ͷ�λ��</para>
    /// </param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab,Vector3 position)
    {
        #if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("�޷��ҵ�Ԥ����" + prefab.name);

            return null;
        }
        #endif
        return dictionary[prefab].PrepareObject(position);
    }
    public static GameObject Release(GameObject prefab, Vector3 position,Quaternion rotation)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("�޷��ҵ�Ԥ����" + prefab.name);

            return null;
        }
#endif
        return dictionary[prefab].PrepareObject(position,rotation);
    }
    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation,Vector3 localscale)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("�޷��ҵ�Ԥ����" + prefab.name);

            return null;
        }
#endif
        return dictionary[prefab].PrepareObject(position, rotation,localscale);
        
    }
    #endregion
}

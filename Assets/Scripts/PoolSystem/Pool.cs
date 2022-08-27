using System.Collections;
using System.Collections.Generic;
using UnityEngine;



 [System.Serializable] public class Pool
{
    public GameObject Prefab => prefab;
    public int Size => size;
    public int RuntimeSize => queue.Count;

    [SerializeField]GameObject prefab;
    [SerializeField] int size = 1;
    Queue<GameObject> queue;

    Transform parent;
    public void Initialize(Transform parent)
    {
        queue = new Queue<GameObject>();
        this.parent = parent;
        for(var i = 0; i < size; i++)
        {
            queue.Enqueue(Copy());
        }
    }
    GameObject Copy()
    {
        var copy= GameObject.Instantiate(prefab,parent);

        copy.SetActive(false);
        return copy;
    }
    GameObject AvaliableObject()
    {
        GameObject avaliableObject = null;
        if (queue.Count > 0&&!queue.Peek().activeSelf)
        {
            avaliableObject= queue.Dequeue();
        }
        else
        {
            avaliableObject = Copy();
        }
        queue.Enqueue(avaliableObject);
        return avaliableObject;
        
    }
    public GameObject PrepareObject()
    {
        GameObject prepareObject = AvaliableObject();
        prepareObject.SetActive(true);

        return prepareObject;

    }
    public GameObject PrepareObject(Vector3 position)
    {
        GameObject prepareObject = AvaliableObject();
        prepareObject.SetActive(true);
        prepareObject.transform.position = position;

        return prepareObject;

    }
    public GameObject PrepareObject(Vector3 position, Quaternion rotation)
    {
        GameObject prepareObject = AvaliableObject();
        prepareObject.SetActive(true);
        prepareObject.transform.position = position;
        prepareObject.transform.rotation = rotation;

        return prepareObject;

    }
    public GameObject PrepareObject(Vector3 position,Quaternion rotation,Vector3 localScale)
    {
        GameObject prepareObject = AvaliableObject();
        prepareObject.SetActive(true);
        prepareObject.transform.position = position;
        prepareObject.transform.rotation = rotation;
        prepareObject.transform.localScale = localScale;


        return prepareObject;

    }
    //public void Return(GameObject gameObject)
    //{
    //    queue.Enqueue(gameObject);
    //}
}

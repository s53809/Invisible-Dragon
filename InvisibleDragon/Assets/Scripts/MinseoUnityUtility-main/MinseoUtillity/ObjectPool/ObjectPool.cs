using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject[] m_Childs;
    [SerializeField] private Int32[] m_NumberOfChilds;
    private Dictionary<String, Queue<GameObject>> m_Pool = new Dictionary<String, Queue<GameObject>>();
    private Dictionary<String, Int32> m_IndexOfName = new Dictionary<String, Int32>();

    private void Start()
    {
        if (m_Childs.Length != m_NumberOfChilds.Length) 
            throw new Exception("Object Pooling Error : Child GameObject and the number of Children do not match");

        for(Int32 i = 0; i < m_Childs.Length; i++)
        {
            Queue<GameObject> qu = new Queue<GameObject>();
            if (!m_Childs[i].TryGetComponent<MonoPooledObject>(out MonoPooledObject temp))
                Debug.LogWarning($"You are trying to push that have no MonoPooledObject : {m_Childs[i].name}");
            for(Int32 j = 0; j < m_NumberOfChilds[i]; j++)
            {
                GameObject obj = Instantiate(m_Childs[i], transform);
                obj.GetComponent<MonoPooledObject>().InitialSetting();
                qu.Enqueue(obj);
            }
            m_Pool.Add(m_Childs[i].name, qu);
            m_IndexOfName.Add(m_Childs[i].name, i);
        }
    }

    private void PushMoreObject(String name)
    {
        Debug.LogWarning($"You should reservate more object to reduce lagging");
        Int32 index = m_IndexOfName[name];
        m_NumberOfChilds[index] *= 2;
        for (Int32 i = 0; i < m_NumberOfChilds[index]; i++)
        {
            GameObject obj = Instantiate(m_Childs[index], transform);
            obj.GetComponent<MonoPooledObject>().InitialSetting();
            m_Pool[name].Enqueue(obj);
        }
    }

    public GameObject SpawnObject(String name)
    {
        if (!m_Pool.ContainsKey(name)) 
            throw new Exception("You are trying to spawn an unreserved object into an object pool.");
        if (m_Pool[name].Count == 0) PushMoreObject(name);
        GameObject SpawnedObj = m_Pool[name].Dequeue();
        SpawnedObj.GetComponent<MonoPooledObject>().Spawn();
        return SpawnedObj;
    }
    public GameObject SpawnObject(String name, Vector3 pos)
    {
        GameObject SpawnedObj = SpawnObject(name);
        SpawnedObj.transform.position = pos;
        return SpawnedObj;
    }

    public void RetrieveObject(GameObject obj)
    {
        String name = obj.name.Replace("(Clone)", "");

        if (!m_Pool.ContainsKey(name)) 
            throw new Exception("You are trying to put an unreserved object into an object pool.");

        m_Pool[name].Enqueue(obj);
    }

    [InspectorButton("Push Object")]
    private void TEST()
    {
        SpawnObject("Square");
    }
}

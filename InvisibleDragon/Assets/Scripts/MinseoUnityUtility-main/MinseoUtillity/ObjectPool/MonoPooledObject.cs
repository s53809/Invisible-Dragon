using System;
using UnityEngine;

public class MonoPooledObject : MonoBehaviour
{
    protected ObjectPool m_objectPooler;
    private Boolean m_isInitialSetting;

    protected virtual void OnDisable()
    {
        if (m_isInitialSetting) m_isInitialSetting = false;
        else m_objectPooler.RetrieveObject(gameObject);
    }

    public virtual void Spawn()
    {
        gameObject.SetActive(true);
    }

    public void InitialSetting()
    {
        m_objectPooler = transform.parent.GetComponent<ObjectPool>();
        m_isInitialSetting = true;
        gameObject.SetActive(false);
    }
}

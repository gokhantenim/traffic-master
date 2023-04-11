using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSingleton<T> : MonoBehaviour where T : Component
{
    static T s_Instance;

    /// <summary>
    /// static Singleton instance
    /// </summary>
    public static T Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType<T>();
                if (s_Instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    s_Instance = obj.AddComponent<T>();
                }
            }

            return s_Instance;
        }
    }

    protected virtual void Awake()
    {
        if (s_Instance == null)
        {
            s_Instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
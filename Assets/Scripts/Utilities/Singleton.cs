using UnityEngine;
using System.Collections;

/// <summary>
/// The base abstract Singleton class.
/// </summary>
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{

    private static T instance = null;

    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    public static void Clear()
    {
        instance = null;
    }

    protected virtual void Awake()
    {
        if (instance != null) Debug.LogError(name + "error: already initialized", this);

        instance = (T)this;
    }
}

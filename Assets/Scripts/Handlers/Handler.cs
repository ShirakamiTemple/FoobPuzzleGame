//***
// Author: Nate
// Description: Handler is a generic class that handlers can inherit from
//***

using UnityEngine;

public class Handler<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }
            _instance = FindObjectOfType<T>();
            if (_instance != null)
            {
                return _instance;
            }
            GameObject go = new();                      
            _instance = go.AddComponent<T>();
            go.name = _instance.GetType().Name;
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        _instance = this as T;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GEvent
{

    private event Action _action = delegate { };

    public void Invoke()
    {
        _action?.Invoke();
    }

    public void AddListener(Action listener)
    {
        _action += listener;
    }

    public void RemoveListener(Action listener)
    {
        _action -= listener;
    }
    
    
}

public class GEvent<T>
{

    private event Action<T> _action = delegate { };

    public void Invoke(T param)
    {
        _action?.Invoke(param);
    }

    public void AddListener(Action<T> listener)
    {
        _action += listener;
    }

    public void RemoveListener(Action<T> listener)
    {
        _action -= listener;
    }
    
    public void ClearAllListeners()
    {
        _action = delegate { };
    }
    
    
}
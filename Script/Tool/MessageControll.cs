using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using UnityEngine;

public class MessageControll : Singleton<MessageControll>
{
    ConcurrentDictionary<int , Action<object>> dict = new ConcurrentDictionary<int , Action<object>>();

    public void AddListener(int id, Action<object> act)
    {  
        if (!dict.ContainsKey(id))
        {
            dict.TryAdd(id, act);
        }
    }

    public void Dispatch(int id, params object[] par)
    {
        if (dict.ContainsKey(id))
        {
            dict[id]?.Invoke(par);
        }
    }

    public void Dispach(int hotfixConfirmEvent)
    {
        
    }
}

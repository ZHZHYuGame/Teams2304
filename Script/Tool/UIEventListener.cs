using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventListener : EventTrigger
{

    Dictionary<EventTriggerType, Action<object>> eventDict = new();

    public static UIEventListener AddEvent_To_Obj(GameObject obj)
    {
        UIEventListener e = obj.GetComponent<UIEventListener>();
        if (e == null)
        {
            e = obj.AddComponent<UIEventListener>();
        }
        return e;
    }

    public void AddListener(EventTriggerType type, Action<object> act)
    {
        if (!eventDict.ContainsKey(type))
        {
            eventDict.Add(type, act);
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (eventDict.ContainsKey(EventTriggerType.BeginDrag))
        {
            eventDict[EventTriggerType.BeginDrag]?.Invoke(eventData);
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
    }

    public override void OnMove(AxisEventData eventData)
    {
        base.OnMove(eventData);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (eventDict.ContainsKey(EventTriggerType.PointerEnter))
        {
            eventDict[EventTriggerType.PointerEnter]?.Invoke(eventData);
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (eventDict.ContainsKey(EventTriggerType.PointerExit))
        {
            eventDict[EventTriggerType.PointerExit]?.Invoke(eventData);
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
    }
}

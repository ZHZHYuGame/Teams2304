using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tool : MonoBehaviour,IPointerDownHandler
{
    public Action<PointerEventData> Action_OnPointerDown;
    public static Tool AddTool(GameObject obj)
    {
        var my_obj = obj.GetComponent<Tool>();

        if (my_obj == null)
        {
            my_obj = obj.AddComponent<Tool>();
        }



        return my_obj;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Action_OnPointerDown?.Invoke(eventData);
    }
}

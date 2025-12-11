using System;
using System.Collections;
using System.Collections.Generic;
using MyGame;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnPointClickTool : MonoBehaviour,IPointerClickHandler
{
    private IPointerClickHandler pointerClickHandlerImplementation;

    public static OnPointClickTool GetTool(GameObject obj)
    {
        if (obj.TryGetComponent<OnPointClickTool>( out var tool))
        {
            return tool;
        }
        
        
        return obj.AddComponent<OnPointClickTool>();
    }

    public Action act_click;
    public void OnPointerClick(PointerEventData eventData)
    {
        act_click?.Invoke();
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using Newtonsoft.Json;

public class Tool : MonoBehaviour,IPointerDownHandler
{
    public Action<PointerEventData> Action_OnPointerDown;
    public static Dictionary<string,string>logdic=new Dictionary<string,string>();
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
    public static void SaveLogData(string name,string pass)
    {
        logdic.Add(name, pass);
        string json=JsonConvert.SerializeObject(logdic);
        File.WriteAllText(Application.dataPath+ "/Resources/Log.json", json);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public Image image;
    // Start is called before the first frame update
    void Start()
    {
        //NetManager.GetInstance().Start();
        //LuaMain.GetInstance().Start();
        ABManager.GetInstance().Start();
        //Instantiate(ABManager.GetInstance().LoadAsset("cube"));
        //image.sprite = ABManager.GetInstance().LoadAsset<Sprite>("40");
        Instantiate(ABManager.GetInstance().LoadAsset("ui_window_shop"),GameObject.Find("Canvas").transform);
    }

    // Update is called once per frame
    void Update()
    {
        //NetManager.GetInstance().Update();
        //Tool_Time_Manager.GetInstance().Update();
        //LuaMain.GetInstance().Update();
    }
}

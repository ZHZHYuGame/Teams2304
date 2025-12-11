using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //NetManager.GetInstance().Start();
        //LuaEnv_Mgr.GetInstance().Start();
        ABManager.GetInstance().Start();
        Instantiate( ABManager.GetInstance().LoadAsset<GameObject>("cube"));
    }

    // Update is called once per frame
    void Update()
    {
        //NetManager.GetInstance().Update();
        //Tool_Time_Manager.GetInstance().Update();
        //LuaEnv_Mgr.GetInstance().Update();
    }
}

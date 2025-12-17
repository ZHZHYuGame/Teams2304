using MyGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ABManager.GetInstance().OnInit();
        //Instantiate(ABManager.GetInstance().LoadAsset<GameObject>("prefab/kamiapp"));

        NetManager.GetInstance().Start();
        //ABManager.GetInstance().OnInit();
        LuaEnvMgr.GetInstance().Start();  
    }

    // Update is called once per frameUpdate
    void Update()
    {
        NetManager.GetInstance().Update();
    }
}

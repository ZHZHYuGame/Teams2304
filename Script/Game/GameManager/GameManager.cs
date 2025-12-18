
using System;
using MyGame;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏管理类
/// </summary>
public class GameManager : MonoBehaviour
{
    
    private void Start()
    {
        NetManager.GetInstance().Start();
        ABManager.GetInstance().OnInit();
        LuaEnvMgr.GetInstance().Start();
        

    }

    private void Update()
    {
        NetManager.GetInstance().Update();
    }
}

using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏管理类
/// </summary>
public class GameManager : MonoBehaviour
{
    
    private void Start()
    {
        
       
        LuaEnvMgr.GetInstance().Start();
    }

    private void Update()
    {
        
    }
}
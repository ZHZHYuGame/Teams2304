
using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏管理类
/// </summary>
public class GameManager : MonoBehaviour
{
    public GameObject loadView;
    public Button yes_btn, cancel_btn;
    private void Awake()
    {
        MessageControl.GetInstance().AddListener(Client_Request.Open_LoadUI,Open_LoadUI_Handle);
        yes_btn.onClick.AddListener(() =>
        {
            loadView.SetActive(false);
            MessageControl.GetInstance().Dispach(Client_Request.Hotfix_AbAsset);
        });
        
    }

    private void Open_LoadUI_Handle(object obj)
    {
        loadView.SetActive(true);
    }

    private void Start()
    {
        VersionAssetHotfixMgr.ins.OnStart();
        
        
        
    }
}
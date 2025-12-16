using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using MyGame;
using UnityEngine;
using UnityEngine.UI;

public class HPManger : Singleton<HPManger>
{
    public Slider playerSlider;
    private float MaxHp;
    private bool isnohp;
    public void Init()
    {
        playerSlider=GameObject.Find("Canvas").transform.Find("PlayerSlider").GetComponent<Slider>();
        MessageControll.GetInstance().AddListener(NetID.S_To_C_PlayerHp,RefreshHp);
    }

    private void RefreshHp(object obj)
    {
        object[] objs =obj as object[];
        byte[] bytes = objs[0] as byte[];
        //Socket st=objs[1] as Socket;
        
        S_To_C_PlayerHp cmsg =S_To_C_PlayerHp.Parser.ParseFrom(bytes);
        if (cmsg.PlayerId==CreatPlayer.PlayerID)
        {
            if (!isnohp)
            {
                isnohp = true;
                MaxHp=(float)cmsg.Hp;
                playerSlider.maxValue = MaxHp;
            }
            playerSlider.value =(float)cmsg.Hp;
            if (cmsg.Type == PlayerType.Dead)
            {
                Player.instance.Nowtype = PlayerType.Dead;
                PlayerAnimatorMgr.GetInstance().SendAnimator(Player.instance.playerId,"Dead");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        
    }
}

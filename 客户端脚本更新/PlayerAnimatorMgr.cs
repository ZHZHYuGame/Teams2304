using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using MyGame;
using UnityEngine;

public class PlayerAnimatorMgr : Singleton<PlayerAnimatorMgr>
{
    public void Init()
    {
        MessageControll.GetInstance().AddListener(NetID.S_To_C_PlayerAnimator,RefreshAnimator);
    }

    private void RefreshAnimator(object obj)
    {
        object[] objs = obj as object[];
        byte[] bytes = objs[0] as byte[];
        S_To_C_PlayerAnimator cmsg = S_To_C_PlayerAnimator.Parser.ParseFrom(bytes);

        CreatPlayer.instance.PlayerDic[cmsg.PlayerId].SetAnimator(cmsg.AnimatorName);
    }

    public void SendAnimator(uint id,string Aname)
    {
        C_To_S_PlayerAnimator msg=new C_To_S_PlayerAnimator();
        msg.PlayerId = id;
        msg.AnimatorName = Aname;
        NetManager.GetInstance().SendMessage(NetID.C_To_S_PlayerAnimator,msg.ToByteArray());
    }

    
}

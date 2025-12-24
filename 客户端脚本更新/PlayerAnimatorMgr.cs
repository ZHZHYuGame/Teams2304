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
        
        if (cmsg.PlayerId==CreatPlayer.PlayerID)
        {
            Player.instance.SetAnimator(cmsg.AnimatorName,cmsg.AniType,cmsg.Type);
        }
        else
        {
            CreatPlayer.instance.PlayerDic[cmsg.PlayerId].GetComponent<Player>().SetAnimator(cmsg.AnimatorName, cmsg.AniType, cmsg.Type);
            if (cmsg.AnimatorName=="Dead")
            {
                CreatPlayer.instance.PlayerDic[cmsg.PlayerId].GetComponent<Collider>().enabled = !cmsg.AniType;
                CreatPlayer.instance.PlayerDic[cmsg.PlayerId].GetComponent<Rigidbody>().useGravity = !cmsg.AniType;
            }
        }
    }
    
    public void SendAnimator(uint id,string Aname,bool AniType,PlayerAniType type)
    {
        C_To_S_PlayerAnimator msg=new C_To_S_PlayerAnimator();
        msg.PlayerId = id;
        msg.AnimatorName = Aname;
        msg.AniType = AniType;
        msg.Type = type;
        NetManager.GetInstance().SendMessage(NetID.C_To_S_PlayerAnimator,msg.ToByteArray());
    }

    
}

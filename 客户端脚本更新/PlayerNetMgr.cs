using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame;
using Google;
using Google.Protobuf;

public class PlayerNetMgr : Singleton<PlayerNetMgr>
{
    /// <summary>
    /// 刷新玩家位置
    /// </summary>
    /// <param name="player"></param>
    /// <param name="playerId"></param>
    public void RefreshPlayerPos(Transform player,uint playerId)
    {
        C_To_S_PlayerOperation msg=new C_To_S_PlayerOperation();
        msg.PlayerId = playerId;
        msg.X=player.position.x;
        msg.Y=player.position.y;
        msg.Z=player.position.z;
        msg.RoundY=player.rotation.eulerAngles.y;
        NetManager.GetInstance().SendMessage(NetID.C_To_S_PlayerOperation,msg.ToByteArray());
    }
}

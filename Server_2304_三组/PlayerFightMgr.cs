using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Google;
using Google.Protobuf;
using MyGame;
namespace Server_2304
{
    
    public class PlayerFightMgr:Singleton<PlayerFightMgr>
    {
        Dictionary<int,C_To_S_PlayerOperation> PlayerDic =new Dictionary<int, C_To_S_PlayerOperation>();
        
        public void Init()
        {
            MessageControll.GetInstance().AddListener(NetID.C_To_S_PlayerOperation,InitPlayer);
            MessageControll.GetInstance().AddListener(NetID.C_To_S_Disconnect,DisconnectPlayer);
            MessageControll.GetInstance().AddListener(NetID.C_To_S_ExitGame,C_To_S_ExitGameHandle);
        }
        /// <summary>
        /// 退出游戏
        /// </summary>
        /// <param name="obj"></param>
        private void C_To_S_ExitGameHandle(object obj)
        {
            object[] objs = obj as object[];
            Byte[] bytes = objs[0] as Byte[];
            Socket st = objs[1] as Socket;
            C_To_S_ExitGame csMsg = C_To_S_ExitGame.Parser.ParseFrom(bytes);
            S_To_C_ExitGame scMsg = new  S_To_C_ExitGame();
            scMsg.PlayerId = csMsg.PlayerId;
            PlayerHpMgr.GetInstance().RemovePlayerHp(scMsg.PlayerId);

            foreach (var item in NetManager.GetInstance().clientsList)
            {
                if (item.st !=st)
                {
                    NetManager.GetInstance().SendMessage(NetID.S_To_C_Disconnect,scMsg.ToByteArray(),item.st);
                }
            }
        }

        private void DisconnectPlayer(object o)
        {
            object[] objs = o as object[];
            Byte[] bytes = objs[0] as Byte[];
            Socket st = objs[1] as Socket;
            
            C_To_S_Disconnect msg =C_To_S_Disconnect.Parser.ParseFrom(bytes);
            S_To_C_Disconnect toCMsg = new S_To_C_Disconnect();
            toCMsg.PlayerId = msg.PlayerId;
            //清除服务器在线列表中的这个客户端以及停止接受消息
            NetManager.GetInstance().RevomeSt(st);
            //删除此玩家的血条数据
            PlayerHpMgr.GetInstance().RemovePlayerHp(msg.PlayerId);
            
            foreach (var item in NetManager.GetInstance().clientsList)
            {
                NetManager.GetInstance().SendMessage(NetID.S_To_C_Disconnect,toCMsg.ToByteArray(),item.st);
            }
        }

        private void InitPlayer(object obj)
        {
            object[] ooo = obj as object[];
            Socket st = ooo[1] as Socket;
            C_To_S_PlayerOperation cmsg=C_To_S_PlayerOperation.Parser.ParseFrom(ooo[0] as byte[]);
            //初始化添加玩家血量
            PlayerHpMgr.GetInstance().AddPlayerHp(cmsg.PlayerId);
            
            S_To_C_PlayerOperation msg = new S_To_C_PlayerOperation();
            msg.PlayerId = cmsg.PlayerId;
            msg.X = cmsg.X;
            msg.Y = cmsg.Y;
            msg.Z = cmsg.Z;
            msg.RoundY=cmsg.RoundY;
            foreach (var item in NetManager.GetInstance().clientsList)
            {
                NetManager.GetInstance().SendMessage(NetID.S_To_C_PlayerOperation,msg.ToByteArray(),item.st);
              
            }
            
        }
    }

}
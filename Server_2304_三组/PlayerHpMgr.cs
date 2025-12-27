using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using MyGame;
using Google;
using Google.Protobuf;

namespace Server_2304
{
    /// <summary>
    /// 玩家血条管理
    /// </summary>
    public class PlayerHpMgr:Singleton<PlayerHpMgr>
    {
        //所有玩家的血条数据
        private Dictionary<uint,int> allPlayerHp = new Dictionary<uint,int>();
        private int maxHp;
        public void Init()
        {
            maxHp = 200;
            MessageControll.GetInstance().AddListener(NetID.C_To_S_PlayerAtk,C_To_S_PlayerAtkHandler);
            MessageControll.GetInstance().AddListener(NetID.C_To_S_PlayerAlive,C_To_S_PlayerAliveHandler);
        }
        /// <summary>
        /// 玩家复活，重置血量
        /// </summary>
        /// <param name="o"></param>
        private void C_To_S_PlayerAliveHandler(object obj)
        {
            object[] objs = obj as object[];
            byte[] bytes = objs[0] as byte[];
            Socket st = objs[1] as Socket;
            
            C_To_S_PlayerAlive csMsg = C_To_S_PlayerAlive.Parser.ParseFrom(bytes);
            
            if (allPlayerHp.ContainsKey(csMsg.PlayerId))
            {
                allPlayerHp[csMsg.PlayerId] = maxHp;
                Console.WriteLine($"{csMsg.PlayerId}满血复活{maxHp}");
                RefreshAllPlayerHp(csMsg.PlayerId,allPlayerHp[csMsg.PlayerId],PlayerType.None);
            }
        }

        /// <summary>
        ///收到攻击数据
        /// </summary>
        /// <param name="obj"></param>
        private void C_To_S_PlayerAtkHandler(object obj)
        {
            object[] objs = obj as object[];
            byte[] bytes = objs[0] as byte[];
            Socket st = objs[1] as Socket;
            
            C_To_S_PlayerAtk csMsg = C_To_S_PlayerAtk.Parser.ParseFrom(bytes);
            ReduceHP(csMsg.AtkPlayerId,csMsg.BAtkPlayerId,csMsg.BulletID);
        }
        /// <summary>
        ///攻击扣血方法
        /// </summary>
        /// <param name="atkPlayerID"></param>
        /// <param name="bAtkPlayerID"></param>
        /// <param name="bulletID"></param>
        public void ReduceHP(uint atkPlayerID,uint bAtkPlayerID,uint bulletID)
        {
            if (allPlayerHp.ContainsKey(bAtkPlayerID))
            {
                allPlayerHp[bAtkPlayerID] -= 20;
                PlayerType type = PlayerType.None;
                if (allPlayerHp[bAtkPlayerID] <=0)
                {
                    allPlayerHp[bAtkPlayerID] = 0;
                    //该玩家血条为零，通知所有人该玩家死亡
                    type =  PlayerType.Dead;
                    PlayerConfig.GetInstance().SetGold(3000, atkPlayerID);
                }
                RefreshAllPlayerHp(bAtkPlayerID, allPlayerHp[bAtkPlayerID],type);
                Console.WriteLine($"{atkPlayerID}对{bAtkPlayerID}造成了20点伤害！剩余血量{allPlayerHp[bAtkPlayerID]}");
            }
            
            
        }
        /// <summary>
        /// 通知所有客户端某个玩家的血量
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="hp"></param>
        public void RefreshAllPlayerHp(uint playerId,int hp,PlayerType type)
        {
            S_To_C_PlayerHp scMsg = new S_To_C_PlayerHp();
            foreach (var item in NetManager.GetInstance().clientsList)
            {
                scMsg.PlayerId = playerId;
                scMsg.Hp = (uint)hp;
                scMsg.Type = type;
                NetManager.GetInstance().SendMessage(NetID.S_To_C_PlayerHp,scMsg.ToByteArray(),item.st);
            }
        }
        /// <summary>
        /// 初始化添加
        /// </summary>
        /// <param name="playerId"></param>
        public void AddPlayerHp(uint playerId)
        {
            if (!allPlayerHp.ContainsKey(playerId))
            {
                allPlayerHp.Add(playerId,maxHp);
                RefreshAllPlayerHp(playerId, allPlayerHp[playerId],PlayerType.None);
            }
        }
        /// <summary>
        /// 删除某个玩家的血条数据
        /// </summary>
        /// <param name="playerId"></param>
        public void RemovePlayerHp(uint playerId)
        {
            if (allPlayerHp.ContainsKey(playerId))
            {
                allPlayerHp.Remove(playerId);
            }
        }
    }
}
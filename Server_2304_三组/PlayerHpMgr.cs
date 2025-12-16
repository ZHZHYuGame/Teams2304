using System;
using System.Collections.Generic;
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
        private Dictionary<uint,uint> allPlayerHp = new Dictionary<uint,uint>();
        public void Init()
        {
            MessageControll.GetInstance().AddListener(NetID.C_To_S_PlayerAtk,C_To_S_PlayerAtkHandler);
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
                
                RefreshAllPlayerHp(bAtkPlayerID, allPlayerHp[bAtkPlayerID]);
            }
            Console.WriteLine($"{atkPlayerID}对{bAtkPlayerID}造成了20点伤害！");
        }
        /// <summary>
        /// 通知所有客户端某个玩家的血量
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="hp"></param>
        public void RefreshAllPlayerHp(uint playerId,uint hp)
        {
            S_To_C_PlayerHp scMsg = new S_To_C_PlayerHp();
            foreach (var item in NetManager.GetInstance().clientsList)
            {
                scMsg.PlayerId = playerId;
                scMsg.Hp = hp;
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
                allPlayerHp.Add(playerId,100);
                RefreshAllPlayerHp(playerId, allPlayerHp[playerId]);
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
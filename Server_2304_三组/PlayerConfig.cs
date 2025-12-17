using System;
using System.Collections.Generic;
using System.Net.Sockets;
using MyGame;
using Google;
using Google.Protobuf;

namespace Server_2304
{
    public class PlayerConfig:Singleton<PlayerConfig>
    {
        private int Glod;
        Dictionary<uint, PlayerData> allPlayerDic = new Dictionary<uint, PlayerData>();
        public void InitPlayer()
        {
            Glod = 10000;
            MessageControll.GetInstance().AddListener(NetID.C_To_S_Player_Res,C_To_S_Player_ResHandler);
            MessageControll.GetInstance().AddListener(NetID.S_To_C_Player_Login,S_To_C_Player_LoginHandler);
            MessageControll.GetInstance().AddListener(NetID.C_To_S_GetPlayer,GetPlayerHandler);
        }
        //玩家初始化背包的空背包集合
        /// <summary>
        ///登录
        /// </summary>
        /// <param name="o"></param>
        private void S_To_C_Player_LoginHandler(object obj)
        {
            object[] ooo = obj as object[];
            byte[] bytes = ooo[0] as byte[];
            Socket st = ooo[1] as Socket;
            C_To_S_PlayerInfo csMsg = C_To_S_PlayerInfo.Parser.ParseFrom(bytes);
            
            S_To_C_PlayerInfo scMsg =new  S_To_C_PlayerInfo();
            
            if (allPlayerDic.ContainsKey(csMsg.PlayerId))
            {
                //登录成功
                scMsg.IsRuccess = true;
                
            }
            else
            {
                //登录失败，没注册
                scMsg.IsRuccess = false;
                
            }

            NetManager.GetInstance().SendMessage(NetID.S_To_C_PlayerInfo,scMsg.ToByteArray(),st);
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="obj"></param>
        private void C_To_S_Player_ResHandler(object obj)
        {
            object[] ooo = obj as object[];
            byte[] bytes = ooo[0] as byte[];
            Socket st = ooo[1] as Socket;
            
            C_To_S_PlayerInfo csMsg = C_To_S_PlayerInfo.Parser.ParseFrom(bytes);
            //玩家注册
            if (!allPlayerDic.ContainsKey(csMsg.PlayerId))
            {
                List<BagData> bags = new List<BagData>();
                for (int i = 0; i < 40; i++)
                {
                    BagData bag = new BagData();
                    bag.BagItemID = i;
                    bag.ShopData = null;
                    bag.Count = 0;
                    bags.Add(bag);
                }
                
                Client data = NetManager.GetInstance().GetClientData(st);
                data.playerId =  csMsg.PlayerId;
                PlayerData playerData = new PlayerData();
                playerData.glod = 10000;
                playerData.bag = bags;
                allPlayerDic.Add(csMsg.PlayerId,playerData);
                Console.WriteLine($"玩家{csMsg.PlayerId}注册成功");
            }
        }

        private void GetPlayerHandler(object obj)
        {
            object[] ooo = obj as object[];
            byte[] bytes = ooo[0] as byte[];
            Socket st = ooo[1] as Socket;
            
            C_To_S_GetPlayerData_Msg csMsg = C_To_S_GetPlayerData_Msg.Parser.ParseFrom(bytes);
            
            S_To_C_GetPlayerData_Msg msg = new S_To_C_GetPlayerData_Msg();
            msg.Glod = GetGold(csMsg.PlayerId);
            NetManager.GetInstance().SendMessage(NetID.S_To_C_GetPlayer,msg.ToByteArray(),st);
        }

        public List<BagData> GetBags(uint playerId)
        {
            if (allPlayerDic.ContainsKey(playerId))
            {
                return allPlayerDic[playerId].bag;
            }
            return null;
        }
        public int GetGold(uint playerId)
        {
            if (allPlayerDic.ContainsKey(playerId))
            {
                return allPlayerDic[playerId].glod;
            }
            return 0;
        }

        public bool SetGold(int gold,uint playerId)
        {
            if (allPlayerDic.ContainsKey(playerId))
            {
                int glod = allPlayerDic[playerId].glod;
                if (glod + gold < 0)
                {
                    allPlayerDic[playerId].glod = glod;
                    return false;
                }
                else
                {
                    glod += gold;
                    allPlayerDic[playerId].glod = glod;
                    return true;
                }
            }

            return false;
        }
    }
}
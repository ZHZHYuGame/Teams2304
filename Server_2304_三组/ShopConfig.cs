
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using Google.Protobuf;
using Google;
using MyGame;
using Newtonsoft.Json;

namespace Server_2304
{
    public class ShopConfig:Singleton<ShopConfig>
    {
        private List<ShopData> list;
        public void Init()
        {
            MessageControll.GetInstance().AddListener(NetID.C_To_S_GetShop,C_To_S_GetShop_Handler);
        }
        
        private void C_To_S_GetShop_Handler(object obj)
        {
            list = new List<ShopData>();
            S_To_C_GetShopData_Msg msg = new S_To_C_GetShopData_Msg();
            
            object[] ooo = obj as object[];
            byte[] data = ooo[0] as byte[];
            Socket st = ooo[1] as Socket;
            
            list = JsonConvert.DeserializeObject<List<ShopData>>(File.ReadAllText("Good.json"));
            
            foreach (var item in list)
            {
                msg.Shopdatas.Add(item);
            }

            Console.WriteLine($"商城数据{msg.Shopdatas.Count}");
            NetManager.GetInstance().SendMessage(NetID.S_To_C_GetShop,msg.ToByteArray(),st);
        }

        public ShopData GetShop(int id)
        {
            foreach (var item in list)
            {
                if (id== item.Id)
                {
                    return item;
                }
            }
            return null;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using MyGame;
using Google;
using Google.Protobuf;

namespace Server_2304
{
    public class BagConfig:Singleton<BagConfig>
    {
        //背包数据
        private List<BagData> bags = new List<BagData>();
        public void Init()
        {
            InitBagDatas();
            MessageControll.GetInstance().AddListener(NetID.C_To_S_GetBag,GetBagHandler);
            MessageControll.GetInstance().AddListener(NetID.C_To_S_BayShop,BayShopHandler);
            
        }
        
        void InitBagDatas()
        {
            for (int i = 0; i < 40; i++)
            {
                BagData bag = new BagData();
                bag.BagItemID = i;
                bag.ShopData = null;
                bag.Count = 0;
                bags.Add(bag);
            }
        }
        private void BayShopHandler(object obj)
        {
            object[] ooo = obj as object[];
            byte[] data = ooo[0] as byte[];
            Socket st = ooo[1] as Socket;
            
            C_To_S_BayShop_Msg msg = C_To_S_BayShop_Msg.Parser.ParseFrom(data);
            
            ShopData shop = ShopConfig.GetInstance().GetShop(msg.GoodID);
            S_To_C_S_BayShop_Msg bagmsg = new S_To_C_S_BayShop_Msg();
            
            if (PlayerConfig.GetInstance().SetGold(-shop.Sale))
            {
                BagData bagData = AddBags(shop);//加入背包并返回数据改变的具体格子
                if (bagData == null)
                {
                    //背包已满
                    bagmsg.Type = BayType.Item;
                    bagmsg.BagData = null;
                    bagmsg.Glod = PlayerConfig.GetInstance().GetGold();
                }
                else
                {
                    //购买成功
                    bagmsg.BagData = bagData;
                    bagmsg.Type = BayType.Success;
                    bagmsg.Glod = PlayerConfig.GetInstance().GetGold();
                }
            }
            else
            {
                //金币不足
                bagmsg.Type = BayType.Glod;
                bagmsg.BagData = null;
                bagmsg.Glod = PlayerConfig.GetInstance().GetGold();
            }
            
            NetManager.GetInstance().SendMessage(NetID.S_To_C_BayShop,bagmsg.ToByteArray(),st);
            
        }
        BagData AddBags(ShopData shop)
        {
            bool isDie = false;
            for (int i = 0; i < bags.Count; i++)
            {
                if (bags[i].ShopData!=null)
                {
                    if (bags[i].ShopData.Id == shop.Id&&shop.InventoryType!= "装备")
                    {
                        bags[i].Count += 1;
                        isDie = true;
                        return bags[i];
                    }
                }
            }

            if (!isDie)
            {
                for (int i = 0; i < bags.Count; i++)
                {
                    if (bags[i].ShopData == null)
                    {
                        BagData bag = new BagData();
                        bag.BagItemID = i;
                        bag.ShopData = shop;
                        bag.Count = 1;
                        bags[i] = bag;
                        return bags[i];
                    }
                }
            }

            return null;
        }
        
        private void GetBagHandler(object obj)
        {
            object[] objs = obj as object[];
            Socket st = objs[1] as Socket;
            
            S_To_C_GetBagData_Msg msg = new S_To_C_GetBagData_Msg();
            foreach (var item in bags)
            {
                msg.BagDatas.Add(item);
            }
            NetManager.GetInstance().SendMessage(NetID.S_To_C_GetBag,msg.ToByteArray(),st);
        }
    }
}
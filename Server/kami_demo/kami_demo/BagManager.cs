using MyGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;



public class BagManager : Singleton<BagManager>
{
    Dictionary<int, Dictionary<int, BagData>> bagDict = new Dictionary<int, Dictionary<int, BagData>>();

    public Dictionary<int, Dictionary<int, BagData>> GetBagDict()
    {
        return bagDict;
    }

    public void OnStart()
    {
        bagDict = ConfigManager.GetInstance().ReadJson<Dictionary<int, Dictionary<int, BagData>>>("bag.json");

        MessageControl.GetInstance().AddListener(NetID.C_To_S_UseBagItem_Msg, C_To_S_UseBagItem_Msg_Handle);
    }

    private void C_To_S_UseBagItem_Msg_Handle(object obj)
    {
        //反序列化消息
        object[] objArray = obj as object[];
        byte[] byteArray = objArray[0] as byte[];
        Socket my_st = objArray[1] as Socket;

        C_To_S_UseBagItem_Msg c_msg = C_To_S_UseBagItem_Msg.Parser.ParseFrom(byteArray);
        S_To_C_UseBagItem_Msg s_msg = new S_To_C_UseBagItem_Msg();
        int c_type = (int)c_msg.ItemType;
        //减少物品
        s_msg.ItemType = c_msg.ItemType;
        if (bagDict.TryGetValue(c_type, out var baglist))
        {
            if (baglist.TryGetValue(c_msg.BagData.Id, out var bagData))
            {
                bagData.count -= c_msg.BagData.Count;

                s_msg.BagData = new NetBagData
                {
                    Id = bagData.itemId,
                    ItemId = bagData.itemId,
                    ItemIndex = bagData.itemIndex,
                    Count = bagData.count
                };


                if (bagData.count == 0)
                {
                    baglist.Remove(c_msg.BagData.Id);
                }

                //写入json
                var str = JsonConvert.SerializeObject(bagDict);
                File.WriteAllText("bag.json",str);
            }

        }

        s_msg.AddType = c_msg.AddType;

        switch (c_msg.ItemType)
        {
            case ItemType.Res:
                //添加资源
                int count = MainSurfaceManager.GetInstance().AddRes(c_msg.AddType, c_msg.UpdateCount * c_msg.BagData.Count);
                s_msg.UpdateCount = count;

                break;
            case ItemType.AddTime:
                break;
            case ItemType.Buff:
                break;
            case ItemType.Equip:
                break;
            case ItemType.Other:
                break;
        }

        NetManager.GetInstance().SendMessage(NetID.S_To_C_UseBagItem_Msg, s_msg.ToByteArray(), my_st);

    }



    public Dictionary<int, BagData> AddBagData(int type,int itemId,int count) 
    {
        Dictionary<int, BagData> dict = new Dictionary<int, BagData>();
        var item_type = (ItemType)type;


        if (bagDict.TryGetValue(type, out var baglist))
        {
            if (item_type == ItemType.Equip)
            {
                //不可叠加

                for (global::System.Int32 i = 0; i < count; i++)
                {
                    //生成新的物品唯一id
                    int id = ConfigManager.GetInstance().CreateItemId();
                    var bagData = new BagData
                    {
                        id = id,
                        itemId = itemId,
                        count = 1,
                        itemIndex = baglist.Count + 1 //物品索引顺序
                    };

                    baglist.Add(id, bagData);

                    dict.Add(bagData.id, bagData);

                }

                
            }
            else
            {

                baglist = new Dictionary<int, BagData>();

                if (baglist.TryGetValue(itemId, out var bagData))
                {
                    bagData.count += count;

                    dict.Add(bagData.id, bagData);
                }
                else
                {
                    //生成新的物品唯一id
                    int id = ConfigManager.GetInstance().CreateItemId();
                    bagData = new BagData
                    {
                        id = id,
                        itemId = itemId,
                        count = count,
                        itemIndex = baglist.Count + 1 //物品索引顺序
                    };
                    baglist.Add(id, bagData);
                    dict.Add(bagData.id, bagData);
                }

                bagDict.Add(type, baglist);
            }

        } 
        
        return dict;
    }
    public class BagData
{
    public int id;
    public int itemId;
    public int itemIndex;
    public int count;
}
}
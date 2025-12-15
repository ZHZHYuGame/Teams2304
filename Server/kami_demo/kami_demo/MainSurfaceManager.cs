using Google.Protobuf;
using MyGame;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;



public class MainSurfaceManager : Singleton<MainSurfaceManager>
{
    PlayerData playerData;
    public void OnStart()
    {
        playerData = ConfigManager.GetInstance().ReadJson<PlayerData>("player.json");

        MessageControl.GetInstance().AddListener(NetID.C_To_S_GetPlayerData_Msg, C_To_S_GetPlayerData_Msg_Handle);
    }

    private void C_To_S_GetPlayerData_Msg_Handle(object obj)
    {
        //反序列化消息
        object[] objArray = obj as object[];
        byte[] byteArray = objArray[0] as byte[];
        Socket my_st = objArray[1] as Socket;

        C_To_S_GetPlayerData_Msg c_msg = C_To_S_GetPlayerData_Msg.Parser.ParseFrom(byteArray);

        S_To_C_GetPlayerData_Msg s_msg = new S_To_C_GetPlayerData_Msg();

        //赋值角色信息
        s_msg.PlayerData = new NetPlayerData
        {
            PlayerId = playerData.playerId,
            Wood = playerData.wood,
            Food = playerData.food,
            Diamond = playerData.diamond,
        };

        //赋值背包信息
        var dict = BagManager.GetInstance().GetBagDict();
        foreach (var item in dict)
        {
            foreach (var list in item.Value)
            {
                var bagData = new NetBagData
                {
                    Id = list.Value.id,
                    ItemId = list.Value.itemId,
                    ItemIndex = list.Value.itemIndex,
                    Count = list.Value.count,
                };
                s_msg.BagDataDict.Add(list.Key, bagData);
            }

        }


        NetManager.GetInstance().SendMessage(NetID.S_To_C_GetPlayerData_Msg, s_msg.ToByteArray(), my_st);

    }


    /// <summary>
    /// 添加资源
    /// </summary>
    /// <returns></returns>
    public int AddRes(AddResType type, int count)
    {
        int nums = -1;

        switch (type)
        {
            case AddResType.Food:
                playerData.food += count;
                nums = playerData.food;
                break;
            case AddResType.Wood:
                playerData.wood += count;
                nums = playerData.wood;
                break;
            case AddResType.Diamond:
                playerData.diamond += count;
                nums = playerData.diamond;
                break;
        }

        //写入json
        string str = JsonConvert.SerializeObject(playerData);
        File.WriteAllText("player.json", str);


        return nums;
    }


}
public class PlayerData
{
    public int playerId;
    public int food;
    public int wood;
    public int diamond;
}
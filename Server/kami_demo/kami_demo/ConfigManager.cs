using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2304_Net;
using Newtonsoft.Json;


/// <summary>
/// 配置表管理类
/// </summary>
public class ConfigManager:Singleton<ConfigManager>
{
    Net_CreateData netData;
    public void OnStart() 
    {
        netData = ReadJson<Net_CreateData>("netData.json");
    }

    public T ReadJson<T>(string str) where T : class
    {
        T config = JsonConvert.DeserializeObject<T>(File.ReadAllText(str));

        return config;
    }


    public int CreateItemId() 
    {
        ++netData.itemId;
        //写入json
        var str = JsonConvert.SerializeObject(netData);
        File.WriteAllText("netData.json",str);

        return netData.itemId;
    }

}

public class ShopData 
{  
    public int id;
    public int count;
}
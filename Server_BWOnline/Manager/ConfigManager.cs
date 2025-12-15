using MyGame;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_2304.Manager
{
    internal class ConfigManager:Singleton<ConfigManager>
    {
        public List<GoodsData>shoplist= new List<GoodsData>();
        public S_To_C_ShopGoods goods=new S_To_C_ShopGoods();

        public void OnShop()
        {
            shoplist = JsonConvert.DeserializeObject<List<GoodsData>>(File.ReadAllText("Goods.json"));
            Console.WriteLine(shoplist.Count);
            foreach (var item in shoplist)
            {
                goods.Goodsdatalist.Add(item);
            }
            Console.WriteLine(goods.Goodsdatalist.Count);
        }
    }
}

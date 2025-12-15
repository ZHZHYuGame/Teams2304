using Google.Protobuf;
using MyGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server_2304.Manager
{
    internal class ShopManager:Singleton<ShopManager>
    {
        public void Start()
        {
            MessageControll.GetInstance().AddListener(NetID.C_To_S_ShopGoods, C_To_S_ShopGoods_Handel);
            MessageControll.GetInstance().AddListener(NetID.C_To_S_BuyGood, C_To_S_BuyGood_Handel);
        }

        /// <summary>
        /// 处理客户端购买商品的请求
        /// </summary>
        /// <param name="obj"></param>
        private void C_To_S_BuyGood_Handel(object obj)
        {
            try
            {
                var objlist = obj as object[];
                byte[] data = objlist[0] as byte[];
                Socket s_ket = objlist[1] as Socket;

                //反序列化客户端购买请求
                var buyRequest = C_To_S_BuyGood.Parser.ParseFrom(data);
                //通过网络管理器
                var client = NetManager.GetInstance().GetClient(s_ket);
                //根据商品ID查找商店中的商品
                var targetGood = ConfigManager.GetInstance().shoplist.Find(g => g.Id == buyRequest.Id.ToString());
                if(targetGood==null)
                {
                    Console.WriteLine($"未找到ID为{buyRequest.Id}的商品");
                    SendBuyResult(s_ket, false);
                    return;
                }
                //检查商品库存是否充足
                if(targetGood.Num<buyRequest.Num)
                {
                    Console.WriteLine($"商品{targetGood.Name}库存不足，当前库存：{targetGood.Num},请求购买：{buyRequest.Num}");
                    SendBuyResult(s_ket, false);
                    return;
                }
                //计算购买总价
                if(!int.TryParse(targetGood.Sale,out int price))
                {
                    Console.WriteLine($"商品{targetGood.Name}价格格式错误：{targetGood.Sale}");
                    SendBuyResult(s_ket, false);
                }
                int totalCost = price * buyRequest.Num;
                //检查客户端金钱是否足够
                if(client.money<totalCost)
                {
                    Console.WriteLine($"客户端{client.name}金钱不足，当前：{client.money}，需要：{totalCost}");
                    SendBuyResult(s_ket, false);
                    return;
                }
                //执行购买逻辑
                client.money-=totalCost;
                targetGood.Num-=buyRequest.Num;
                Console.WriteLine($"{client.name}购买成功，商品：{targetGood.Name},数量：{buyRequest.Num},花费：{totalCost}");
                SendBuyResult(s_ket, true);

                //购买成功后更新客户端背包数据
                UpdateClientBag(s_ket,targetGood,buyRequest.Num);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"处理购买商品异常：{ex.Message}");
            }
        }

        private void UpdateClientBag(Socket s_ket, GoodsData targetGood, int num)
        {
            if (s_ket == null) return;

            //获取客户端信息
            var client = NetManager.GetInstance().GetClient(s_ket);
            //构建背包数据响应消息
            var response = new S_To_C_BagGoods();
            response.Bagitemlist.Add(new BagGoodsData 
            { 
                Data= targetGood,
                Num=num
            });
            //发送背包数据到客户端
            NetManager.GetInstance().SendMessage(NetID.S_To_C_BagGoods, response.ToByteArray(), s_ket);
        }

        private void SendBuyResult(Socket s_ket, bool v)
        {
            if (s_ket == null) return;
            //构建购买结果响应消息
            var response = new S_To_C_BuyGood() { Isbuy = v };
            NetManager.GetInstance().SendMessage(NetID.S_To_C_BuyGood, response.ToByteArray(), s_ket);
        }

        /// <summary>
        /// 处理客户端请求商店商品列表的消息
        /// </summary>
        /// <param name="obj"></param>
        private void C_To_S_ShopGoods_Handel(object obj)
        {
            try
            {
                object[] objlist = obj as object[];
                byte[] data = objlist[0] as byte[];
                Socket s_ket = objlist[1] as Socket;
                //通过网络管理器向客户端发送响应消息
                NetManager.GetInstance().SendMessage(NetID.S_To_C_ShopGoods, ConfigManager.GetInstance().goods.ToByteArray(), s_ket);
                Console.WriteLine(1111);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"处理商店商品请求异常：{ex.Message}");
            }
        }
    }
}

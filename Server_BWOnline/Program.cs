using Server_2304.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server_2304
{
    class Program
    {
        static void Main(string[] args)
        {
            //网络处理器启动
            NetManager.GetInstance().Start();
            //聊天处理器启动
            //ChatManager.GetInstance().Start();

            ConfigManager.GetInstance().OnShop();

            MainManager.GetInstance().Start();

            ShopManager.GetInstance().Start();
            

            Console.ReadKey();

        }

        
    }
}

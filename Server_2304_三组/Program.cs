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
            
            ShopConfig.GetInstance().Init();
            BagConfig.GetInstance().Init();
            PlayerConfig.GetInstance().InitPlayer();
            PlayerFightMgr.GetInstance().Init();
            RefreshBullet.GetInstance().Init();
            PlayerHpMgr.GetInstance().Init();
            PlayerAnimatorMgr.GetInstance().Init();
            
            Console.ReadKey();
        }

        
    }
}

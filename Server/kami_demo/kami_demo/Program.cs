using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kami_demo
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //网络处理器启动
            NetManager.GetInstance().Start();

            ConfigManager.GetInstance().OnStart();

            MainSurfaceManager.GetInstance().OnStart();

            //背包处理器启动
            BagManager.GetInstance().OnStart();



            Console.ReadKey();
        }
    }
}

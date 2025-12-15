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
    internal class MainManager:Singleton<MainManager>
    {
        public void Start()
        {
            MessageControll.GetInstance().AddListener(NetID.C_To_S_Main, C_To_S_Main_Handel);
        }

        private void C_To_S_Main_Handel(object obj)
        {
            var objlist = obj as object[];
            byte[] data = objlist[0] as byte[];
            Socket s_ket = objlist[1] as Socket;
            var getsocket =NetManager.GetInstance().GetClient(s_ket);
            var main = new S_To_C_Main();
            main.Money = (int)getsocket.money;
            NetManager.GetInstance().SendMessage(NetID.S_To_C_Main, main.ToByteArray(), s_ket);
        }
    }
}

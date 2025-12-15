using System.Net.Sockets;
using MyGame;
using Google;
using Google.Protobuf;

namespace Server_2304
{
    public class PlayerConfig:Singleton<PlayerConfig>
    {
        private int Glod;

        public void InitPlayer()
        {
            Glod = 10000;
            MessageControll.GetInstance().AddListener(NetID.C_To_S_GetPlayer,GetPlayerHandler);
        }

        private void GetPlayerHandler(object obj)
        {
            object[] ooo = obj as object[];
            Socket st = ooo[1] as Socket;
            
            S_To_C_GetPlayerData_Msg msg = new S_To_C_GetPlayerData_Msg();
            msg.Glod = Glod;
            NetManager.GetInstance().SendMessage(NetID.S_To_C_GetPlayer,msg.ToByteArray(),st);
        }

        public int GetGold()
        {
            return Glod;
        }
        public bool SetGold(int gold)
        {
            if (Glod + gold < 0)
            {
                return false;
            }
            else
            {
                Glod += gold;
                return true;
            }
        }
    }
}
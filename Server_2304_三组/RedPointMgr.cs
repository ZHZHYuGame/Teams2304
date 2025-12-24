using System.Net.Sockets;

namespace Server_2304
{
    using MyGame;
    using Google.Protobuf;
    using Google;
    public class RedPointMgr:Singleton<RedPointMgr>
    {
        public void RefreshRedPoint(UIRedPointType uiRedPointType,bool state,int num,Socket st)
        {
            S_To_C_RedPoint_Msg msg = new S_To_C_RedPoint_Msg();
            msg.Type = (int)uiRedPointType;
            msg.State = state;
            msg.Num = num;
            
            NetManager.GetInstance().SendMessage(NetID.S_To_C_RedPoint_Msg,msg.ToByteArray(),st);
        }
    }
}
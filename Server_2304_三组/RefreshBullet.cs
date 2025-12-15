using System.Net.Sockets;
using MyGame;
using Google;
using Google.Protobuf;
namespace Server_2304
{
    public class RefreshBullet:Singleton<RefreshBullet>
    {
        public void Init()
        {
            MessageControll.GetInstance().AddListener(NetID.C_To_S_Bullet,BulletInit);
            MessageControll.GetInstance().AddListener(NetID.C_To_S_ReomveBullet,ReomveBullet);
        }

        private void ReomveBullet(object obj)
        {
            object[] objs = obj as object[];
            byte[] bytes = objs[0] as byte[];
            Socket st = objs[1] as Socket;
            
            
            
            foreach (var item in NetManager.GetInstance().clientsList)
            {
                if (st != item.st)
                {
                    NetManager.GetInstance().SendMessage(NetID.S_To_C_Bullet,scMsg.ToByteArray(),item.st);
                }
            }
        }

        private void BulletInit(object obj)
        {
            object[] objs = obj as object[];
            byte[] bytes = objs[0] as byte[];
            Socket st = objs[1] as Socket;
            
            C_To_S_Bullet csMsg = C_To_S_Bullet.Parser.ParseFrom(bytes);
            csMsg.Atk = 20;
            S_To_C_Bullet scMsg = new S_To_C_Bullet();
            scMsg.Atk = 20;
            scMsg.X = csMsg.X;
            scMsg.Y = csMsg.Y;
            scMsg.Z = csMsg.Z;
            scMsg.PlayerId = csMsg.PlayerId;

            foreach (var item in NetManager.GetInstance().clientsList)
            {
                if (st != item.st)
                {
                    NetManager.GetInstance().SendMessage(NetID.S_To_C_Bullet,scMsg.ToByteArray(),item.st);
                }
            }
        }
    }
}
using System.Net.Sockets;
using Google;
using Google.Protobuf;
using MyGame;

namespace Server_2304
{
    /// <summary>
    /// 玩家动画管理器
    /// </summary>
    public class PlayerAnimatorMgr:Singleton<PlayerAnimatorMgr>
    {
        public void Init()
        {
            MessageControll.GetInstance().AddListener(NetID.C_To_S_PlayerAnimator,C_To_S_PlayerAnimatorHandler);
        }

        private void C_To_S_PlayerAnimatorHandler(object obj)
        {
            object[] objs = obj as object[];
            byte[] bytes = objs[0] as byte[];
            Socket st = objs[1] as Socket;
            C_To_S_PlayerAnimator csMsg = C_To_S_PlayerAnimator.Parser.ParseFrom(bytes);

            BroadAllPlayer(csMsg,st);
        }
        /// <summary>
        /// 广播给除了自己之外的玩家
        /// </summary>
        /// <param name="csMsg"></param>
        public void BroadAllPlayer(C_To_S_PlayerAnimator csMsg,Socket st)
        {
            S_To_C_PlayerAnimator scMsg = new S_To_C_PlayerAnimator();
            scMsg.PlayerId = csMsg.PlayerId;
            scMsg.AnimatorName = csMsg.AnimatorName;
            
            foreach (var item in NetManager.GetInstance().clientsList)
            {
                if (st != item.st)
                {
                    NetManager.GetInstance().SendMessage(NetID.S_To_C_PlayerAnimator,scMsg.ToByteArray(),item.st);
                }
            }
        }
    }
}
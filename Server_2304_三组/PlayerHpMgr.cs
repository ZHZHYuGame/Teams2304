using System.Collections.Generic;

namespace Server_2304
{
    public class PlayerHpMgr:Singleton<PlayerHpMgr>
    {
        private Dictionary<uint,uint> allPlayerHp = new Dictionary<uint,uint>();
        public void Init()
        {
            
        }
        
    }
}
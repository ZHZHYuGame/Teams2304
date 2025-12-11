using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetID 
{
     /// <summary>
        ///  客户端到服务器聊天消息
        /// </summary>
        public static int C_To_S_Chat_Msg = 1001;
        
        /// <summary>
        /// 服务器到客户端 
        /// </summary>
        public static int S_To_C_Chat_Msg = 1002;
        
        /// <summary>
        ///  客户端到服务器创建新角色
        /// </summary>
        public static int C_To_S_Create_Role_Msg = 1003;
        
        /// <summary>
        /// 服务器到客户端 
        /// </summary>
        public static int S_To_C_Create_Role_Msg = 1004;
        
        /// <summary>
        /// 更新角色信息
        /// </summary>
        public static int S_To_C_roles_Update_online_Msg = 1005;
        
        /// <summary>
        /// 客户端发送好友请求到服务器
        /// </summary>
        public static int C_To_S_GetFriendsInfo_msg = 1006;
    
        public static int S_To_C_GetFriendsInfo_msg = 1007;
    
        public static int C_To_S_Update_FriendInfo_msg = 1008;
        public static int S_To_C_Update_FriendInfo_msg = 1009;
    
        public static int C_To_S_Update_Request_msg = 1010;
        public static int S_To_C_Update_Request_msg = 1011;
    
        public static int C_To_S_GetPlayersInfo_msg = 1012;
    
        public static int S_To_C_GetPlayersInfo_msg = 1013;
        
        public static int S_To_C_Update_PlayerInfo_msg = 1014;
        
        public static int C_To_S_GetShopInfos_msg = 1015;
        public static int S_To_C_GetShopInfos_msg = 1016;
        
        

}

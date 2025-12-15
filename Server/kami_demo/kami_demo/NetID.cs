using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 网络消息id
/// </summary>
public class NetID 
{
    /// <summary>
    ///  客户端到服务器请求角色消息
    /// </summary>
    public static int C_To_S_GetPlayerData_Msg = 1001;

    /// <summary>
    ///  服务器反馈给客户端角色消息
    /// </summary>
    public static int S_To_C_GetPlayerData_Msg = 1002;

    /// <summary>
    ///  客户端到服务器使用物品消息
    /// </summary>
    public static int C_To_S_UseBagItem_Msg = 1003;


    /// <summary>
    ///  服务器反馈给客户端使用物品消息
    /// </summary>
    public static int S_To_C_UseBagItem_Msg = 1004;



}

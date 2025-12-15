using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 双端的网络通信ID
/// </summary>
public class NetID 
{
    // GoldRefresh_Msg = 1000,
    // C_To_S_GetShopData_Msg = 1001,
    // S_To_C_GetShopData_Msg = 1002,
    // C_To_S_GetsBagData_Msg = 1003,
    // S_To_C_GetsBagData_Msg = 1004,
    // C_To_S_BayShop_Msg = 1005,
    // S_To_C_S_BayShop_Msg = 1006,
    // C_To_S_GetPlayer_Msg = 1007,
    // S_To_C_GetPlayer_Msg = 1008,

    public static int C_To_S_PlayerOperation = 10001;
    public static int S_To_C_PlayerOperation = 10002;
    public static int C_To_S_Disconnect = 10003;
    public static int S_To_C_Disconnect = 10004;
    public static int C_To_S_Bullet = 10005;
    public static int S_To_C_Bullet = 10006;
    public static int C_To_S_ReomveBullet = 10007;
    public static int S_To_C_ReomveBullet = 10008;

}
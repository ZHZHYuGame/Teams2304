using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 双端的网络通信ID
/// </summary>
public static class NetID
{
    /// <summary>
    /// 客户端向服务器请求商城数据
    /// </summary>
    public static int C_To_S_ShopGoods = 1001;
    /// <summary>
    /// 服务器向客户端回馈商城数据
    /// </summary>
    public static int S_To_C_ShopGoods = 1002;
    /// <summary>
    /// 客户端向服务器请求背包数据
    /// </summary>
    public static int C_To_S_BagGoods = 1003;
    /// <summary>
    /// 服务器向客户端回馈背包数据
    /// </summary>
    public static int S_To_C_BagGoods = 1004;
    /// <summary>
    /// 客户端向服务器发送购买请求
    /// </summary>
    public static int C_To_S_BuyGood = 1005;
    /// <summary>
    /// 客户端向服务器回馈购买请求
    /// </summary>
    public static int S_To_C_BuyGood = 1006;
    /// <summary>
    /// 客户端向服务器发送获取物品请求
    /// </summary>
    public static int C_To_S_SendGood = 1007;
    /// <summary>
    /// 服务器向客户端回馈获取物品请求
    /// </summary>
    public static int S_To_C_SendGood = 1008;
    /// <summary>
    /// 客户端向服务器请求主面板数据
    /// </summary>
    public static int C_To_S_Main = 1009;
    /// <summary>
    /// 服务器向客户端回馈主面板数据
    /// </summary>
    public static int S_To_C_Main = 1010;
}

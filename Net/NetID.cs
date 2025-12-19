using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NetID
{
    /// <summary>
    /// �ͻ���������������̳�����
    /// </summary>
    public static int C_To_S_ShopGoods = 1001;
    /// <summary>
    /// ��������ͻ��˻����̳�����
    /// </summary>
    public static int S_To_C_ShopGoods = 1002;
    /// <summary>
    /// �ͻ�������������󱳰�����
    /// </summary>
    public static int C_To_S_BagGoods = 1003;
    /// <summary>
    /// ��������ͻ��˻�����������
    /// </summary>
    public static int S_To_C_BagGoods = 1004;
    /// <summary>
    /// �ͻ�������������͹�������
    /// </summary>
    public static int C_To_S_BuyGood = 1005;
    /// <summary>
    /// �ͻ����������������������
    /// </summary>
    public static int S_To_C_BuyGood = 1006;
    /// <summary>
    /// �ͻ�������������ͻ�ȡ��Ʒ����
    /// </summary>
    public static int C_To_S_SendGood = 1007;
    /// <summary>
    /// ��������ͻ��˻�����ȡ��Ʒ����
    /// </summary>
    public static int S_To_C_SendGood = 1008;
    /// <summary>
    /// �ͻ�����������������������
    /// </summary>
    public static int C_To_S_Main = 1009;
    /// <summary>
    /// ��������ͻ��˻������������
    /// </summary>
    public static int S_To_C_Main = 1010;
    /// <summary>
    /// 客户端向服务器请求注册
    /// </summary>
    public static int C_To_S_Register = 1011;
    /// <summary>
    /// 服务器反馈客户端注册结果
    /// </summary>
    public static int S_To_C_Register = 1012;
    /// <summary>
    /// 客户端向服务器请求登录
    /// </summary>
    public static int C_To_S_Log = 1013;
    /// <summary>
    /// 服务器反馈客户端登录结果
    /// </summary>
    public static int S_To_C_Log = 1014;
}

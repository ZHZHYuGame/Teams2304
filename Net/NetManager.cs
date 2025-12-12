using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
//using MyGame;
using UnityEngine;

public class NetManager : Singleton<NetManager>
{
    Socket st;
    /// <summary>
    /// 用来存储数据(粘包)
    /// </summary>
    private MyMemoryStream myStream = new MyMemoryStream();

    Action lua_Handle;

    byte[] receiveData = new byte[1024];
    /// <summary>
    /// 要处理的消息队列
    /// </summary>
    public Queue<byte[]> dataQue = new Queue<byte[]>();
    public void Start()
    {
        st = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        st.BeginConnect("10.161.25.87", 10086, Connect_To_Net_Handle, null);
    }

    private void Connect_To_Net_Handle(IAsyncResult ar)
    {
        st.EndConnect(ar);

        st.BeginReceive(receiveData, 0, receiveData.Length, SocketFlags.None, ReceiveHandle, null);

        Debug.Log("连接2304服务器成功");
    }

    private void ReceiveHandle(IAsyncResult ar)
    {
        try
        {
            int dataLen = st.EndReceive(ar);
            //接收客户端数据成功
            if (dataLen > 0)
            {
                //与客户端同步数据组成，数据拆分的结构、数据对应位置数据类型
                byte[] r_Bytes = new byte[dataLen];

                Buffer.BlockCopy(receiveData, 0, r_Bytes, 0, dataLen);
                //如有剩余未处理的包，则在包的后面进入写入
                myStream.Position = myStream.Length;
                //数据已经存进来了
                myStream.Write(r_Bytes, 0, r_Bytes.Length);
                //判断是不是到少有一个不完整的包(为什么？因为还没到判断完整包的地方)
                while (myStream.Length >= 2)
                {
                    //现在位置在写入数据的长度的位置
                    myStream.Position = 0;
                    //包头的值 = 包体的长度
                    ushort titleLen = myStream.ReadUshort();
                    //包的整体长度
                    int allLen = titleLen + 2;
                    //这里才是判断是不是有一个可以处理的完整的包
                    if (myStream.Length >= allLen)
                    {
                        //这里已经开始读消息的内容(id + 内容)
                        byte[] tampData = new byte[titleLen];
                        myStream.Read(tampData, 0, tampData.Length);
                        this.dataQue.Enqueue(tampData);

                        //int netId = BitConverter.ToInt32(tampData, 0);

                        //byte[] desc = new byte[tampData.Length - 4];
                        //Buffer.BlockCopy(tampData, 4, desc, 0, desc.Length);
                        //MessageControll.GetInstance().Dispach(netId, desc);

                        int shLen = (int)myStream.Length - allLen;
                        //还有未处理完的数据包
                        if (shLen > 0)
                        {
                            //存剩余数据
                            byte[] shData = new byte[shLen];
                            myStream.Read(shData, 0, shData.Length);
                            //请空流
                            myStream.Position = 0;
                            myStream.SetLength(0);
                            //将剩余的数据写到缓冲区
                            myStream.Write(shData, 0, shData.Length);
                        }
                        else
                        {
                            //请空流
                            myStream.Position = 0;
                            myStream.SetLength(0);
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                st.BeginReceive(receiveData, 0, receiveData.Length, SocketFlags.None, ReceiveHandle, null);
            }
            //接收客户端数据异常
            else
            {
                //删除客户端连接

            }
        }
        catch (Exception)
        {

        }

    }
    /// <summary>
    /// 客户端发送数据(字节流)，PB
    /// </summary>
    /// <param name="id">游戏功能ID</param>
    /// <param name="contextData">游戏要发送的内容</param>
    public void SendMessage(int id, byte[] contextData)
    {
        //功能ID的byte[]
        byte[] idData = BitConverter.GetBytes(id);
        //new一个功能ID与内容的长度的byte[]
        byte[] data = new byte[idData.Length + contextData.Length];
        //将功能ID的byte[]写到data里
        Buffer.BlockCopy(idData, 0, data, 0, idData.Length);
        //将内容的byte[]写到data里(在idData的后面)
        Buffer.BlockCopy(contextData, 0, data, idData.Length, contextData.Length);
        //
        byte[] makeDatas = MakeData(data);
        //send message to server
        st.BeginSend(makeDatas, 0, makeDatas.Length, SocketFlags.None, Client_Send_To_Net_Handle, null);
    }
    /// <summary>
    /// 转换？(发送的内容转换成包头与包体格式，为什么？处理粘包)
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public byte[] MakeData(byte[] data)
    {
        //using 决定作用域里的实例的生命周期，运行结束后自行清除
        using (MyMemoryStream myStream = new MyMemoryStream())
        {
            ushort len = (ushort)data.Length;
            myStream.WriteUShort(len);
            myStream.Write(data, 0, data.Length);
            return myStream.ToArray();
        }
    }
    public void SendMessage(int id, int value)
    {
        byte[] id_Byte = BitConverter.GetBytes(id);
        byte[] value_Byte = BitConverter.GetBytes(value);
        byte[] send_Byte = new byte[id_Byte.Length + value_Byte.Length];
        Buffer.BlockCopy(id_Byte, 0, send_Byte, 0, id_Byte.Length);
        Buffer.BlockCopy(value_Byte, 0, send_Byte, 4, value_Byte.Length);

        st.BeginSend(send_Byte, 0, send_Byte.Length, SocketFlags.None, Client_Send_To_Net_Handle, null);
    }

    private void Client_Send_To_Net_Handle(IAsyncResult ar)
    {
        try
        {
            st.EndSend(ar);
        }
        catch (Exception)
        {
            Debug.Log("向服务器发送数据失败");
        }
        
    }

    public void Update()
    {
        while (dataQue.Count > 0)
        {
            byte[] data = dataQue.Dequeue();

            int netId = BitConverter.ToInt32(data, 0);

            byte[] desc = new byte[data.Length - 4];
            Buffer.BlockCopy(data, 4, desc, 0, desc.Length);
            //MessageControll消息中心派发
            //MessageControll.GetInstance().Dispach(netId, desc, true, 1, "asdf", 333);

            //Controll--安全字典中心 -- ExecuteCommand 处理
            //AppFacade.GetInstance().controll.ExecuteCommand(new Notification(netId.ToString(), desc, NotificationType.Game_Net_Type));
            Net_To_Lua_Data toLuaData = new Net_To_Lua_Data()
            {
                netID = netId,
                byteData = desc
            };
            //S_To_C_GetShopData_Msg s_msg = S_To_C_GetShopData_Msg.Parser.ParseFrom(toLuaData.byteData);
            //Debug.Log("byte" + s_msg.Shopdatas[0].Id);
            CShap_Handle_Lua_Tool.GetInstance().Call_Lua_Net_Msg(toLuaData);
        }
    }

    public void ExitGame()
    {
        st.Shutdown(SocketShutdown.Both);
        st.Close();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using MyGame;
using UnityEngine;

public class NetManager :Singleton<NetManager>
{
   
   


    Socket st;
    /// <summary>
    /// 网络数据流
    /// </summary>
    private MyMemoryStream myStream = new MyMemoryStream();

    byte[] receiveData = new byte[1024];
    /// <summary>
    /// 网络消息队列
    /// </summary>
    public Queue<byte[]> dataQue = new Queue<byte[]>();
    public void Start()
    {
        st = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        st.BeginConnect("127.0.0.1", 11008, Connect_To_Net_Handle, null);
    }

    private void Connect_To_Net_Handle(IAsyncResult ar)
    {
        st.EndConnect(ar);

        st.BeginReceive(receiveData, 0, receiveData.Length, SocketFlags.None, ReceiveHandle, null);

        
    }

    private void ReceiveHandle(IAsyncResult ar)
    {
        try
        {
            int dataLen = st.EndReceive(ar);
            //??????????????
            if (dataLen > 0)
            {
                
                byte[] r_Bytes = new byte[dataLen];

                Buffer.BlockCopy(receiveData, 0, r_Bytes, 0, dataLen);
                //???????δ??????????????????????д??
                myStream.Position = myStream.Length;
                //??????????????
                myStream.Write(r_Bytes, 0, r_Bytes.Length);
                //?ж???????????????????????(??????????????ж???????????)
                while (myStream.Length >= 2)
                {
                    //????λ????д???????????λ??
                    myStream.Position = 0;
                    //?????? = ????????
                    ushort titleLen = myStream.ReadUshort();
                    //???????峤??
                    int allLen = titleLen + 2;
                    //判断是不是一个完整的包
                    if (myStream.Length >= allLen)
                    {
                        //?????????????????????(id + ????)
                        byte[] tampData = new byte[titleLen];
                        myStream.Read(tampData, 0, tampData.Length);
                        this.dataQue.Enqueue(tampData);
                        int shLen = (int)myStream.Length - allLen;
                        //????δ????????????
                        if (shLen > 0)
                        {
                            //?????????
                            byte[] shData = new byte[shLen];
                            myStream.Read(shData, 0, shData.Length);
                            //?????
                            myStream.Position = 0;
                            myStream.SetLength(0);
                            //??????????д????????
                            myStream.Write(shData, 0, shData.Length);
                        }
                        else
                        {
                            //?????
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
            //??????????????
            else
            {
                //????????????

            }
        }
        catch (Exception)
        {

        }

    }
    /// <summary>
    /// 发送网络消息到服务器
    /// </summary>
    /// <param name="id">???????ID</param>
    /// <param name="contextData">?????????????</param>
    public void SendMessage(int id, byte[] contextData)
    {
        //????ID??byte[]
        byte[] idData = BitConverter.GetBytes(id);
        //new???????ID???????????byte[]
        byte[] data = new byte[idData.Length + contextData.Length];
        //??????ID??byte[]д??data??
        Buffer.BlockCopy(idData, 0, data, 0, idData.Length);
        //???????byte[]д??data??(??idData?????)
        Buffer.BlockCopy(contextData, 0, data, idData.Length, contextData.Length);
        //
        byte[] makeDatas = MakeData(data);
        //send message to server
        st.BeginSend(makeDatas, 0, makeDatas.Length, SocketFlags.None, Client_Send_To_Net_Handle, null);
    }
    /// <summary>
    /// ?????(?????????????????????????????????????)
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public byte[] MakeData(byte[] data)
    {
        //声明字节流拼接包头和包体组成一个完整的包
        using (MyMemoryStream myStream = new MyMemoryStream())
        {
            ushort len = (ushort)data.Length;
            myStream.WriteUShort(len);
            myStream.Write(data, 0, data.Length);
            return myStream.ToArray();
        }
    }
    // public void SendMessage(int id, int value)
    // {
    //     byte[] id_Byte = BitConverter.GetBytes(id);
    //     byte[] value_Byte = BitConverter.GetBytes(value);
    //     byte[] send_Byte = new byte[id_Byte.Length + value_Byte.Length];
    //     Buffer.BlockCopy(id_Byte, 0, send_Byte, 0, id_Byte.Length);
    //     Buffer.BlockCopy(value_Byte, 0, send_Byte, 4, value_Byte.Length);
    //
    //     st.BeginSend(send_Byte, 0, send_Byte.Length, SocketFlags.None, Client_Send_To_Net_Handle, null);
    // }

    private void Client_Send_To_Net_Handle(IAsyncResult ar)
    {
        try
        {
            st.EndSend(ar);
        }
        catch (Exception)
        {
            Debug.Log("??????????????????");
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
            // MessageControl.GetInstance().Dispach(netId, desc);
            Net_To_Lua_Data toLuaData = new Net_To_Lua_Data()
            {
                netID = netId,
                byteData = desc
            };
            CShap_Handle_Lua_Tool.GetInstance().Call_Lua_Net_Msg(toLuaData);
            
            S_To_C_GetShopInfos_Msg s_msg = new S_To_C_GetShopInfos_Msg();
           
           
        }
    }


    public void ExitGame()
    {
        st.Shutdown(SocketShutdown.Both);  
        st.Close();
    }
}



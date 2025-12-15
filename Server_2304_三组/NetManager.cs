using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


public class NetManager:Singleton<NetManager>
{
    Socket socket;

    byte[] data = new byte[1024];
    /// <summary>
    /// 用来存储数据(粘包)
    /// </summary>
    private MyMemoryStream myStream = new MyMemoryStream();
    /// <summary>
    /// 管理所有客户端
    /// </summary>
    public List<Client> clientsList = new List<Client>();
    public void Start()
    {
        //单一服务器
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //设定服务器接收任一连接服务器的客户端
        socket.Bind(new IPEndPoint(IPAddress.Any, 11008));
        //服务器可接收的连接客户端数量
        socket.Listen(100);

        Console.WriteLine("服务器启动成功");
        //监听客户端连接的触发
        socket.BeginAccept(AcceptHandle, socket);
    }
    /// <summary>
    /// 监听客户端连接到服务器的触发
    /// </summary>
    /// <param name="ar"></param>
    private void AcceptHandle(IAsyncResult ar)
    {
        Socket st = socket.EndAccept(ar);

        IPEndPoint ip = st.RemoteEndPoint as IPEndPoint;

        Console.WriteLine($"连接进来的客户端IP = {ip.Address}  端口号 = {ip.Port}");

        Client c = new Client()
        {
            ip = ip.Address.ToString(),
            prot = ip.Port,
            st = st,
            name = ""
        };

        clientsList.Add(c);

        st.BeginReceive(data, 0, data.Length, SocketFlags.None, ReceiveHandle, st);

        socket.BeginAccept(AcceptHandle, socket);
    }

    private void ReceiveHandle(IAsyncResult ar)
    {
        Socket st = ar.AsyncState as Socket;
        try
        {
            int dataLen = st.EndReceive(ar);
            //接收客户端数据成功
            if (dataLen > 0)
            {
                //与客户端同步数据组成，数据拆分的结构、数据对应位置数据类型
                byte[] r_Bytes = new byte[dataLen];
                
                Buffer.BlockCopy(data, 0, r_Bytes, 0, dataLen);
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

                        int netID = BitConverter.ToInt32(tampData, 0);
                        //内容
                        byte[] descByte = new byte[tampData.Length - 4];
                        Buffer.BlockCopy(tampData, 4, descByte, 0, descByte.Length);
                        MessageControll.GetInstance().Dispach(netID, descByte, st, 1, "", true);

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
                st.BeginReceive(data, 0, data.Length, SocketFlags.None, ReceiveHandle, st);
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
    public void SendMessage(int id, byte[] contextData, Socket st)
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
        st.BeginSend(makeDatas, 0, makeDatas.Length, SocketFlags.None, SynSend, st);
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

    /// <summary>
    /// 发送数据结束
    /// </summary>
    /// <param name="ar"></param>
    private void SynSend(IAsyncResult ar)
    {
        Socket st = ar.AsyncState as Socket;
        try
        {
            st.EndSend(ar);
        }
        catch (Exception)
        {
            
        }
    }

    public void RevomeSt(Socket st)
    {
        foreach (var item in clientsList)
        {
            if (item.st == st)
            {
                clientsList.Remove(item);
                break;
            }
        }
        st.Close();
        st.Dispose();
    }

    public List<Client> Get_ClientList()
    {
        return clientsList;
    }
}



public class Client
{
    public Socket st;
    public string ip;
    public int prot;
    public string name;
}

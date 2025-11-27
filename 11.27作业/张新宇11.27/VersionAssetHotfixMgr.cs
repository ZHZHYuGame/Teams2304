using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
/// <summary>
/// 资源更新框架
/// 1.版本下载， 并与本地P目录下的版本号文件对比
/// </summary>
public class VersionAssetHotfixMgr : MonoBehaviour
{
    private void Awake()
    {
        MessageControll.GetInstance().AddListener(Client_Const_Event.Hotfix_Confirm_Event, Hotfix_Confirm_Event_Handle);
    }
    /// <summary>
    /// 资源服务器IP_Tirle
    /// </summary>
    string http_Server_IP = "127.0.0.1/Game";
    /// <summary>
    /// 服务器
    /// </summary>
    VersionData server_Version;
    /// <summary>
    /// 服务器资源清单
    /// </summary>

    string server_AssetsMainfast_Str;
    /// <summary>
    /// 服务器的AB资源
    /// </summary>
    Dictionary<string,ABAsset> server_Assets_Dict = new Dictionary<string,ABAsset>();

    /// <summary>
    /// 下载资源队列（必下载）
    /// </summary>
    Queue<ABAsset> load_Asset_Queue = new Queue<ABAsset>();

    /// <summary>
    /// 删除资源队列
    /// </summary>
    Queue<ABAsset> delete_Asset_Queue= new Queue<ABAsset>();    
    
    public void Start()
    {
        
        string server_Verion_txt = $"{http_Server_IP}/Version.txt";

        //版本下载，并与本地P目录下的版本号文件对比
        Load_Asset_Server_Version(server_Verion_txt, (data) =>
        {
            string s_Version_Str = Encoding.UTF8.GetString (data);
            
            server_Version =new VersionData(s_Version_Str);
            string l_Version_Path = $"{Application.persistentDataPath}/Version.txt";
            VersionData local_Version = null;
            //Directory.Exists(l_Version_Path)
            if (File.Exists(l_Version_Path))
            {
                local_Version=new VersionData(File.ReadAllText(l_Version_Path));
                Debug.Log(local_Version);
            }
            if(local_Version==null)
            {
                DownLoad_All_Assets();
            }
            else
            {
                //大版本更新
                if (server_Version.big > local_Version.big)
                {
                    DownLoad_All_Assets();
                }
                //中版本更新
                else if (server_Version.middle > local_Version.middle)
                {
                    
                }
                //小版本更新
                else if (server_Version.small > local_Version.small)
                {
                   // Hotfix_Version_Assets_AllComparison();
                }
                else
                {
                    //无更新需求，走正常进入游戏

                }
            }
        });
    }
    /// <summary>
    /// 版本号文件的下载与I/O
    /// </summary>
    /// <param name="path"></param>
    /// <param name="action"></param>
    void Load_Asset_Server_Version(string path, Action<byte[]> action)
    {
        StartCoroutine(LoadGameAsset(path,action));
    }
    IEnumerator LoadGameAsset(string path, Action<byte[]> action)
    {
        
        UnityWebRequest unityWeb=UnityWebRequest.Get(path);
        
        UnityWebRequestAsyncOperation op = unityWeb.SendWebRequest();
        Thread.Sleep(100);
        if (op.isDone)
        {
            
            action(unityWeb.downloadHandler.data);
        }
        yield return null;
    }
    /// <summary>
    /// 下载服务器的资源清单文件（里面记录的时所有的这个版本的资源文件）
    /// </summary>
    /// <param name="path"></param>
    /// <param name="action"></param>
    void Load_Asset_Server_AssetMainfast(string path, Action<byte[]> action)
    {
        StartCoroutine(LoadGameAsset(path, action));
    }
    /// <summary>
    /// 第一次
    /// </summary>
    void DownLoad_All_Assets()
    {
        //资源清单文件
        string path = $"{http_Server_IP}/AssetMainfast.txt";
        //下载服务器的资源清单文件
        Load_Asset_Server_AssetMainfast(path, (data) =>
        {
            //通过Web下载读取的byte[] 转成对应txt 字符串
            server_AssetsMainfast_Str = Encoding.UTF8.GetString(data);

            string[] abStrArr = server_AssetsMainfast_Str.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (string aStr in abStrArr)
            {
                ABAsset ab = new ABAsset(aStr);
                //记录所有的需要服务器AB数据资源
                load_Asset_Queue.Enqueue(ab);
            }
            //开始下载资源
            DownLoad_Asset(load_Asset_Queue.Dequeue());
        });
    }
    /// <summary>
    /// 热更新版本资源(所有版本资源对比)
    /// </summary>
    void Hotfix_Version_Assets_AllComparison()
    {
        //资源清单文件
        string path = $"{http_Server_IP}/AssetMainfast.txt";
        //下载服务器的资源清单文件
        Load_Asset_Server_AssetMainfast(path, (data) =>
        {
            //通过Web下载读取的byte[] 转成对应txt 字符串
            string assetMainfast_Str=Encoding.UTF8.GetString(data);
            server_AssetsMainfast_Str = assetMainfast_Str;
            Asset_ABAsset_Add_To_Dict(server_Assets_Dict,assetMainfast_Str);
            string local_AssetMainfast_Path = $"{Application.persistentDataPath}/AssetMainfast.txt";
            string local_Asset_Str= File.ReadAllText(local_AssetMainfast_Path);
            Dictionary<string, ABAsset> local_Assets_Dict = new Dictionary<string, ABAsset>();
            Asset_ABAsset_Add_To_Dict(local_Assets_Dict,local_Asset_Str);
            Version_Asset_Compare_S_And_L(local_Assets_Dict);
            //下载更新
            if(load_Asset_Queue.Count>0)
            {
                DownLoad_Asset(load_Asset_Queue.Dequeue());
            }
            
        });
    }
    void Asset_ABAsset_Add_To_Dict(Dictionary<string,ABAsset> dict,string abStr)
    {
        string[] abStrArr = abStr.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);

        foreach (string aStr in abStrArr)
        {
            ABAsset ab = new ABAsset(aStr);
            //记录所有的服务器AB数据资源
            dict.Add(ab.abName, ab);
        }
    }
    /// <summary>
    /// 服务器与本地对比AB资源状态，确定下载列表数据
    /// </summary>
    /// <param name="local_Assets_Dict"></param>
    void Version_Asset_Compare_S_And_L(Dictionary<string ,ABAsset> local_Assets_Dict)
    {
        foreach(var s_Asset in server_Assets_Dict)
        {
            //判断本地AB资源是否有服务器上的资源
            if(local_Assets_Dict.ContainsKey(s_Asset.Key))
            {
                //判断资源的MD5码是否相同，不同代表有更新
                if (s_Asset.Value.md5 != local_Assets_Dict[s_Asset.Key].md5)
                {
                    load_Asset_Queue.Enqueue(s_Asset.Value);
                }
            }
            //本地没有服务器上的资源，代表是新增文件（必下载）
            else
            {
                load_Asset_Queue.Enqueue(s_Asset.Value);
            }
        }
        //反向判断，找到是否有删除AB资源
        foreach(var l_Asset in local_Assets_Dict)
        {
            if(!server_Assets_Dict.ContainsKey(l_Asset.Key))
            {
                l_Asset.Value.isDel = true;
                delete_Asset_Queue.Enqueue(l_Asset.Value);
            }
        }
    }
    /// <summary>
    /// 下载服务器的具体AB资源
    /// </summary>
    /// <param name="path"></param>
    /// <param name="action"></param>
    void Load_Asset_Server_Asset(string path, Action<byte[]> action)
    {
        StartCoroutine(LoadGameAsset(path, action));
    }
    /// <summary>
    /// 下载具体的某个AB资源
    /// </summary>
    /// <param name="ast"></param>
    void DownLoad_Asset(ABAsset ast)
    {
        string path = $"{http_Server_IP}/{ast.abName}";
        Load_Asset_Server_Asset(path, (data) =>
        {
            //本地P目录下的资源路径
            string local_Asset_Path = $"{Application.persistentDataPath}/{ast.abName}";
            
            if(Directory.Exists(local_Asset_Path))
            {
                File.Delete(local_Asset_Path);
            }
            //判断对应路径文件夹不存在，不存在创建
            if(Directory.Exists(local_Asset_Path))
            {
                Directory.CreateDirectory(local_Asset_Path);
            }
            //写到对应的本地P目录
            File.WriteAllBytes(local_Asset_Path,data);
            if(load_Asset_Queue.Count>0)
            {
                DownLoad_Asset(load_Asset_Queue.Dequeue());
            }
            else
            {
                //保存Version号文件
                Save_Server_VerSicon();
                //保存资源清单文件
                Save_Server_AssetsMainfast();
                //进入游戏，通知进入游戏事件抛出（弹出退出游戏按钮）
            }
            

        });
    }
    /// <summary>
    /// 保存服务器的版本号到本地P目录
    /// </summary>
    void Save_Server_VerSicon()
    {
        File.WriteAllText($"{Application.persistentDataPath}/Version.txt",server_Version.ToString());
    }
    void Save_Server_AssetsMainfast()
    {
        File.WriteAllText($"{Application.persistentDataPath}/AssetMainfast.txt", server_AssetsMainfast_Str);
    }

    internal void Hotfix_Confirm_Event_Handle(object obj)
    {
        Hotfix_Version_Assets_AllComparison();
    }
}

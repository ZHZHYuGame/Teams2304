using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using System.IO;
using System.Threading;

/// <summary>
/// 资源更新框架
/// 1.版本下载，并与本地P目录下的版本号文件对比
/// 2.
/// </summary>
public class VersionAssetHotfixMgr : MonoBehaviour
{
    /// <summary>
    /// 资源服务器的IP Title
    /// </summary>
    string http_Asset_Server_IP = "http://127.0.0.1/Game2304";
    /// <summary>
    /// 服务器版本号
    /// </summary>
    VersionData server_Version;
    /// <summary>
    /// 资源服务器的资源清单内容
    /// </summary>
    string server_AssetMainfast_Str;
    /// <summary>
    /// 服务器的AB资源
    /// </summary>
    Dictionary<string, ABAsset> server_Assets_Dict = new Dictionary<string, ABAsset>();
    /// <summary>
    /// 下载资源队列（必下载）
    /// </summary>
    Queue<ABAsset> load_Asset_Queue = new Queue<ABAsset>();

    public void Start()
    {
        MessageControll.Ins.AddListener(NetID.Hotfix_Confirm_Event, Hotfix_Confirm_Event_Handle);
        //版本下载，并与本地P目录下的版本号文件对比
        string server_Verion_txt = $"{http_Asset_Server_IP}/Version.txt";
        
        Load_Asset_Server_Version(server_Verion_txt, (data)=>
        {
            string s_Version_Str = Encoding.UTF8.GetString(data);
            //资源服务器的版本号
            server_Version = new VersionData(s_Version_Str);
            //本地资源的版本号
            string l_Version_Path = $"{Application.persistentDataPath}/Version.txt";
            VersionData local_Version = null;
            //判断本地资源版本号文件是否存在
            if (Directory.Exists(l_Version_Path))
            {
                local_Version = new VersionData(File.ReadAllText(l_Version_Path));
            }
            //不存本地版本号文件，代表第一次下载
            if (local_Version == null)
            {
                DownLoad_All_Assets();
            }
            //存在，走相关的资源更新逻辑
            else
            {
                
                //大版本更新
                if(server_Version.big > local_Version.big)
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
                    Hotfix_Version_Assets_AllComparison();
                }
                else
                {
                    //无更新需求，走正常进入游戏

                }
            }
            
        });
    }

    private void Hotfix_Confirm_Event_Handle(object obj)
    {
        Hotfix_Version_Assets_AllComparison();
    }

    /// <summary>
    /// 版本号文件的下载与I/O
    /// </summary>
    /// <param name="path"></param>
    /// <param name="complete"></param>
    void Load_Asset_Server_Version(string path, Action<byte[]> complete)
    {
        StartCoroutine(LoadGameAsset(path, complete));
    }

    IEnumerator LoadGameAsset(string path, Action<byte[]> complete)
    {

        UnityWebRequest unityWeb = UnityWebRequest.Get(path);

        UnityWebRequestAsyncOperation op = unityWeb.SendWebRequest();
        Thread.Sleep(1000);
        if (op.isDone)
        {
            complete(unityWeb.downloadHandler.data);
        }
        yield return null;
    }
    /// <summary>
    /// 下载服务器的资源清单文件（里面记录的是所有的这个版本的资源文件）
    /// </summary>
    /// <param name="path"></param>
    /// <param name="complete"></param>
    void Load_Asset_Server_AssetMainfast(string path, Action<byte[]> complete)
    {
        StartCoroutine(LoadGameAsset(path, complete));
    }
    /// <summary>
    /// 第一次全部资源下载
    /// </summary>
    void DownLoad_All_Assets()
    {
        //资源清单文件
        string path = $"{http_Asset_Server_IP}/AssetMainfast.txt";
        //下载服务器的资源清单文件
        Load_Asset_Server_AssetMainfast(path, (data) =>
        {
            //通过Web下载读取的byte[] 转成对应的txt 字符串内容
            server_AssetMainfast_Str = Encoding.UTF8.GetString(data);
            //拆分每个资源的结构
            string[] abStrArr = server_AssetMainfast_Str.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (var aStr in abStrArr)
            {
                ABAsset ab = new ABAsset(aStr);
                //记录所有的需要下载服务器AB数据信息
                load_Asset_Queue.Enqueue(ab);
            }
            //开始下载资源
            DownLoad_Asset(load_Asset_Queue.Dequeue());
        });
    }

    /// <summary>
    /// 热更新版本资源（所有版本资源对比）
    /// </summary>
    void Hotfix_Version_Assets_AllComparison()
    {
        //资源清单文件
        string path = $"{http_Asset_Server_IP}/AssetMainfast.txt";
        //下载服务器的资源清单文件
        Load_Asset_Server_AssetMainfast(path, (data) =>
        {
            //通过Web下载读取的byte[] 转成对应的txt 字符串内容
            string assetMainfast_Str = Encoding.UTF8.GetString(data);
            //拆分每个资源的结构
            string[] abStrArr = assetMainfast_Str.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (var aStr in abStrArr)
            {
                ABAsset ab = new ABAsset(aStr);
                //记录所有的服务器AB数据信息
                server_Assets_Dict.Add(ab.abName, ab);
            }
        });
    }
    /// <summary>
    /// 下载服务器的具体AB资源
    /// </summary>
    /// <param name="path"></param>
    /// <param name="complete"></param>
    void Load_Asset_Server_Asset(string path, Action<byte[]> complete)
    {
        StartCoroutine(LoadGameAsset(path, complete));
    }
    /// <summary>
    /// 下载具体的某个AB资源
    /// </summary>
    /// <param name="ast"></param>
    void DownLoad_Asset(ABAsset ast)
    {
        string path = $"{http_Asset_Server_IP}/{ast.abName}";

        Load_Asset_Server_Asset(path, (data) =>
        {
            //本地P目录下的资源路径
            string local_Asset_Path = $"{Application.persistentDataPath}/{ast.abName}";

            if (Directory.Exists(local_Asset_Path))
            {
                File.Delete(local_Asset_Path);
            }
            //判断对应路径文件夹存不存在，不存在就创建
            if (Directory.Exists(local_Asset_Path))
            {
                Directory.CreateDirectory(local_Asset_Path);
            }

            //写到对应的本地P目录下
            File.WriteAllBytes(local_Asset_Path, data);
            //一个资源下载完毕
            if (load_Asset_Queue.Count > 0)
            {
                DownLoad_Asset(load_Asset_Queue.Dequeue());
            }
            else
            {
                //保存Version号文件
                Save_Server_Version();
                //保存资源清单文件
                Save_Server_AssetsMainfast();
                //进入游戏，通知进入游戏事件抛出（弹出退出游戏按钮）

            }

        });
    }
    /// <summary>
    /// 保存服务器的版本号到本地P目录
    /// </summary>
    void Save_Server_Version()
    {
        File.WriteAllText($"{Application.persistentDataPath}/Version.txt", server_Version.ToString());
    }
    /// <summary>
    /// 保存服务器的资源清单文件到本地P目录
    /// </summary>
    void Save_Server_AssetsMainfast()
    {
        File.WriteAllText($"{Application.persistentDataPath}/AssetMainfast.txt", server_AssetMainfast_Str);
    }
}

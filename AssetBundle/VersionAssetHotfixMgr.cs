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
    string http_Asset_Server_IP = "10.161.25.106/Game";
    /// <summary>
    /// 服务器版本号
    /// </summary>
    VersionData server_Version;
    /// <summary>
    /// 资源服务器的资源清单内容
    /// </summary>
    string server_AssetMainfast_Str;
    
    /// <summary>
    /// 本地版本号
    /// </summary>
    VersionData local_Version;
    
    /// <summary>
    /// 服务器的AB资源
    /// </summary>
    Dictionary<string, ABAsset> server_Assets_Dict = new Dictionary<string, ABAsset>();
    /// <summary>
    /// 下载资源队列（必下载）
    /// </summary>
    Queue<ABAsset> load_Asset_Queue = new Queue<ABAsset>();
    
    /// <summary>
    /// 删除资源队列
    /// </summary>
    Queue<ABAsset> delete_Asset_Queue = new Queue<ABAsset>();

    public static VersionAssetHotfixMgr ins;
    private void Awake()
    {
        ins = this;
    }

    public void Start()
    {
        //版本下载，并与本地P目录下的版本号文件对比
        string server_Verion_txt = $"{http_Asset_Server_IP}/Version.txt";

        Load_Asset_Server_Version(server_Verion_txt, (data)=>
        {
            string s_Version_Str = Encoding.UTF8.GetString(data);
            //资源服务器的版本号
            server_Version = new VersionData(s_Version_Str);
            //本地资源的版本号
            string l_Version_Path = $"{Application.persistentDataPath}/Version.txt";
            
            //判断本地资源版本号文件是否存在
            if (File.Exists(l_Version_Path))
            {
                local_Version = new VersionData(File.ReadAllText(l_Version_Path));
                
            }
            //不存在，第一次下载版本全部资源
            if (local_Version == null)
            {
                Load_Assets_Server_Version_Mainfest();
                DownLoad_All_Assets();
            }
            //存在，走相关的资源更新逻辑
            else
            {
                //大版本更新
                if(server_Version.big > local_Version.big)
                {
                    Load_Assets_Server_Version_Mainfest();
                    DownLoad_All_Assets();
                }
                //中版本更新
                else if (server_Version.middle > local_Version.middle)
                {
                    
                }
                //小版本更新
                else if (server_Version.small > local_Version.small)
                {
                    
                }
                else
                {
                    //无更新需求，走正常进入游戏

                }
            }
            
        });
    }

    void Load_Assets_Server_Version_Mainfest()
    {
        //下载服务器版本清单文件
        string sever_version_mainfest_path =
            $"{http_Asset_Server_IP}/{server_Version.ToString()}/{server_Version.ToString()}.u3d";
        Load_Asset_Server_Version(sever_version_mainfest_path, (data) =>
        {
            //判断本地是否第一次下载资源,不是第一次就卸载之前的版本清单文件
            if (local_Version !=null )
            {
                string local_version_mainfest_path = $"{Application.persistentDataPath}/{local_Version.ToString()}";
                File.Delete(local_version_mainfest_path);
            }
            
            File.WriteAllBytes($"{Application.persistentDataPath}/{server_Version.ToString()}",data);
           
        });
    }

    // /// <summary>
    // /// 热更新回调
    // /// </summary>
    // /// <param name="obj"></param>
    // private void Hotfix_AbAsset_Handle(object obj)
    // {
    //     Load_Assets_Server_Version_Mainfest();
    //     Hotfix_Version_Assets_AllComparison();
    // }

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
        yield return new WaitForSeconds(0.1f);
        while (!op.isDone)
        {
            yield return null;
        }
        if (op.isDone)
        {
            complete(unityWeb.downloadHandler.data);
        }
        
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
        
        //对应版本的资源清单文件路径
        string path = $"{http_Asset_Server_IP}/{server_Version.ToString()}/AssetMainfast.txt";
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
            server_AssetMainfast_Str = assetMainfast_Str;
            //整理服务器资源清单
            ABAssets_Add_dict(server_Assets_Dict,assetMainfast_Str);
            
            //读取本地资源清单
            string local_AssetMainfast_path = $"{Application.persistentDataPath}/AssetMainfast.txt";
            string local_Asset_str = File.ReadAllText(local_AssetMainfast_path);
            //整理本地资源清单
            Dictionary<string, ABAsset> local_Assets_dict = new Dictionary<string, ABAsset>();
            ABAssets_Add_dict(local_Assets_dict, local_Asset_str);
            Version_Asset_compare_S_And_L(local_Assets_dict);
            
            //下载资源
            if (load_Asset_Queue.Count>0)
            {
                DownLoad_Asset(load_Asset_Queue.Dequeue());
            }
            
        });
    }
    
    /// <summary>
    /// 资源清单整理
    /// </summary>
    /// <param name="dict"></param>
    void ABAssets_Add_dict(Dictionary<string,ABAsset> dict,string abStr)
    {
        //拆分每个资源的结构
        string[] abStrArr = abStr.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);

        foreach (var aStr in abStrArr)
        {
            ABAsset ab = new ABAsset(aStr);
            //记录所有的服务器AB数据信息
            dict.Add(ab.abName, ab);
        }
    }


    void Version_Asset_compare_S_And_L(Dictionary<string ,ABAsset>local_Assets_dict)
    {
        foreach (var s_Asset in server_Assets_Dict)
        {
            //判断贝蒂AB资源是否含有服务器上的AB资源
            if (local_Assets_dict.ContainsKey(s_Asset.Key))
            {
                //判断资源的MD5吗是否相同，不同代表有更新
                if (s_Asset.Value.md5 != local_Assets_dict[s_Asset.Key].md5)
                {
                    load_Asset_Queue.Enqueue(s_Asset.Value);
                }
            }
            //本地没有服务器上的AB资源，代表是新资源（必下资源）
            else
            {
                load_Asset_Queue.Enqueue(s_Asset.Value);
            }

            // foreach (var l_asset in local_Assets_dict)
            // {
            //     //判断服务器是否有删除AB资源
            //     if (server_Assets_Dict.ContainsKey(l_asset.Key))
            //     {
            //         delete_Asset_Queue.Enqueue(l_asset.Value);
            //     }
            // }
        }
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
        string path = $"{http_Asset_Server_IP}/{server_Version.ToString()}/{ast.abName}";

        Load_Asset_Server_Asset(path, (data) =>
        {
            //本地P目录下的资源路径
            string local_Asset_Path = $"{Application.persistentDataPath}/{ast.abName}";
            string local_Asset_dire_Path =  $"{Application.persistentDataPath}/{ast.abDirectory}";
            if (File.Exists(local_Asset_Path))
            {
                File.Delete(local_Asset_Path);
            }
            
            //判断对应路径文件夹存不存在，不存在就创建
            if (!Directory.Exists(local_Asset_dire_Path))
            {
                Directory.CreateDirectory(local_Asset_dire_Path);
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

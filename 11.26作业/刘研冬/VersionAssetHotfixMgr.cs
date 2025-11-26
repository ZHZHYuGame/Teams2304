using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class VersionAssetHotfixMgr : MonoBehaviour
{
    string http_Asset_Server_IP = "127.0.0.1/Client_1121";

    VersionData server_Version;
    // Start is called before the first frame update
    void Start()
    {
        //版本下载，并于本地P目录下的版本号文件对比
        string server_Verion_txt = $"{http_Asset_Server_IP}/Version.txt";
        Load_Asset_Server_Version(http_Asset_Server_IP, (data) =>
        {
            string s_Version_Str=BitConverter.ToString(data);
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

            }
            //存在，走相关的资源更新逻辑
            else
            {
                //大版本更新
                if (server_Version.big > local_Version.big)
                {

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

    private void Load_Asset_Server_Version(string path, Action<byte[]>complete)
    {
        StartCoroutine(LoadGameAsset(path, complete));
    }
    IEnumerator LoadGameAsset(string path, Action<byte[]>complete)
    {
        UnityWebRequest unityWeb = UnityWebRequest.Get(path);

        UnityWebRequestAsyncOperation op = unityWeb.SendWebRequest();

        if(op.isDone)
        {
            complete(unityWeb.downloadHandler.data);
        }
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

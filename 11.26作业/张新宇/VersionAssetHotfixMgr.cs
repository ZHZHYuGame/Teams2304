using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
/// <summary>
/// 资源更新框架
/// 1.版本下载， 并与本地P目录下的版本号文件对比
/// </summary>
public class VersionAssetHotfixMgr : MonoBehaviour
{
    string http_Server_IP = "127.0.0.1/Game";

    VersionData server_Version;
    
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
            if(Directory.Exists(l_Version_Path))
            {
                local_Version=new VersionData(File.ReadAllText(l_Version_Path));
                Debug.Log(local_Version);
            }
            if(local_Version==null)
            {

            }
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

    void Load_Asset_Server_Version(string path, Action<byte[]> action)
    {
        StartCoroutine(LoadGameAsset(path,action));
    }
    IEnumerator LoadGameAsset(string path, Action<byte[]> action)
    {
        UnityWebRequest unityWeb=UnityWebRequest.Get(path);

        UnityWebRequestAsyncOperation op = unityWeb.SendWebRequest();

        if(op.isDone)
        {
            action(unityWeb.downloadHandler.data);
        }
        yield return null;
    }
}

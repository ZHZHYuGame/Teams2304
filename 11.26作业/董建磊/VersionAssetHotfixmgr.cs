using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class VersionAssetHotfixmgr :MonoBehaviour
{
    string http_Asset_Server_IP = "127.0.0.1/GameAsset";
    VersionData server_Version;
    // Start is called before the first frame update
    public void Start()
    {
        string server_Verion_txt = $"{http_Asset_Server_IP}/Version.txt";
        Load_Server_Version(server_Verion_txt, (data) =>
        {
            string s_Version_Str = Encoding.UTF8.GetString(data);
            server_Version=new VersionData(s_Version_Str);
            string l_Version_Path = $"{Application.persistentDataPath}/Version.txt";
            VersionData local_Version = null;
            if(Directory.Exists(l_Version_Path))
            {
                local_Version=new VersionData(File.ReadAllText(l_Version_Path));
            }
            if(local_Version==null)
            {

            }
            else
            {
                if(server_Version.big>local_Version.big)
                {

                }
                else if(server_Version.middle>local_Version.middle)
                {

                }
                else if(server_Version.small>local_Version.small)
                {

                }
                else
                {

                }
            }
        });
    }
    void Load_Server_Version(string path, Action<byte[]> action)
    {
        StartCoroutine(LoadGameAsset(path, action));
    }
    IEnumerator LoadGameAsset(string path, Action<byte[]> complete)
    {
        UnityWebRequest unityWeb = UnityWebRequest.Get(path);
        UnityWebRequestAsyncOperation op = unityWeb.SendWebRequest();
        if(op.isDone)
        {
            complete(unityWeb.downloadHandler.data);
        }
        yield return null;
    }
}

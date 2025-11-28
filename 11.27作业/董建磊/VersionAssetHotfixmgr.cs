using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

public class VersionAssetHotfixmgr :MonoBehaviour
{
    string http_Asset_Server_IP = "http://127.0.0.1/GameAsset";
    VersionData server_Version;
    // Start is called before the first frame update
    string server_AssetMainFast_Str;
    Dictionary<string, ABAsset> server_Assets_Dict = new Dictionary<string, ABAsset>();
    Queue<ABAsset>load_Asset_Queue= new Queue<ABAsset>();
    Queue<ABAsset>delete_Asset_Queue=new Queue<ABAsset>();
    public void Start()
    {
        MessageControll.Instance.AddListener(MesID.Hotfix_Configm_Event, Hotfix_Event_Assets);
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
                DownLoad_All_Assets();
            }
            else
            {
                if(server_Version.big>local_Version.big)
                {
                    DownLoad_All_Assets();
                }
                else if(server_Version.middle>local_Version.middle)
                {

                }
                else if(server_Version.small>local_Version.small)
                {
                    Hotfix_Version_Assets_AllComparison();
                }
                else
                {

                }
            }
        });
    }

    private void Hotfix_Event_Assets(object obj)
    {
        Hotfix_Version_Assets_AllComparison();
    }

    private void Hotfix_Version_Assets_AllComparison()
    {
        string path = $"{http_Asset_Server_IP}/AssetMainfast.txt";
        Load_Asset_Server_AssetMainfast(path, (data) =>
        {
            string assetMainfast_Str=Encoding.UTF8.GetString(data);
            Asset_ABAsset_Add_To_Dict(server_Assets_Dict, assetMainfast_Str);
            string local_AssetMainfast_Path = $"{Application.persistentDataPath}/AssetMainfast.txt";
            string loc_Asset_Str=File.ReadAllText(local_AssetMainfast_Path);
            Dictionary<string,ABAsset>local_Assets_Dict=new Dictionary<string,ABAsset>();
            Asset_ABAsset_Add_To_Dict(local_Assets_Dict, loc_Asset_Str);
            Version_Asset_Compare_S_And_L(local_Assets_Dict);
            if (load_Asset_Queue.Count > 0)
            {
                DownLoad_Asset(load_Asset_Queue.Dequeue());
            }
        });
    }

    private void Asset_ABAsset_Add_To_Dict(Dictionary<string, ABAsset> server_Assets_Dict, string assetMainfast_Str)
    {
        string[] abStrArr = assetMainfast_Str.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
        foreach(var item in abStrArr)
        {
            ABAsset ab = new ABAsset(item);
            server_Assets_Dict.Add(ab.abName, ab);
        }
    }

    void Version_Asset_Compare_S_And_L(Dictionary<string,ABAsset>local_Assets_Dict)
    {
        foreach(var s_Asset in server_Assets_Dict)
        {
            if(local_Assets_Dict.ContainsKey(s_Asset.Key))
            {
                if(s_Asset.Value.md5!=local_Assets_Dict[s_Asset.Key].md5)
                {
                    load_Asset_Queue.Enqueue(s_Asset.Value);
                }
            }
            else
            {
                load_Asset_Queue.Enqueue(s_Asset.Value);
            }
        }
        foreach (var l_Asset in local_Assets_Dict)
        {
            if(server_Assets_Dict.ContainsKey(l_Asset.Key))
            {
                delete_Asset_Queue.Enqueue(l_Asset.Value);
            }
        }
    }
    private void DownLoad_All_Assets()
    {
        string path = $"{http_Asset_Server_IP}/AssetMainfast.txt";
        Load_Asset_Server_AssetMainfast(path, (data) =>
        {
            server_AssetMainFast_Str=Encoding.UTF8.GetString(data);
            string[] abSetArr = server_AssetMainFast_Str.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
            foreach(var aStr in abSetArr)
            {
                ABAsset ab=new ABAsset(aStr);
                load_Asset_Queue.Enqueue(ab);
            }
            
            DownLoad_Asset(load_Asset_Queue.Dequeue());
        });
    }

    private void DownLoad_Asset(ABAsset aBAsset)
    {
        string path = $"{http_Asset_Server_IP}/{aBAsset.abName}";
        Load_Asset_Server_Asset(path, (data) =>
        {
            string local_Asset_Path = $"{Application.persistentDataPath}/{aBAsset.abName}";
            if(Directory.Exists(local_Asset_Path))
            {
                File.Delete(local_Asset_Path);
            }
            if(Directory.Exists(local_Asset_Path))
            {
                Directory.CreateDirectory(local_Asset_Path);
            }
            File.WriteAllBytes(local_Asset_Path, data);
            if(load_Asset_Queue.Count>0)
            {
                DownLoad_Asset(load_Asset_Queue.Dequeue());
            }
            else
            {
                Save_Server_Version();
                Save_Server_AssetsMainfast();
            }
        });
    }

    private void Save_Server_AssetsMainfast()
    {
        File.WriteAllText($"{Application.persistentDataPath}/AssetMainfast.txt", server_AssetMainFast_Str);
    }

    private void Save_Server_Version()
    {
        File.WriteAllText($"{Application.persistentDataPath}/Version.txt", server_Version.ToString());
    }
    void Load_Asset_Server_Asset(string path, Action<byte[]>complete)
    {
        StartCoroutine(LoadGameAsset(path, complete));
    }
    void Load_Asset_Server_AssetMainfast(string path, Action<byte[]>complete)
    {
        StartCoroutine(LoadGameAsset(path, complete));
    }
    void Load_Server_Version(string path, Action<byte[]> action)
    {
        StartCoroutine(LoadGameAsset(path, action));
    }
    IEnumerator LoadGameAsset(string path, Action<byte[]> complete)
    {
        UnityWebRequest unityWeb = UnityWebRequest.Get(path);
        UnityWebRequestAsyncOperation op = unityWeb.SendWebRequest();
        Thread.Sleep(100);
        if (op.isDone)
        {
            complete(unityWeb.downloadHandler.data);
        }
        yield return null;
    }
}

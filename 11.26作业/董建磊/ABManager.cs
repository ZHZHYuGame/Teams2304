using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ABManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        abPath = Path.Combine(Application.streamingAssetsPath + "/ABs");
        InitDependence();
    }

    private void InitDependence()
    {
        if(allDependDict==null)
        {
            allDependDict = new Dictionary<string, string[]>();
            string path = Path.Combine(abPath, "ABs");
            AssetBundle assetBundle = AssetBundle.LoadFromFile(path);
            var manifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            string[] allAssetBundle = manifest.GetAllAssetBundles();
            foreach(var item in allAssetBundle)
            {
                string[] dlist = manifest.GetAllDependencies(item);
                allDependDict.Add(item, dlist);
                allDependDict[item] = dlist;
            }
        }
    }
    public T LoadAsset<T>(string name)where T:UnityEngine.Object
    {
        string assetBundleName=name.ToLower()+".u3d";
        if(allDependDict.ContainsKey(assetBundleName))
        {
            string[] dependencelist = allDependDict[assetBundleName];
            foreach(var item in dependencelist)
            {
                LoadAssetBundle(item);
            }
        }
        MyAssetBundle my = LoadAssetBundle(assetBundleName);
        return my.ab.LoadAllAssets<T>()[0];
    }
    private MyAssetBundle LoadAssetBundle(string assetbundlename)
    {
        string path = Path.Combine(abPath, assetbundlename);
        if(abCache.ContainsKey(assetbundlename))
        {
            abCache[assetbundlename].count++;
            return abCache[assetbundlename];
        }
        else
        {
            try
            {
                AssetBundle ab = AssetBundle.LoadFromFile(path);
                MyAssetBundle my = new MyAssetBundle(ab);
                abCache.Add(assetbundlename, my);
                return my;
            }
            catch(System.Exception ex)
            {
                Debug.LogError($"加载资源包={path},发生错误 msg={ex.Message}");
            }
        }
        return null;
    }
    public void UnLoad(string name)
    {
        string assetbundlename = name.ToLower() + ".u3d";
        if(allDependDict.ContainsKey(assetbundlename))
        {
            string[]dependenceArr=allDependDict[assetbundlename];
            foreach(var item in dependenceArr)
            {
                UnLoadAssetBundle(item);
            }
        }
        UnLoadAssetBundle(assetbundlename);
    }

    private void UnLoadAssetBundle(string item)
    {
        if(abCache.ContainsKey(item))
        {
            abCache[item].count--;
            if (abCache[item].count <= 0)
            {
                abCache[item].ab.Unload(false);
                abCache.Remove(item);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            string mPath = Application.streamingAssetsPath + "/AssetBundle/new material.u3d";
            AssetBundle.LoadFromFile(mPath);
        }
    }
    private Dictionary<string, MyAssetBundle>abCache=new Dictionary<string, MyAssetBundle>();
    private Dictionary<string, string[]> allDependDict;
    private string abPath;
}
public class MyAssetBundle
{
    public int count;
    public AssetBundle ab;
    public MyAssetBundle(AssetBundle ab)
    {
        count = 1;
        this.ab = ab;
    }
}
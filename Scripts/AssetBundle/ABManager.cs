using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ABManager : Singleton<ABManager>
{
    // Start is called before the first frame update
    public void Start()
    {
        //string colorpath = Application.streamingAssetsPath + "/AssetBundle/color.u3d";
        //AssetBundle cab = AssetBundle.LoadFromFile(colorpath);

        ////资源包的路径
        //string path = Application.streamingAssetsPath + "/AssetBundle/cube.u3d";
        ////通过资源包的路径找到这一个资源包
        //AssetBundle aBundle = AssetBundle.LoadFromFile(path);
        ////通过资源包找到里面存在的物体
        //GameObject obj = aBundle.LoadAsset<GameObject>("cube");
        //Instantiate(obj);

        Init();
        //var obj= LoadAsset<GameObject>("cube");
        //GameObject.Instantiate(obj);
        //List<ABAsset> paths = GetAllPath($"{Application.persistentDataPath}/AssetMainfast.txt");
        //foreach (ABAsset asset in paths)
        //{
        //    string path = $"{Application.persistentDataPath}/{asset.abName}";
        //    AssetBundle assetBundle = AssetBundle.LoadFromFile(path);
        //    string name = asset.abName.Split('.')[0];
        //    GameObject obj = assetBundle.LoadAsset<GameObject>(name);
        //    if(obj!=null)
        //    {
        //        Instantiate(obj);
        //    }
            
        //}
    }

    private List<ABAsset> GetAllPath(string v)
    {
        string str= File.ReadAllText(v);
        string[] strs = str.Trim().Split(new string[]{ "\r\n"},StringSplitOptions.None);
        List<ABAsset> aBAssets = new List<ABAsset>();
        foreach(var s in strs)
        {
            ABAsset aB = new ABAsset(s);
            aBAssets.Add(aB);
        }
        return aBAssets;
    }

    GameObject m_obj;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (m_obj==null)
            {
                string mPath = Application.streamingAssetsPath + "/AssetBundle/materi.u3d";
                AssetBundle aBundle=  AssetBundle.LoadFromFile(mPath);
                m_obj = aBundle.LoadAsset<GameObject>("materi");
            }
            GameObject.Instantiate(m_obj);
        }
    }

    /// <summary>
    /// ab包的缓存
    /// </summary>
    private Dictionary<string, MyAssetBundle> abCache = new Dictionary<string, MyAssetBundle>();
    /// <summary>
    /// 用来存储所有的资源依赖关系的数据
    /// </summary>
    private Dictionary<string, string[]> allDependDict;
    /// <summary>
    /// AB资源路径
    /// </summary>
    private string abPath;
    public void Init()
    {
        abPath = Path.Combine(Application.streamingAssetsPath + "/ABs");
        InitDependence();
    }
    /// <summary>
    /// 初始化资源包的依赖关系
    /// </summary>
    public void InitDependence()
    {
        if (allDependDict == null)
        {
            allDependDict = new Dictionary<string, string[]>();
            //拼接的是路径ABs文件夹下面的ABs这个AB包
            string path = Path.Combine(abPath, "ABOutFiles");
            //加载资源包
            AssetBundle assetBundle = AssetBundle.LoadFromFile(path);
            //加载资源
            var manifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            //获取所有资源包的名字
            string[] allAssetBundle = manifest.GetAllAssetBundles();

            foreach (var item in allAssetBundle)
            {
                //获取ab包的依赖的资源包
                string[] dList = manifest.GetAllDependencies(item);
                allDependDict.Add(item, dList);
                allDependDict[item] = dList;
            }
        }
    }

    /// <summary>
    /// 加载指定的资源(具体某个资源)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public T LoadAsset<T>(string name) where T : UnityEngine.Object
    {
        string assetBundleName = name.ToLower() + ".u3d";

        //加载依赖的资源包
        if (allDependDict.ContainsKey(assetBundleName))
        {
            string[] dependenceList = allDependDict[assetBundleName];
            foreach (var item in dependenceList)
            {
                //被依赖的资源只要加载到内存中就可以了
                LoadAssetBundle(item);
            }
        }
        //加载真正需要的资源自己
        MyAssetBundle my = LoadAssetBundle(assetBundleName);
        Debug.Log(my);
        return my.ab.LoadAllAssets<T>()[0];///因为打包工具中，一个资源包里就只有一个资源。所以是[0]
    }

    /// <summary>
    /// 加载单个资源包的方法 
    /// </summary>
    /// <param name="assetbundlename"></param>
    private MyAssetBundle LoadAssetBundle(string assetbundlename)
    {
        string path = Path.Combine(abPath, assetbundlename).Replace(@"\", "/");
        if (abCache.ContainsKey(assetbundlename))
        {
            abCache[assetbundlename].count++;///之前加载过这个AB包，计数增加就可以了。
            return abCache[assetbundlename];
        }
        else
        {
            try
            {
                ///没加载过，加载一波，放入缓存。
                AssetBundle ab = AssetBundle.LoadFromFile(path);
                MyAssetBundle my = new MyAssetBundle(ab);
                abCache.Add(assetbundlename, my);
                return my;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"加载资源包={path},发生错误  msg= {ex.Message}");
            }
        }
        return null;

    }
    /// <summary>
    /// 删除一个资源
    /// </summary>
    /// <param name="name"></param>
    public void UnLoad(string name)
    {
        string assetbundlename = name.ToLower() + ".u3d";
        //
        if (allDependDict.ContainsKey(assetbundlename))
        {
            string[] dependenceArr = allDependDict[assetbundlename];
            foreach (var item in dependenceArr)
            {
                UnLoadAssetBundle(item);
            }
        }

        UnLoadAssetBundle(assetbundlename);
    }
    /// <summary>
    /// 卸载一个资源
    /// </summary>
    /// <param name="abName"></param>
    private void UnLoadAssetBundle(string abName)
    {
        if (abCache.ContainsKey(abName))
        {
            abCache[abName].count--;///之前加载过这个AB包，计数增加就可以了。
            if (abCache[abName].count <= 0)
            {
                abCache[abName].ab.Unload(false);
                abCache.Remove(abName);
            }
        }
    }
}

public class MyAssetBundle
{
    /// <summary>
    /// 代表一个资源包被加载的次数。
    /// </summary>
    public int count;

    public AssetBundle ab;
    public MyAssetBundle(AssetBundle ab)
    {
        count = 1;
        this.ab = ab;
    }

}

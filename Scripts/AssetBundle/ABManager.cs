using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using XLua;
using static UnityEditor.Progress;
[LuaCallCSharp]
public class ABManager : Singleton<ABManager>
{
    
    // Start is called before the first frame update
    public void Start()
    {
        Init();
    }

    GameObject m_obj;

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
    private Dictionary<string, Sprite[]> allSprites= new Dictionary<string, Sprite[]>();
    public void Init()
    {
        InitDependence();
        Asstes_ABRule();
    }

    void Asstes_ABRule()
    {
        Tool_Time_Manager.GetInstance().Delay_Handle_Most(600,ABDestoryRule);
        Tool_Time_Manager.GetInstance().Delay_Handle_Most(300, TwoCache_ABRule);
    }
    /// <summary>
    /// 資源釋放規則
    /// 在什麽時候釋放？（數量與時間）
    /// 如果在釋放后馬上又要使用（釋放隊列）
    /// 如果使用的頻率不大（時間）
    /// 如果緩存太多（數量）
    /// 如果存在的時間太長（不用的情況下）（時間）
    /// </summary>
    void ABDestoryRule()
    {
        Assets_ABCountRule();
        Assets_ABLifeTimeRule();
    }
    /// <summary>
    /// 資源的數量判斷規則
    /// </summary>
    void Assets_ABCountRule()
    {
        if(abCache.Count>200)
        {
            Debug.Log("AB包资源数异常，走出规定最大上限值，请检查相关泄漏资源情况");
            foreach (var item in abCache.Values)
            {
                if (item.count <= 0 && item.isActive)
                {
                    item.isActive = false;
                    DestoryAssetBundle_TwoCache(item.name);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// AB包资源按时检测的时间
    /// </summary>
    int abCheckTimes = 10;
    /// <summary>
    /// 2级缓存的检测时间
    /// </summary>
    int twoCacheTimes = 5;
    /// <summary>
    /// 二级缓存（进行最终的释放与复用）
    /// </summary>
    Dictionary<string, MyAssetBundle> TwoCache_AB = new Dictionary<string, MyAssetBundle>();
    void Assets_ABLifeTimeRule()
    {
        List<string> keys = abCache.Keys.ToList();
        for (int i = 0; i < keys.Count ; i++)
        {
            var ab = abCache[keys[i]];
            var abLifeTimes = (DateTime.Now.Ticks - ab.lifeCreateTimes) / 10000000;
            if (abLifeTimes >= abCheckTimes && ab.count <= 0 && ab.isActive)
            {
                ab.isActive = false;
                DestoryAssetBundle_TwoCache(ab.name);
            }
        }
        //foreach(var ab in abCache.Values)
        //{
        //    var abLifeTimes = (DateTime.Now.Ticks - ab.lifeCreateTimes) / 10000000;
        //    if(abLifeTimes>=abCheckTimes&&ab.count<=0)
        //    {
        //        DestoryAssetBundle_TwoCache(ab.name);

        //        //TwoCache_AB.Remove(ab.name);
        //    }
        //}
    }

    void DestoryAssetBundle_TwoCache(string abName)
    {
        Debug.Log("进入二级缓存" +abName);
        TwoCache_AB.Add(abName, abCache[abName]);
    }
    private void TwoCache_ABRule()
    {
        List<string> keys = TwoCache_AB.Keys.ToList();
        for (int i=0;i<keys.Count;i++)
        {
            var ab = TwoCache_AB[keys[i]];
            var abLifeTimes = (DateTime.Now.Ticks - ab.lifeCreateTimes) / 10000000;
            if (abLifeTimes >= twoCacheTimes)
            {
                Debug.Log("释放" + ab.name);
                TwoCache_AB[ab.name].ab.Unload(false);
                TwoCache_AB.Remove(ab.name);
                abCache.Remove(ab.name);
            }
        }
        //foreach (var ab in TwoCache_AB.Values)
        //{
        //    var abLifeTimes = (DateTime.Now.Ticks - ab.lifeCreateTimes) / 10000000;
        //    if (abLifeTimes >= twoCacheTimes)
        //    {
        //        Debug.Log("释放"+ab.name);
        //        TwoCache_AB[ab.name].ab.Unload(false);
        //        abCache.Remove(ab.name);
        //    }
        //}
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
            //string path = Path.Combine(abPath, "ABOutFiles");
            string path = Application.persistentDataPath + "/ABOutFiles";
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
    /// Lua使用AB包加载的中转战
    /// </summary>
    /// <param name="name">资源名称</param>
    /// <param name="Load_Enum">资源类型</param>
    public void Load_Transfer(string name,string Load_Enum)
    {
        switch (Load_Enum)
        {
            case "GameObject":

                break;
            case "Sprite":

                break;
        }
    }
    /// <summary>
    /// 加载指定的资源(具体某个资源)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    ///   T           <T>              where T : UnityEngine.Object
    public GameObject LoadAsset_GameObject(string name) 
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
        var obj = my.ab.LoadAllAssets<GameObject>()[0];
        return obj;///因为打包工具中，一个资源包里就只有一个资源。所以是[0]
    }
    /// <summary>
    /// 加载不在图集中Sprite
    /// </summary>
    /// <param name="name">AB包名</param>
    /// <returns></returns>
    public Sprite LoadAsset_Sprite(string name)
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
        return my.ab.LoadAllAssets<Sprite>()[0];///因为打包工具中，一个资源包里就只有一个资源。所以是[0]
    }
    /// <summary>
    /// 加载图集中Sprite
    /// </summary>
    /// <param name="Atlas_Name">AB包名</param>
    /// <param name="Sprite_Name">Sprit名</param>
    /// <returns></returns>
    public Sprite LoadAsset_Atlas_Sprite(string Atlas_Name,string Sprite_Name)
    {
        string assetBundleName = Atlas_Name.ToLower() + ".u3d";
        if (allDependDict.ContainsKey(assetBundleName))
        {
            string[] dependenceList = allDependDict[assetBundleName];
            foreach (var item in dependenceList)
            {
                //被依赖的资源只要加载到内存中就可以了
                LoadAssetBundle(item);
            }
        }
        MyAssetBundle my = LoadAssetBundle(assetBundleName);
        Debug.Log(my);
        Sprite sprite;
        if (!allSprites.ContainsKey(Atlas_Name))
        {
            Sprite[] sprites = my.ab.LoadAssetWithSubAssets<Sprite>(assetBundleName);
            allSprites.Add(Atlas_Name, sprites);
            sprite= System.Array.Find(sprites, item => item.name == Sprite_Name);
        }
        else
        {
            sprite = System.Array.Find(allSprites[Atlas_Name], item => item.name == Sprite_Name);
        }
        return sprite;
    }
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
        //string path = Path.Combine(abPath, assetbundlename);
        string path = Application.persistentDataPath + "/" + assetbundlename;
        if (abCache.ContainsKey(assetbundlename))
        {
            abCache[assetbundlename].isActive = true;
            abCache[assetbundlename].count++;///之前加载过这个AB包，计数增加就可以了。
            abCache[assetbundlename].lifeCreateTimes = DateTime.Now.Ticks;
            return abCache[assetbundlename];
        }
        else
        {
            try
            {
                ///没加载过，加载一波，放入缓存。
                AssetBundle ab = AssetBundle.LoadFromFile(path);
                MyAssetBundle my = new MyAssetBundle(ab,assetbundlename);
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

    public long lifeCreateTimes;

    public string name;

    public bool isActive=true;
    public MyAssetBundle(AssetBundle ab, string name)
    {
        count = 1;
        this.ab = ab;
        lifeCreateTimes = DateTime.Now.Ticks;
        this.name = name;
    }

}

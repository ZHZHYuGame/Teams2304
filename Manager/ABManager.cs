using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

public class ABManager : Singleton<ABManager>
{
    
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
    public void OnInit()
    {
        abPath = $"{Application.streamingAssetsPath}/{Application.version}";
        InitDependence();
    }
    /// <summary>
    /// 初始化资源包的依赖关系路径（方便以后加载一个具体资源能拿到它的依赖项进行加载）
    /// </summary>
    public void InitDependence()
    {
        if (allDependDict == null)
        {
            allDependDict = new Dictionary<string, string[]>();
            //拼接的是p目录路径下面的1.0.3(版本号资源清单,根据实际版本读取)这个mainfest类型文件
            string version = File.ReadAllText($"{Application.streamingAssetsPath}/Version.txt");
            string path = $"{abPath}/{Application.version}";
            //加载整体的资源包
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
        ///因为打包工具中，一个资源包里就只有一个资源。所以是[0]
        T[] t = my.ab.LoadAllAssets<T>();
        return t[0];
    }
    /// <summary>
    /// lua调用加载指定的资源(具体某个资源)
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public Object LoadAssetLua(string name , System.Type type)
    {
        MethodInfo loadMethod = GetInstance().GetType().GetMethod(
            "LoadAsset", 
            BindingFlags.Public | BindingFlags.Instance, // 注意：原代码用 NonPublic，需匹配方法实际访问修饰符！
            null,
            new[] { typeof(string) }, // LoadAsset<T> 的参数类型是 string
            null
        );
        
        MethodInfo spMethodInfo = loadMethod.MakeGenericMethod(type);

        object rawResult = spMethodInfo.Invoke(this, new object[] { name });

        //object obj = ConvertReturnValue(rawResult, type);

        return rawResult as Object;
    }
    
    /// <summary>
    /// lua加载指定的资源(资源数组)
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public Object[] LoadAssetArrayLua(string name , System.Type type)
    {
        MethodInfo loadMethod = GetInstance().GetType().GetMethod(
            "LoadAssetArray", 
            BindingFlags.Public | BindingFlags.Instance, // 注意：原代码用 NonPublic，需匹配方法实际访问修饰符！
            null,
            new[] { typeof(string) }, // LoadAsset<T> 的参数类型是 string
            null
        );
        
        MethodInfo spMethodInfo = loadMethod.MakeGenericMethod(type);

        object rawResult = spMethodInfo.Invoke(this, new object[] { name });
        
        object[] objArray = rawResult as object[];

        for (int i = 0; i < objArray.Length; i++)
        {
            objArray[i] = ConvertReturnValue(objArray[i], type);
        }

        return objArray as Object[];
    }
/// <summary>
/// 类型转换
/// </summary>
/// <param name="rawResult"></param>
/// <param name="type"></param>
/// <returns></returns>
/// <exception cref="InvalidCastException"></exception>
    private object ConvertReturnValue(object rawResult, Type type)
    {
        //处理 null：引用类型返回 null，值类型返回默认值
        if (rawResult == null)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
        //类型已匹配
        if (rawResult.GetType() == type)
        {
            return rawResult;
        }
        //可空类型
        if (Nullable.GetUnderlyingType(type) is Type nullableUnderType)
        {
            return Convert.ChangeType(rawResult, nullableUnderType);
        }
        //值类型/基础类型转换
        if (type.IsValueType || type == typeof(string))
        {
            return Convert.ChangeType(rawResult, type);
        }
        //引用类型转换
        if (type.IsAssignableFrom(rawResult.GetType()))
        {
            return Convert.ChangeType(rawResult, type);
        }
        
        throw new InvalidCastException($"无法将 {rawResult.GetType().Name} 转换为 {type.Name}");
    }
    
    /// <summary>
    /// 加载指定的资源(资源数组)
    /// </summary>
    /// <param name="name"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T[] LoadAssetArray<T>(string name) where T : UnityEngine.Object
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
        ///因为打包工具中，一个资源包里就只有一个资源。所以是[0]
        T[] t = my.ab.LoadAllAssets<T>();
        return t;
    }

    /// <summary>
    /// 加载单个资源包的方法 
    /// </summary>
    /// <param name="assetbundlename"></param>
    private MyAssetBundle LoadAssetBundle(string assetbundlename)
    {
        string path = Path.Combine(abPath, assetbundlename);
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

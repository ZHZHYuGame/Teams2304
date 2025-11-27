using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;

//AB 包（AssetBundle） 是 Unity 官方提供的资源打包与动态加载解决方案实现减小安装包体积、资源热更新、按需加载等核心需求。
public class AssetBundlePackageMgr : Editor
{
    [MenuItem("Tool/AssetBundle/AB打包（正常）")]
    public static void CreateAssetBundle()
    {
        AssetBundleFiltrate();
        string outPath = $"{Application.dataPath}/Resources/ABFiles";
        if(!Directory.Exists(outPath))
        {
            Directory.CreateDirectory(outPath);
        }
        BuildPipeline.BuildAssetBundles(outPath, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows);

        Progress.Start(outPath);
    }

    private static void AssetBundleFiltrate()
    {
        //设置需要筛选的文件格式类型
        string[] filtrateArr = new string[] { ".meta", ".pdf" };
        //获取到所有需要打包的资源文件
        string[] allFiles = Directory.GetFiles($"{Application.dataPath}/Resources/ABPackage", "*.*", SearchOption.AllDirectories);
        //判断筛除掉的文件格式类型资源文件，并得到筛除后的资源文件数组
        allFiles = allFiles.Where((x) => !filtrateArr.Contains(Path.GetExtension(x))).ToArray();
        
        StringBuilder sb= new StringBuilder();
        //最后得到资源打包
        foreach (var fp in allFiles)
        {
            //将文件的\都装换为/，并且截取Assets后的相关路径（下面AssetImporter打包所需格式路径）
            string packPath = fp.Replace(@"\", "/").Replace(Application.dataPath, "Assets");
            //得到对应的文件路径的资源名称
            string abName = Path.GetFileNameWithoutExtension(packPath);
            //AB打包器
            AssetImporter importer=AssetImporter.GetAtPath(packPath);
            //AB打包名称设置
            importer.assetBundleName=abName;
            //AB打包扩展名设置
            importer.assetBundleVariant = "u3d";
            //资源的MD5码（做为资源的改变判定）
            string md5 = GetMD5(fp);
            //拼成设计的资源格式（AB包资源名+资源的MD5码）
            string abStr = $"{importer.assetBundleName}.{importer.assetBundleVariant}|{md5}";
            sb.AppendLine(abStr);
        }

        SaveAssetsMainfast(sb.ToString());
        SaveAssetsVersion();
    }
    [MenuItem("Tool/AssetBundle/创建StreamingAssetsPath目录")]
    public static void CreateStreamingAssetsPath()
    {
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
        }
        //做为打包时的一些进入游戏之前的展示资源（模型、图片、动画、特效、剧情过场动画）的携带，跟着打包走
    }
    [MenuItem("Tool/AssetBundle/创建PersistentDataPath目录")]
    public static void ShowPersistentDataPath()
    {
        Process.Start(Application.persistentDataPath);
    }
    /// <summary>
    /// 保存版本号文件
    /// </summary>
    private static void SaveAssetsVersion()
    {
        File.WriteAllText($"{Application.streamingAssetsPath}/Version.txt", Application.version);
    }
    /// <summary>
    /// 保存资源清单文件
    /// </summary>
    private static void SaveAssetsMainfast(string v)
    {
        File.WriteAllText($"{Application.streamingAssetsPath}/AssetMainfast.txt", v);
    }
    /// <summary>
    /// 转化一个资源的MD5
    /// </summary>
    /// <param name="fp"></param>
    /// <returns></returns>
    private static string GetMD5(string fp)
    {
        byte[] data = File.ReadAllBytes(fp);
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] mData = md5.ComputeHash(data);
        return BitConverter.ToString(mData).Replace("-", "");
    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;

public class AssetBundlePackaggeMer : Editor
{
    [MenuItem("Tool/AssetBundle/打包(正常)")]
    public static void CreateAssetBundle()
    {
        AssetBundleFiltrate();
        //输出AB资源路径
        string outPath = Application.dataPath + "/Resources/ABOutFiles";

        if(!Directory.Exists(outPath))
        {
            Directory.CreateDirectory(outPath);
        }
        BuildPipeline.BuildAssetBundles(outPath, BuildAssetBundleOptions.ChunkBasedCompression,BuildTarget.StandaloneWindows);
        Progress.Start(outPath);
    }
    public static void AssetBundleFiltrate()
    {
        string[] filtrateArr =new string[] { ".meta",".pdf"};
        //获取文件夹中所有资源
        string[] allFiles = Directory.GetFiles($"{Application.dataPath}/Resources/ABPackage", "*.*",SearchOption.AllDirectories);
        allFiles=allFiles.Where((x) =>!filtrateArr.Contains(Path.GetExtension(x))).ToArray();

        StringBuilder sb = new StringBuilder();

        foreach (string fp in allFiles)
        {
            string packPath = fp.Replace(@"\", "/").Replace(Application.dataPath, "Assets");
            //"D:/Unity项目文件/专高6/练习/Script/Assets/Resources/ABPackage/Btn.prefab"
            //string b = str.Substring(str.IndexOf("Assets"));
            string abName=Path.GetFileNameWithoutExtension(packPath);
            AssetImporter importer = AssetImporter.GetAtPath(packPath);
            importer.assetBundleName = abName;
            importer.assetBundleVariant = "u3d";
            string md5 = GetMD5(fp);
            string abStr = $"{importer.assetBundleName}.{importer.assetBundleVariant}|{md5}";
            sb.AppendLine(abStr);
        }
        SaveAssetsMainfast(sb.ToString());

        SaveAssetsVersion();
    }
    /// <summary>
    /// 保存资源清单文件
    /// </summary>
    /// <param name="text"></param>
    private static void SaveAssetsMainfast(string text)
    {
        File.WriteAllText($"{Application.streamingAssetsPath}/AssetMainfast.txt",text);
    }
    /// <summary>
    /// 转化一个资源的MD5码
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private static string GetMD5(string fp)
    {
        byte[] data =File.ReadAllBytes(fp);
        MD5CryptoServiceProvider md5=new MD5CryptoServiceProvider();
        byte[] mData= md5.ComputeHash(data);
        return BitConverter.ToString(mData).Replace("-","");
    }

    [MenuItem("Tool/AssetBundle/创建StreamingPath目录")]
    public static void CreateStreamingPath()
    {
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory (Application.streamingAssetsPath);
        }
        //作为打包时的一些进入游戏之前的展示资源（模型，图标，动画，特效，剧情过场动画）的携带，跟打包走
    }
    [MenuItem("Tool/AssetBundle/创建PersistentDataPath目录")]
    public static void ShowPersistentDataPath()
    {
        Process.Start(Application.persistentDataPath);
    }
    //private static string GetMD5(string path)
    //{
    //    byte[] data= File.ReadAllBytes(path);
    //    MD5CryptoServiceProvider
    //}
    /// <summary>
    /// 保存版本号文件
    /// </summary>
    static void SaveAssetsVersion()
    {
        File.WriteAllText($"{Application.streamingAssetsPath}/Version.txt",Application.version);
    }
}

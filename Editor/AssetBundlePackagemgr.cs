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
using Debug = System.Diagnostics.Debug;

public class AssetBundlePackagemgr : Editor
{
    [MenuItem("Tool/AssetBundle/打包（正常）")]
    public static void CreateAssetBundle()
    {
        AssetBundleFilter();
        
        var outPath = $"{Application.streamingAssetsPath}/{Application.version}";

        if (!Directory.Exists(outPath))
        {
            Directory.CreateDirectory(outPath);
        }

        BuildPipeline.BuildAssetBundles(outPath, BuildAssetBundleOptions.ChunkBasedCompression,
            BuildTarget.StandaloneWindows);
        Process.Start(outPath);
    }

    private static void AssetBundleFilter()
    {
        //设置资源文件筛选格式
        string[] filtrateArr = new string[] { ".meta", ".pdf" };
        //获取所有需要打包的资源路径
        string[] allFiles = Directory.GetFiles($"{Application.dataPath}/Resources/ABPackage", "*.*",
            SearchOption.AllDirectories);
        allFiles = allFiles.Where((x) => !filtrateArr.Contains(Path.GetExtension(x))).ToArray();
        
        StringBuilder sb = new StringBuilder();

        foreach (var path in allFiles)
        {
            string packPath = path.Replace(@"\", "/").Replace(Application.dataPath, "Assets");
            string abName = Path.GetFileNameWithoutExtension(packPath);
            //获取文件扩展名
            string extension = Path.GetExtension(packPath);
            
            //分类打包
            switch (extension)
            {
                case ".prefab":
                    abName = $"Prefab/{abName}";
                    break;
                case ".mat":
                    abName = $"Material/{abName}";
                    break;
                case ".mp3":
                case ".mp4":
                    abName = $"Audio/{abName}";
                    break;
                case ".jpg":
                    abName = $"Jpg/{abName}";
                    break;
                case ".png":
                    abName = $"Png/{abName}";
                    break;
            }
            

            AssetImporter importer = AssetImporter.GetAtPath(packPath);
            importer.assetBundleName = abName;
            importer.assetBundleVariant = "u3d";
            
            //资源的MD5码（做为资源的改变判定）
            string md5 = GetMD5(path);
            //拼成设计的资源格式（AB包资源名 + 资源的MD5码）
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
    /// 转化一个资源的MD5码
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private static string GetMD5(string path)
    {
        byte[] data = File.ReadAllBytes(path);
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] mData = md5.ComputeHash(data);
        return BitConverter.ToString(mData).Replace("-", "");
    }
    /// <summary>
    /// 保存资源清单文件
    /// </summary>
    /// <param name="text"></param>
    static void SaveAssetsMainfast(string text)
    {
        var version_path = $"{Application.streamingAssetsPath}/{Application.version}";
        var assMf_path = $"{Application.streamingAssetsPath}/{Application.version}/AssetMainfast.txt";

        if (!Directory.Exists(version_path))
        {
            Directory.CreateDirectory(version_path);
        }
        File.WriteAllText(assMf_path, text);
    }
    /// <summary>
    /// 保存版本号文件
    /// </summary>
    static void SaveAssetsVersion()
    {
        File.WriteAllText($"{Application.streamingAssetsPath}/Version.txt", Application.version);
    }
}

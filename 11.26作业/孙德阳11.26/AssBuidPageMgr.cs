using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;


public class AssBuidPageMgr : Editor
{
    [MenuItem("Tool/AssetBundle/打包（正常）")]
    public static void CreatAssBundle()
    {
        //
        AssetBundleFileBundle();
        //输出AB资源路径
        string outPath = $"{Application.dataPath}/Resources/ABFiles";
        //Debug.Log(Application.dataPath);
        Debug.Log(outPath);
        //using System.IO;检查文件是否存在，不存在就创建它
        if (!Directory.Exists(outPath))
        {
            Directory.CreateDirectory(outPath);
        }
        //编辑器提供的打包接口，输出路径，AB包的压缩方式，AB包的目标平台
        BuildPipeline.BuildAssetBundles(outPath, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows);
        //自动打开（路径）文件夹
        Process.Start(outPath);
    }

    private static void AssetBundleFileBundle()
    {
        //定义需要过滤的文件类型
        string[] filtrateArr=new string[]{".meta",".pdf"};
        
        //获取这个路径下的所有文件
        string[] allFiles = Directory.GetFiles($"{Application.dataPath}/Resources/ABPackage", "*.*", SearchOption.AllDirectories);
        //where，保留符合的条件                ==》 //！是否包含的文件，获取文件完整 名称如（text.meta）.转换为数组，重新赋值给allFiles
        allFiles = allFiles.Where((x) => !filtrateArr.Contains(Path.GetExtension(x))).ToArray();

        foreach (var fp in allFiles)
        {
            //将"\"换成"/"避免路径格式错误         //将（此项目绝对路径）替换为Assets//得到packPath路径
            string packPath=fp.Replace(@"\","/").Replace(Application.dataPath, "Assets");
            //获取文件名，不带后缀的文件名
            string abName = Path.GetFileNameWithoutExtension(packPath);
            //获取importer，根据路径导入对应的实例//AssetImporter的类型
            AssetImporter importer = AssetImporter.GetAtPath(packPath);
            //将当前AB包的名称=abName
            importer.assetBundleName=abName;
            //将当前AB包的变体="u3d"
            importer.assetBundleVariant="u3d";
        }
        
        
    }
}



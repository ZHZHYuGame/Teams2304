using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
/// <summary>
/// AB���Ľṹ
/// </summary>
public class ABAsset
{
    public string abName;
    public string md5;
    public string abDirectory;
    public ABAsset(string abStr)
    {
        string[] abStrArr = abStr.Split('|');
        abName = abStrArr[0];
        //获取目录
        StringBuilder sb = new StringBuilder();
        string[] abDirectoryArr = abStrArr[0].Split('/');
        for (int i = 0; i < abDirectoryArr.Length -1; i++)
        {
            sb.Append($"{abDirectoryArr[i]}/");
        }
        abDirectory = sb.ToString();
        md5 = abStrArr[1];
    }
}

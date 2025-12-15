using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// AB包的结构
/// </summary>
public class ABAsset
{
    public string abName;
    public string md5;

    public ABAsset(string abStr)
    {
        string[] abStrArr = abStr.Split('|');
        abName = abStrArr[0];
        md5 = abStrArr[1];
    }
}

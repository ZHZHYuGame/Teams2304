using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABAsset
{
    //public AssetBundle ab;
    public string abName;
    public string md5;
    public bool isDel;
    public ABAsset(string abStr)
    {
        string[] abStrArr = abStr.Split('|');
        abName = abStrArr[0];
        md5 = abStrArr[1];
    }
}

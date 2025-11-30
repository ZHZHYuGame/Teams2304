using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionData
{
    public int big;
    public int middle;
    public int small;
    public string vStr;
    public VersionData(string version)
    {
        vStr = version;
        string[]verArr=version.Split('.');
        big=int.Parse(verArr[0]);
        middle=int.Parse(verArr[1]);
        small=int.Parse(verArr[2]);
    }
    public override string ToString()
    {
        return vStr;
    }
}

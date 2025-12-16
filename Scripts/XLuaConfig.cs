using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class XLuaConfig
{
   
}
// 必须放在类外部，作为全局委托
[CSharpCallLua]
public delegate void UnityActionBool(bool isOn);


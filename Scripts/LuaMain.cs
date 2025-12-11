using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

public class LuaMain : Singleton<LuaMain>
{
    LuaEnv lua=new LuaEnv();
    public Action start, updata;
    // Start is called before the first frame update
   public void Start()
    {
        lua.AddLoader(Cust);
        lua.DoString("require 'LuaMain'");
        start = lua.Global.Get<Action>("LuaStart");
        updata = lua.Global.Get<Action>("LuaUpdata");
        start?.Invoke();
    }

    private byte[] Cust(ref string filepath)
    {
        return File.ReadAllBytes(Application.dataPath + "/Lua/" + filepath + ".lua");
    }

    // Update is called once per frame
    public void Update()
    {
        updata?.Invoke();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

public class LuaEnvMgr : Singleton<LuaEnvMgr>
{
    private LuaEnv lua_Env;
    Action update_act,start_act;
    public void Start()
    {
        lua_Env = new LuaEnv();
        lua_Env.AddLoader(CustomLoaderHandle);
        lua_Env.DoString("require 'main/LuaMain' ");
        start_act = lua_Env.Global.Get<Action>("Lua_Start");
        update_act = lua_Env.Global.Get<Action>("Lua_Update");
        
        start_act?.Invoke();
    }

    private byte[] CustomLoaderHandle(ref string filepath)
    {
        return File.ReadAllBytes($"{Application.dataPath}/Script/Lua/{filepath}.lua");
    }

    public void Update()
    {
        update_act?.Invoke();
    }
}

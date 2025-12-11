using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CShap_Handle_Lua_Tool: Singleton<CShap_Handle_Lua_Tool>
{

    Action<Net_To_Lua_Data> lua_Handle_Net_Msg;
    
    
    /// <summary>
    /// 注册网络消息发送给lua的回调函数
    /// </summary>
    /// <param name="handle"></param>
    public void Bind_Net_To_Lua_Handle(Action<Net_To_Lua_Data> handle)
    {
        lua_Handle_Net_Msg = handle;
    }

    public void Call_Lua_Net_Msg(Net_To_Lua_Data data)
    {
        lua_Handle_Net_Msg?.Invoke(data);
    }

}

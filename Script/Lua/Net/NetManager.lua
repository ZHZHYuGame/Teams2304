Lua_NetManager = BaseClass("NetManager")

function Handle_Net_Msg(netData)

    NetMessageControll:Dispatch(netData.netID, netData.byteData)

    
end

function Lua_NetManager:Init()
    CShap_Handle_Lua_Tool.GetInstance():Bind_Net_To_Lua_Handle(Handle_Net_Msg)
end

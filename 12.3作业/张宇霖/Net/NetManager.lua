Net_Manager = BaseClass("NetManager")

function Handle_Net_Msg(netData)

    NetMessageControll:Dispatch(netData.netID, netData.byteData)

    
end

function Net_Manager:Init()
    CShap_Handle_Lua_Tool.GetInstance():Bind_Net_To_Lua_Handle(Handle_Net_Msg)
end

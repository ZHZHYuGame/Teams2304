Lua_NetManager = BaseClass("NetManager")

function Handle_Net_Msg(netData)
    local byteData = {}
    if netData.netID == NetID.S_To_C_GetPlayerData_Msg then
        byteData = MyGame.S_To_C_GetPlayerData_Msg.Parser:ParseFrom(netData.byteData)
    elseif netData.netID == NetID.S_To_C_UseBagItem_Msg then
        byteData = MyGame.S_To_C_UseBagItem_Msg.Parser:ParseFrom(netData.byteData)
    end
    NetMessageControll:Dispatch(netData.netID, byteData)
end

function Lua_NetManager:Init()
    CShap_Handle_Lua_Tool.GetInstance():Bind_Net_To_Lua_Handle(Handle_Net_Msg)
end

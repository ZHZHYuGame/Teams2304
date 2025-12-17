Lua_NetManager = BaseClass("NetManager")

function Handle_Net_Msg(netData)
    local byteData = nil
    if netData.netID == NetID.S_To_C_GetPlayerData_Msg then
        byteData = MyGame.S_To_C_GetPlayerData_Msg.Parser:ParseFrom(netData.byteData)
    elseif netData.netID == NetID.S_To_C_UseBagItem_Msg then
        byteData = MyGame.S_To_C_UseBagItem_Msg.Parser:ParseFrom(netData.byteData)
    end

    if byteData ~=nil then
        NetMessageControll:Dispatch(netData.netID, byteData)
    end
    
end

function Lua_NetManager:Init()
    CShap_Handle_Lua_Tool.GetInstance():Bind_Net_To_Lua_Handle(Handle_Net_Msg)
end

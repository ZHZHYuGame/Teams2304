NetManager = BaseClass("NetManager")

function Handle_Net_Msg(netData)
    local s_Msg = nil
    -- if netData.netID == NetID.S_To_C_GetShopData_Msg then
    --     s_Msg = MyGame.S_To_C_GetShopData_Msg.Parser:ParseFrom(netData.byteData)
    -- end
    -- if netData.netID==NetID.S_To_C_Get_MainData_Msg then
    --     s_Msg = MyGame.S_To_C_Get_Data_Msg.Parser:ParseFrom(netData.byteData)
    -- end
    -- if netData.netID==NetID.S_To_C_BuyShopData_Msg then
    --     s_Msg = MyGame.S_To_C_But_Good_Msg.Parser:ParseFrom(netData.byteData)
    -- end
    -- if netData.netID==NetID.S_To_C_Get_BagData_Msg then
    --     s_Msg = MyGame.S_To_C_GetBagData_Msg.Parser:ParseFrom(netData.byteData)
    -- end
    -- if netData.netID==NetID.S_To_C_Add_BagData_Msg then
    --     s_Msg = MyGame.S_To_C_AddBagData_Msg.Parser:ParseFrom(netData.byteData)
    -- end
    -- if netData.netID==NetID.S_To_C_Add_BagItem_Msg then
    --     s_Msg=MyGame.S_To_C_Add_BagItem_Msg.Parser:ParseFrom(netData.byteData)
    -- end
    -- if netData.netID==NetID.S_To_C_Organize_Msg then
    --     s_Msg=MyGame.S_To_C_Organize_Msg.Parser:ParseFrom(netData.byteData)
    -- end
    NetMessageControll:Dispatch(netData.netID, s_Msg)
end

function NetManager:Init()
    CShap_Handle_Lua_Tool.GetInstance():Bind_Net_To_Lua_Handle(Handle_Net_Msg)
end

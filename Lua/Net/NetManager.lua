NetManager = BaseClass("NetManager")

function NetManager:Init()
    -- 绑定C#到Lua的网络消息回调
    CShap_Handle_Lua_Tool.GetInstance():Bind_Net_To_Lua_Handle(Bind(self, self.Handle_Net_Msg))
end

function NetManager:Handle_Net_Msg(netData)
    local s_Msg = nil
    if netData.netID == NetID.S_To_C_Main then
        s_Msg = MyGame.S_To_C_Main.Parser:ParseFrom(netData.byteData)
    elseif netData.netID == NetID.S_To_C_ShopGoods then
        s_Msg = MyGame.S_To_C_ShopGoods.Parser:ParseFrom(netData.byteData)
    elseif netData.netID == NetID.S_To_C_BagGoods then
        s_Msg = MyGame.S_To_C_BagGoods.Parser:ParseFrom(netData.byteData)
    elseif netData.netID == NetID.S_To_C_SendGood then
        s_Msg = MyGame.S_To_C_SendGood.Parser:ParseFrom(netData.byteData)
    elseif netData.netID == NetID.S_To_C_Log then
        s_Msg = MyGame.S_To_C_Log.Parser:ParseFrom(netData.byteData)
    elseif netData.netID == NetID.S_To_C_Register then
        s_Msg = MyGame.S_To_C_Register.Parser:ParseFrom(netData.byteData)
    end
    -- 分发消息到NetMessageControll
    NetMessageControll:Dispatch(netData.netID, s_Msg)
end

NetManager = BaseClass("NetManager")

function NetManager:Init()
    -- 绑定C#到Lua的网络消息回调
    CShap_Handle_Lua_Tool.GetInstance():Bind_Net_To_Lua_Handle(Bind(self, self.Handle_Net_Msg))
end
function NetManager:Handle_Net_Msg(netData)
    local s_Msg = nil

    
    -- 分发消息到NetMessageControll
    NetMessageControll:Dispatch(netData.netID, s_Msg)
end

local MainsurfaceControll = BaseClass("MainsurfaceControll")

function MainsurfaceControll:__init(view, model)

    self.view = view
    self.model = model
    self:AddListener()
end



function MainsurfaceControll:AddListener()
    NetMessageControll:AddListener(NetID.S_To_C_GetPlayerData_Msg, Bind(self, self.S_To_C_GetPlayerData_Msg_handle));
end

function MainsurfaceControll:S_To_C_GetPlayerData_Msg_handle(s_msg)
    --self.model
    self.model:InitData(s_msg.PlayerData);
    self.view:InitUIView(self.model.playerData);
    --背包
    local bagDict = s_msg.BagList
    UIMessageControll:Dispatch(ClientID.Init_BagData, bagDict);
end 

function MainsurfaceControll:OneOpenUI()
end

function MainsurfaceControll:RemoveListener()

end

return MainsurfaceControll

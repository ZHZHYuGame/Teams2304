local MainSurfaceControll = BaseClass("MainSurfaceControll")

function MainSurfaceControll:__init()
	self:AddListener()
end

function MainSurfaceControll:S_To_C_GetPlayerInfo_msg_Handle(byteData)
	local s_msg = MyGame.S_To_C_GetPlayerInfo_Msg.Parser:ParseFrom(byteData);

	local playerDataTable = 
	{
		gold = s_msg.PlayerInfo.Gold,
		level = s_msg.PlayerInfo.Level
	}
	self.model:OnInitData(playerDataTable)	
	self.view:OnInitView(self.model.playerDataTable);

end

function MainSurfaceControll:Change_MainSurface_Gold_Handle(anyTab)
	local gold = anyTab[1]
	self.model:RefreshGold(gold)
	self.view:RefreshGold(self.model.playerDataTable.gold)
end
function MainSurfaceControll:AddListener()
	NetMessageControll:AddListener(NetID.S_To_C_GetPlayerInfo_msg,Bind(self, self.S_To_C_GetPlayerInfo_msg_Handle))
	UIMessageControll:AddListener(ClientID.Change_MainSurface_Gold,Bind(self, self.Change_MainSurface_Gold_Handle));
end

function MainSurfaceControll:RemoveListener()

end

return MainSurfaceControll

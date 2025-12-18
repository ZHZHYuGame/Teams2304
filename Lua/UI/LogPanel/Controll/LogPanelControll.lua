local LogPanelControll = BaseClass("LogPanelControll")

function LogPanelControll:__init()
	self:AddListener()
end

function LogPanelControll:AddListener()
	NetMessageControll:AddListener(NetID.S_To_C_Log, Bind(self, self.S_To_C_Log_Handel))
end

function LogPanelControll:S_To_C_Log_Handel(type)
	self.type = type[1].Type
	if self.type == MyGame.RegisterOrLogEndType.UserNameNull then
		print("账号不能为空")
	elseif self.type == MyGame.RegisterOrLogEndType.PassWordNull then
		print("密码不能为空")
	elseif self.type == MyGame.RegisterOrLogEndType.UserNameNotHave then
		print("账号未注册")
	elseif self.type == MyGame.RegisterOrLogEndType.PassWordMistake then
		print("密码错误")
	elseif self.type == MyGame.RegisterOrLogEndType.Success then
		print("登录成功")
	end
end

function LogPanelControll:RemoveListener()

end

return LogPanelControll

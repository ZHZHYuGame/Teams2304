local RegisterPanelControll = BaseClass("RegisterPanelControll")

function RegisterPanelControll:__init()
	self:AddListener()
end

function RegisterPanelControll:AddListener()
	NetMessageControll:AddListener(NetID.S_To_C_Register, Bind(self, self.S_To_C_Register_Handel))
end

function LogPanelControll:S_To_C_Register_Handel(type)
	self.type = type[1].Type
	if self.type == MyGame.RegisterOrLogEndType.UserNameNull then
		print("账号不能为空")
	elseif self.type == MyGame.RegisterOrLogEndType.PassWordNull then
		print("密码不能为空")
	elseif self.type == MyGame.RegisterOrLogEndType.UserNameHave then
		print("账号已注册")
	elseif self.type == MyGame.RegisterOrLogEndType.Success then
		print("注册成功")
	end
end

function RegisterPanelControll:RemoveListener()

end

return RegisterPanelControll

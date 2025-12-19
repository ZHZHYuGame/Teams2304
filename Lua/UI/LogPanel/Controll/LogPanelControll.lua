local LogPanelControll = BaseClass("LogPanelControll")

function LogPanelControll:__init()
	self:AddListener()
end

function LogPanelControll:AddListener()
	NetMessageControll:AddListener(NetID.S_To_C_Log, Bind(self, self.S_To_C_Log_Handel))
end

function LogPanelControll:S_To_C_Log_Handel(type)
	self.type = type[1].Type
	self.name = type[1].UserName
	self.pass = type[1].PassWord
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
		_G.UImgr:CloseUI(UITypeEnum.log, false)
		CS.Tool.SaveLogData(self.name, self.pass)
		SceneManager.LoadScene("SampleScene")
		_G.UImgr:ShowUI(UITypeEnum.main)
		_G.UImgr:ShowUI(UITypeEnum.shop)
		_G.UImgr:ShowUI(UITypeEnum.bag)
	end
end

function LogPanelControll:RemoveListener()

end

return LogPanelControll

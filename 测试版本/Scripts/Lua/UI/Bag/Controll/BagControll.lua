local BagControll = BaseClass("BagControll")

function BagControll:__init()
	self:AddListener()
end

function BagControll:OnStart()
	self:BindUIEvent()
end

function BagControll:AddListener()
	UIMessageControll:AddListener(ClientID.Tips_BagData, Bind(self, self.Tips_BagData_Msg))
end

function BagControll:Tips_BagData_Msg(data)
	if data[1].config.Type == "资源" then
		for key, value in pairs(self.model.bagDataDict[0]) do
			if data[1].id == value.id then
				self.view:TipsInitData(value)
				break
			end
		end
	elseif data[1].config.Type == "加速" then
		for key, value in pairs(self.model.bagDataDict[1]) do
			if data[1].id == value.id then
				self.view:TipsInitData(value)
				break
			end
		end
	elseif data[1].config.Type == "增益" then
		for key, value in pairs(self.model.bagDataDict[2]) do
			if data[1].id == value.id then
				self.view:TipsInitData(value)
				break
			end
		end
	elseif data[1].config.Type == "装备" then
		for key, value in pairs(self.model.bagDataDict[3]) do
			if data[1].id == value.id then
				self.view:TipsInitData(value)
				break
			end
		end
	elseif data[1].config.Type == "其他" then
		for key, value in pairs(self.model.bagDataDict[4]) do
			if data[1].id == value.id then
				self.view:TipsInitData(value)
				break
			end
		end
	end
end

function BagControll:OneOpenUI()
	self.view:InitData(self.model.bagDataDict[0]);
end

function BagControll:BindUIEvent()
	self.view.resTog.onValueChanged:AddListener(
		function(isOn)
			if isOn then
				self:GetTypeofBagData(0)
			end
		end
	)

	self.view.boostTog.onValueChanged:AddListener(
		function(isOn)
			if isOn then
				self:GetTypeofBagData(1)
			end
		end
	)

	self.view.buffTog.onValueChanged:AddListener(
		function(isOn)
			if isOn then
				self:GetTypeofBagData(2)
			end
		end
	)

	self.view.outfitTog.onValueChanged:AddListener(
		function(isOn)
			if isOn then
				self:GetTypeofBagData(3)
			end
		end
	)

	self.view.otherTog.onValueChanged:AddListener(
		function(isOn)
			if isOn then
				self:GetTypeofBagData(4)
			end
		end
	)
	for key, value in pairs(self.model.bagDataDict[0]) do
		self.view:TipsInitData(value)
		break
	end
	self.view.Jian.onClick:AddListener(function()
		if self.view.StartNum > 1 then
			self.view.StartNum = self.view.StartNum - 1
			self.view.NumText.text = self.view.StartNum
		else
			self.view.StartNum = 1
			self.view.NumText.text = self.view.StartNum
		end
	end)
	self.view.Add.onClick:AddListener(function()
		if self.view.StartNum < self.view.NowBagData.count then
			self.view.StartNum = self.view.StartNum + 1
			self.view.NumText.text = self.view.StartNum
		else
			self.view.StartNum = self.view.NowBagData.count
			self.view.NumText.text = self.view.StartNum
		end
	end)
	self.view.MaxBtn.onClick:AddListener(function()
		self.view.StartNum = self.view.NowBagData.count
		self.view.NumText.text = self.view.StartNum
	end)
	self.view.UseBtn.onClick:AddListener(function()
		local AddResType = nil
		local itemType = nil
		if self.view.NowBagData.config.Type == "资源" then
			AddResType = 0
			itemType = 0 
		elseif self.view.NowBagData.config.Type == "加速" then
			AddResType = 1
			itemType = 0
		elseif self.view.NowBagData.config.Type == "增益" then
			AddResType = 2
			itemType = 0
		elseif self.view.NowBagData.config.Type == "装备" then
			AddResType = 3
			itemType = 0
		elseif self.view.NowBagData.config.Type == "其他" then
			AddResType = 4
			itemType = 0
		end
		local useData = MyGame.C_To_S_UseBagItem_Msg()
		useData.BagData = MyGame.NetBagData
		{
			
		}
		useData.ItemType = itemType
		useData.AddType = AddResType
		useData.UpdateCount = self.view.StartNum
		NetManager.GetInstance():SendMessage(NetID.C_To_S_UseBagItem_Msg, Protobuf.ToByteArray(useData))
	end)
end

function BagControll:GetTypeofBagData(type)
	if self.model.bagDataDict[type] == nil then
		print("not type of BagData!")
		return
	end
	self.view:InitData(self.model.bagDataDict[type])
end

function BagControll:RemoveListener()

end

return BagControll

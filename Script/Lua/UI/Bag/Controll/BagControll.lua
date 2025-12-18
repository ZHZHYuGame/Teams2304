local BagControll = BaseClass("BagControll")

function BagControll:__init(view, model)
	self.view = view
	self.model = model
	self.currentType = 0
	self.currentNum = 1
	self.currentBagData = {}
	self:AddListener()
	self:BindUIEvent()
	self:OneOpenUI()
	
end


function BagControll:AddListener()
	UIMessageControll:AddListener(ClientID.Tips_BagData, Bind(self, self.Tips_BagData_Msg))
end

function BagControll:Tips_BagData_Msg(table)
	local id = table[1]
	local bagData = self.model.bagDataDict[self.currentType][id]
	self.currentBagData = bagData
	self.currentNum = 1
	if bagData ~= nil then
		self.view:TipsInitData(bagData)
	else
		print("背包中没有此数据")
	end
	
end

function BagControll:OneOpenUI()
	self.currentBagData = self.model.bagDataDict[0][1]
	self.view:InitData(self.model.bagDataDict[0]);
	for key, value in pairs(self.model.bagDataDict[0]) do
		self.view:TipsInitData(value)
		break
	end
end

function BagControll:BindUIEvent()
	self.view.resTog.onValueChanged:AddListener(
		function(isOn)
			if isOn then
				self:GetTypeofBagData(0)
				self.currentType = 0
			end
		end
	)

	self.view.boostTog.onValueChanged:AddListener(
		function(isOn)
			if isOn then
				self:GetTypeofBagData(1)
				self.currentType = 1
			end
		end
	)

	self.view.buffTog.onValueChanged:AddListener(
		function(isOn)
			if isOn then
				self:GetTypeofBagData(2)
				self.currentType = 2
			end
		end
	)

	self.view.outfitTog.onValueChanged:AddListener(
		function(isOn)
			if isOn then
				self:GetTypeofBagData(3)
				self.currentType = 3
			end
		end
	)

	self.view.otherTog.onValueChanged:AddListener(
		function(isOn)
			if isOn then
				self:GetTypeofBagData(4)
				self.currentType = 4
			end
		end
	)
	self.view.Jian.onClick:AddListener(function()
		if self.currentNum > 1 then
			self.currentNum = self.currentNum - 1
			self.view.NumText.text = tostring(self.currentNum)
		
		end
	end)
	self.view.Add.onClick:AddListener(function()
		if self.currentNum < self.currentBagData.count then
			self.currentNum = self.currentNum + 1
			self.view.NumText.text = tostring(self.currentNum)
		else
			self.currentNum = self.currentBagData.count
			self.view.NumText.text = tostring(self.currentNum)
		end
	end)
	self.view.MaxBtn.onClick:AddListener(function()
		self.currentNum = self.currentBagData.count
		self.view.NumText.text = tostring(self.currentNum)
	end)
	self.view.UseBtn.onClick:AddListener(function()
		self:UseItem()
	end)
end


function BagControll:UseItem()

	local AddResType = nil
	local itemType = nil
	if self.view.NowBagData.config.Type == "资源" then
		AddResType = 0
		itemType = MyGame.ItemType.Res 

		elseif self.view.NowBagData.config.Type == "增益" then
			
			itemType = MyGame.ItemType.Buff 
	end
		local useData = MyGame.C_To_S_UseBagItem_Msg()
		useData.BagData = MyGame.NetBagData
		{
			  
		}
		useData.ItemType = itemType
		useData.AddType = AddResType
		useData.UpdateCount = self.view.StartNum
		NetManager.GetInstance():SendMessage(NetID.C_To_S_UseBagItem_Msg, Protobuf.ToByteArray(useData))
end

function BagControll:GetTypeofBagData(type)
	if self.model.bagDataDict[type] == nil then
		print("not type of BagData!")
		return
	end

	self.view:InitData(self.model.bagDataDict[type])
	self.currentBagData = self.model.bagDataDict[type][1]
	self.currentNum = 1
	self.view:TipsInitData(self.model.bagDataDict[type][1])
	
end

function BagControll:RemoveListener()

end

return BagControll

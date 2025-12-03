local shopControll = BaseClass("shopControll")

function shopControll:__init()
	self:AddListener()
end

function shopControll:AddListener()
	NetMessageControll:AddListener(NetID.S_To_C_GoodData_Msg, Bind(self, self.S_To_C_GoodData_Handle))
end

function shopControll:S_To_C_GoodData_Handle(shopdata)
	self.data = shopdata[1]
	self.view:ShowShopItem(self.data)
end

function shopControll:RemoveListener()

end

return shopControll

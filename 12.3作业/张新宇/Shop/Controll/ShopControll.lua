local ShopControll = BaseClass("ShopControll")

function ShopControll:__init()
	self:AddListener()
end

function ShopControll:SetShopData(data)
	self.model.config = data[1].Shopdatas
	print(self.model.config.Count)
	self.view:ShopShowItem(self.model.config)
	-- print(data[1].Shopdatas.Count)
	-- for i = 0, data[1].Shopdatas.Count - 1, 1 do
	-- 	print(data[1].Shopdatas[i].Id)
	-- end
end

function ShopControll:AddListener()
	NetMessageControll:AddListener(NetID.S_To_C_GetShopData_Msg, Bind(self, self.SetShopData))
end

function ShopControll:RemoveListener()

end

return ShopControll

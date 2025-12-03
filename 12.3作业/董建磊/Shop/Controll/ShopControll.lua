local ShopControll = BaseClass("ShopControll")

function ShopControll:__init()
	self:AddListener()
end

function ShopControll:S_To_C_GoodsData_Msg_Handle(ShopGoods)
	self.m_Data = ShopGoods[1]
	self.view:ShowShopItem(self.m_Data)
	--self.code_View:ShowShopItem(self.m_Data)
end

function ShopControll:AddListener()
	NetMessageControll:AddListener(NetID.S_To_C_GoodsData_Msg, Bind(self, self.S_To_C_GoodsData_Msg_Handle))
end

function ShopControll:RemoveListener()

end

return ShopControll

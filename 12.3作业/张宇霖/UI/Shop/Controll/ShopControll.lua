local ShopControll = BaseClass("ShopControll")

function ShopControll:__init()
	self:AddListener()
end

function ShopControll:S_To_C_GetShopInfos_msg_Handle(data)
	--解析数据
	local s_msg = MyGame.S_To_C_GetShopInfos_Msg.Parser:ParseFrom(data);
	-- print("s_msg = ", s_msg.ShopInfoList)
	self:RefreshView(s_msg)
	
end

function ShopControll:RefreshView(s_msg)
	self.model:RefreshTable(s_msg.ShopInfoList)
	self.view:RefreshView(self.model.shopDataTable)

end

function ShopControll:BuyShop(id)
	local c_msg = MyGame.C_To_S_GetShopInfos_Msg()
    NetManager.GetInstance():SendMessage(NetID.C_To_S_GetShopInfos_msg,Protobuf.ToByteArray(c_msg))
end

function ShopControll:AddListener()
	NetMessageControll:AddListener(NetID.S_To_C_GetShopInfos_msg,Bind(self, self.S_To_C_GetShopInfos_msg_Handle))
end



function ShopControll:RemoveListener()

end

return ShopControll

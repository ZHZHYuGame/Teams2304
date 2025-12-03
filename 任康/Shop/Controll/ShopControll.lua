local ShopControll = BaseClass("ShopControll")

function ShopControll:__init()
	self:AddListener()
end

function ShopControll:S_To_C_GetShopInfos_msg_Handle(data)
	--解析数据
	local s_msg = MyGame.S_To_C_GetShopInfos_Msg.Parser:ParseFrom(data);
	-- print("s_msg = ", s_msg.ShopInfoList)
	self:OnInitData(s_msg)
	
end

function ShopControll:OnInitData(s_msg)
	self.model:OnInitData(s_msg.ShopInfoList)
	self.view:OnInitView(self.model.shopDataTable)

end


function ShopControll:S_To_C_BuyShop_msg_Handle(byteData)
	local s_msg = MyGame.S_To_C_BuyShop_Msg.Parser:ParseFrom(byteData);
	if s_msg.BuyR == MyGame.BuyResult.Sucess then
		print("购买成功! ==> ",s_msg.ShopInfo.Id)

		local shopData = self.model:RefreshData(s_msg.ShopInfo)
		if shopData ~= nil then
			self.view:RefreshItem(shopData)
		else
			print("商品刷新异常!==>", s_msg.ShopInfo.Id)
		end

		--刷新金币
		UIMessageControll:Dispatch(ClientID.Change_MainSurface_Gold, s_msg.Gold);

		else if s_msg.BuyR == MyGame.BuyResult.Gold then
				print("金币不足! ==>", s_msg.ShopInfo.Id)
			else if s_msg.BuyR == MyGame.BuyResult.Limit then
				print("商品不足! ==>" , s_msg.ShopInfo.Id)
			end
		end
	end
end
function ShopControll:AddListener()
	NetMessageControll:AddListener(NetID.S_To_C_GetShopInfos_msg,Bind(self, self.S_To_C_GetShopInfos_msg_Handle))
	NetMessageControll:AddListener(NetID.S_To_C_BuyShop_msg,Bind(self, self.S_To_C_BuyShop_msg_Handle))
end




function ShopControll:RemoveListener()

end

return ShopControll

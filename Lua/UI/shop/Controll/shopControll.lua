local shopControll = BaseClass("shopControll")

function shopControll:__init()
    self.model = nil 
    self.view = nil
    local getShopData = MyGame.C_To_S_ShopGoods()
    CS.NetManager.GetInstance():SendMessage(CS.NetID.C_To_S_ShopGoods, Protobuf.ToByteArray(getShopData))
    self:AddListener()
end

function shopControll:AddListener()
    NetMessageControll:AddListener(NetID.S_To_C_ShopGoods, Bind(self, self.GetShopDataRefreshed))
end
function shopControll:GetShopDataRefreshed(ShopDataList)
    self.data = ShopDataList[1].Goodsdatalist  
    self.model:UpdateShopData(self.data)
    if self.view then
        self.view:RefreshShopUI(self.model:GetShopData())
    end
end
function shopControll:RemoveListener()

end

return shopControll

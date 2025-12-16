local BagControll = BaseClass("BagControll")

function BagControll:__init()
    local getShopData = MyGame.C_To_S_BagGoods()
    CS.NetManager.GetInstance():SendMessage(CS.NetID.C_To_S_BagGoods, Protobuf.ToByteArray(getShopData))
    self:AddListener()
    NetMessageControll:AddListener(NetID.S_To_C_BuyGood, Bind(self, self.S_To_C_BuyGoods_Handle))
end

function BagControll:AddListener()
    NetMessageControll:AddListener(NetID.S_To_C_BagGoods, Bind(self, self.S_To_C_BagGoods_Handle))
end

function BagControll:S_To_C_BagGoods_Handle(data)
    self.model:SetDatas(data[1].Bagitemlist)
    self.view:UpdateBagUI(data[1].Bagitemlist)
end
function BagControll:S_To_C_BuyGoods_Handle(data)
    print(111111)
end
function BagControll:RemoveListener()

end

return BagControll

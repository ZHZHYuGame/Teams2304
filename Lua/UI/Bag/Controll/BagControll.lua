local BagControll = BaseClass("BagControll")

function BagControll:__init()
    local getShopData = MyGame.C_To_S_BagGoods()
    CS.NetManager.GetInstance():SendMessage(CS.NetID.C_To_S_BagGoods, Protobuf.ToByteArray(getShopData))
    self:AddListener()
    NetMessageControll:AddListener(NetID.S_To_C_SendGood, Bind(self, self.S_To_C_SendGood_Handle))
end

function BagControll:AddListener()
    NetMessageControll:AddListener(NetID.S_To_C_BagGoods, Bind(self, self.S_To_C_BagGoods_Handle))
end

function BagControll:S_To_C_BagGoods_Handle(data)
    self.model:SetDatas(data[1].Bagitemlist)
    self.view:UpdateBagUI(self.model:GetBagDatas())
end
function BagControll:S_To_C_SendGood_Handle(data)
    self.model:SetDatas(data[1].Bagitemlist)
    self.view:RefreshBagItemUI(self.model:GetBagDatas())
end
function BagControll:RemoveListener()

end

return BagControll

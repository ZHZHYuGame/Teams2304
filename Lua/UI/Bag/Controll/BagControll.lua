local BagControll = BaseClass("BagControll")

function BagControll:__init()
    local getShopData = MyGame.C_To_S_BagGoods()
    CS.NetManager.GetInstance():SendMessage(CS.NetID.C_To_S_BagGoods, Protobuf.ToByteArray(getShopData))
    self:AddListener()
    NetMessageControll:AddListener(NetID.S_To_C_SendGood, Bind(self, self.S_To_C_SendGood_Handle))
end

--UI逻辑层的红点注册方法
function BagControll:RegisterRedPointListener()
    Red_Mgr:Bind_RedPiontData_To_UI_Handle(UIRedPointType.UI_Bag_AllIcon, Bind(self, self.Update_RedPoint))
    Red_Mgr:Bind_RedPiontData_To_UI_Handle(UIRedPointType.UI_Bag_BaoIcon, Bind(self, self.Update_RedPoint))
    Red_Mgr:Bind_RedPiontData_To_UI_Handle(UIRedPointType.UI_Bag_EquipIcon, Bind(self, self.Update_RedPoint))
    Red_Mgr:Bind_RedPiontData_To_UI_Handle(UIRedPointType.UI_Bag_YaoIcon, Bind(self, self.Update_RedPoint))
end

--UI逻辑层的红点删除方法
function BagControll:RemoveRedPointListener()

end

function BagControll:Update_RedPoint(type, bool)
    local redPoint;
    if type == UIRedPointType.UI_Bag_AllIcon then
        redPoint = self.view.All_RedPoint
    end
    if type == UIRedPointType.UI_Bag_BaoIcon then
        redPoint = self.view.other_RedPoint
    end
    if type == UIRedPointType.UI_Bag_EquipIcon then
        redPoint = self.view.Equipment_RedPoint
    end
    if type == UIRedPointType.UI_Bag_YaoIcon then
        redPoint = self.view.other_RedPoint
    end
    if redPoint ~= nil then
        if bool then
            redPoint.transform.localScale = Vector3(1, 1, 1)
        else
            redPoint.transform.localScale = Vector3(0, 0, 0)
        end
    end
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
    local goodsdata = data[1].Bagitemlist[0].Data
    print(goodsdata.InventoryType)

    if goodsdata.InventoryType ~= nil then
        Red_Mgr:Update_UI_RedPoint(UIRedPointType.UI_Bag_AllIcon, true)
    end
    if goodsdata.InventoryType == "装备" then
        Red_Mgr:Update_UI_RedPoint(UIRedPointType.UI_Bag_EquipIcon, true)
    end
    if goodsdata.InventoryType == "药品" then
        Red_Mgr:Update_UI_RedPoint(UIRedPointType.UI_Bag_YaoIcon, true)
    end
    if goodsdata.InventoryType == "宝箱" or goodsdata.InventoryType == "经验书" then
        Red_Mgr:Update_UI_RedPoint(UIRedPointType.UI_Bag_BaoIcon, true)
    end
end

function BagControll:RemoveListener()

end

return BagControll

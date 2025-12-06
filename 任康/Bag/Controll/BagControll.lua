local bagControll = BaseClass("BagControll")

function bagControll:__init()
    self:AddListener()
end

function bagControll:Init_BagData_Handle(table)
    
    local myTable = table[1]
    local bagTable = {}
    for i = 1, #myTable, 1 do
        local bagData = 
        {
            id = i,
            config = ConfigManager:GetShopConfig(myTable[i].ItemId),
            num = myTable[i].Count,
        }

        bagTable[i] = bagData
    end

    self.model:OnInitData(bagTable);

    self.view:OnInitView(self.model.bagTable)

end

function bagControll:Update_BagData_Handle(table)
    local bagData = table[1]
    --刷新背包数据
    local bagItemData = self.model:UpdateData(bagData)
    self.view:RefreshItem(bagItemData)
end

--刷新红点
function bagControll:Update_RedPoint_Handle(redPointData)
    --查找要刷新红点的预制件
    local id = redPointData.BagInfo.CellId
    local itemData = ConfigManager:GetShopConfig(redPointData.BagInfo.ItemId)
    if self.view[id] ~= nil then
        local bagItem = self.view[id]

        self:RegisterRedPointData(itemData.inventoryType, bagItem.name)

        

    end


end


function bagControll:RegisterRedPointData(itemType, childrenType)
    if itemType == "装备" then
        _G.redPointManager:Bind_UI_To_RedPoint
        (UIRedPointType.UI_Bag_EquipIcon, childrenType,
            nil
        )
    else if itemType == "宝箱" then
            _G.redPointManager:Bind_UI_To_RedPoint
            (UIRedPointType.UI_Bag_BoxIcon, childrenType,
                nil
            )
        else if itemType == "药品" then
                _G.redPointManager:Bind_UI_To_RedPoint
                (UIRedPointType.UI_Bag_MedicineIcon, childrenType,
                    nil
                )
            end
        end
    end
end


function bagControll:AddListener()
    UIMessageControll:AddListener(ClientID.Init_BagData,Bind(self,self.Init_BagData_Handle))    
    UIMessageControll:AddListener(ClientID.Update_BagData, Bind(self,self.Update_BagData_Handle));
end

function bagControll:Remove()
    
end

return bagControll
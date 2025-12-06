local bagModel = BaseClass("BagModel")

function bagModel:__init()
    self.bagTable = {}
end

function bagModel:OnInitData(table)
    
    self.bagTable = table
end

function bagModel:UpdateData(bagData)

    local bagItemData = self.bagTable[bagData.CellId] 
    if bagItemData.num ~= 0 and bagData.ItemId == bagItemData.config.id then
        bagItemData.num = bagData.Count
    else
        --查找配置表
        local config = ConfigManager:GetShopConfig(bagData.ItemId)
        bagItemData.config = config
        bagItemData.num = bagData.Count
    end

    

    return bagItemData
end

return bagModel
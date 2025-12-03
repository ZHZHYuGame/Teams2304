local ShopModel = BaseClass("ShopModel")

function ShopModel:__init()
    self.shopDataTable = {}

end

function ShopModel:OnInitData(list)

    if self.shopDataTable == nil then
        self.shopDataTable = {}
    end

     for i = 0, list.Count - 1, 1 do
        local shopData = 
        {
            config = ConfigManager:GetShopConfig(list[i].Id),
            limit = list[i].Count
        }

        self.shopDataTable[shopData.config.id] = shopData
        --table.insert(self.shopDataTable, shopData.config.id, shopData)
        
    end

end

--刷新商品数据
function ShopModel:RefreshData(shopInfo)
    local id = tostring(shopInfo.Id)
    if self.shopDataTable[id] ~= nil then
        self.shopDataTable[id].limit = shopInfo.Count
        return self.shopDataTable[id]
    end

    return nil
end


return ShopModel

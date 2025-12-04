local ShopModel = BaseClass("ShopModel")

function ShopModel:__init()
    self.shopDataTable = {}

end

function ShopModel:RefreshTable(list)

    if self.shopDataTable == nil then
        self.shopDataTable = {}
    end

     for i = 0, list.Count - 1, 1 do
        local shopData = 
        {
            config = ConfigManager:GetShopConfig(list[i].Id),
            limit = list[i].Count
        }

        table.insert(self.shopDataTable, shopData)
    end

end

return ShopModel

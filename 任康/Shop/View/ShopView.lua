local ShopView = BaseClass("ShopView")

function ShopView:__init(prefab)
    self.gameObject = prefab

    --查找组件
    self.shopContent = self.gameObject.transform:Find("bg/shopRoot/Scroll View/Viewport/shopContent")
    self.shopItem = require("UI/Shop/Component/ShopItem")
    self.shopItemTable = {}
end

function ShopView:OnEnable()

end

function ShopView:OnInitView(table)
    for index, value in pairs(table) do
        local shopItem = self.shopItem.New(value, self.shopContent)  

        self.shopItemTable[value.config.id] = shopItem
        --table.insert(self.shopItemTable, value.config.id, self.shopItem)
    end
end

function ShopView:RefreshItem(shopData)
    --查找商品
    if self.shopItemTable[shopData.config.id] ~= nil then
        self.shopItemTable[shopData.config.id]:RefreshData(shopData)
    end
end

return ShopView

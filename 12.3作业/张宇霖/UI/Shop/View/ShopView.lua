local ShopView = BaseClass("ShopView")

function ShopView:__init(prefab)
    self.gameObject = prefab

    --查找组件
    self.shopContent = self.gameObject.transform:Find("Panel")
    self.shopItem = require("UI/Shop/Component/ShopItem")
end

function ShopView:OnEnable()

end

function ShopView:RefreshView(table)
    for index, value in ipairs(table) do
        self.shopItem.New(value, self.shopContent)
    end
end

return ShopView

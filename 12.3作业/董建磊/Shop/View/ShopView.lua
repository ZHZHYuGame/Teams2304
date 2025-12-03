local ShopView = BaseClass("ShopView")
local ShopItem = require("UI/Shop/ShopItem")
function ShopView:__init(prefab)
    self.prefab = prefab
    self.parent = prefab.transform:Find("Scroll View/Viewport/Content", typeof(CS.UnityEngine.GameObject))
    self.btn = prefab.transform:Find("CloseBtn"):GetComponent("Button")
    self.btn.onClick:AddListener(function()
        prefab.gameObject:SetActive(false)
    end)
    prefab.gameObject:SetActive(false)
end

function ShopView:ShowShopItem(shopgoods)
    self.goods = shopgoods
    for i = 0, self.goods.Goodsdatalist.Count - 1 do
        ShopItem.New(self.goods.Goodsdatalist[i], self.parent)
    end
end

function ShopView:OnEnable()

end

return ShopView

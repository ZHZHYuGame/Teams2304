local shopView = BaseClass("shopView")
local shopitem = require("UI/shop/shopitem")
local tran
function shopView:__init(prefab)
    tran = prefab.transform:GetChild(0).transform:GetChild(0).transform:GetChild(0).transform:GetChild(0).transform
        :GetChild(0).transform
end

function shopView:OnEnable()

end

function shopView:ShowShopItem(shopdata)
    self.data = shopdata
    for i = 0, self.data.Goodsdatalist.Count - 1 do
        shopitem.New(self.data.Goodsdatalist[i], tran)
    end
end

return shopView

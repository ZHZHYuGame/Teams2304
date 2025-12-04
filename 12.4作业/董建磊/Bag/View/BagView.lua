local bagView = BaseClass("bagView")
local BagItem = require("UI/Bag/BagItem")
local BagItemList = {}
function bagView:__init(prefab)
    self.gaObject = prefab
    self.prefab = prefab
    self.parent = prefab.transform:Find("Scroll View/Viewport/Content", typeof(CS.UnityEngine.GameObject))
    self.btn = prefab.transform:Find("CloseBtn"):GetComponent("Button")
    self.btn.onClick:AddListener(function()
        prefab.gameObject:SetActive(false)
    end)
    prefab.gameObject:SetActive(false)
end

function bagView:ShowBagItem(bagitemlist)
    self.bagitemlist = bagitemlist
    for i = 0, self.bagitemlist.Bagitemlist.Count - 1 do
        local item = BagItem.New(self.bagitemlist.Bagitemlist[i].Date, self.parent)
        BagItemList[i] = item
    end
end

function bagView:ShowNewBagItem(data)
    for i = 0, #BagItemList - 1 do
        if BagItemList[i].data == nil then
            BagItemList[i]:RefreshView(data)
            break
        end
    end
end

function bagView:OnEnable()

end

return bagView

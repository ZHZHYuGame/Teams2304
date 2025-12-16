local BagView = BaseClass("BagView")
local BagItem = require("UI/Bag/Component/BagItem")
function BagView:__init(prefab)
    self.prefab = prefab
    self.prefab.gameObject:SetActive(false)
    self.itemContent = prefab.transform:Find("BagPanel/Scroll View/Viewport/Content")
    self.BagCell = {}
    self.CloseBut = prefab.transform:GetChild(0).transform:GetChild(3):GetComponent("Button")
    self.CloseBut.onClick:AddListener(function()
        _G.UImgr:CloseUI(UITypeEnum.bag, false)
    end)
end

function BagView:UpdateBagUI(data)
    self.bagdata = data
    for i = 0, self.bagdata.Count - 1 do
        local BagCell
        BagCell = BagItem.New(i, self.bagdata[i], self.itemContent)
        self.BagCell[i] = BagCell
    end
end

function BagView:RefreshBagItemUI(data)
    self.bagdata = data
   
    for i = 0, self.bagdata.Count - 1 do
     
        if self.bagdata[i].Data ~= nil then
         
            self.BagCell[i]:RefreshBagItem(self.bagdata[i])
        end
    end
end

function BagView:OnEnable()

end

return BagView

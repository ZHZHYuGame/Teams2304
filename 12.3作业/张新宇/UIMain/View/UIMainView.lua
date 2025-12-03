local UIMainView = BaseClass("UIMainView")

function UIMainView:__init(prefab)
    self.gameobj=prefab
    self.Bag_Btn=prefab.transform:Find("Bag_Btn"):GetComponent("Button")
    self.Shop_Btn=prefab.transform:Find("Shop_Btn"):GetComponent("Button")
    self.Bag_Btn.onClick:AddListener(function ()
        _G.UImgr:ShowUI(UITypeEnum.bag)
    end)
    self.Shop_Btn.onClick:AddListener(function ()
        _G.UImgr:ShowUI(UITypeEnum.shop)
    end)
end

function UIMainView:OnEnable()
    self.gameobj.gameObject:SetActive(true)
end

return UIMainView

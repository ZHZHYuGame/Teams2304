local mainView = BaseClass("mainView")

function mainView:__init(prefab)
    self.coinText = prefab.transform:GetChild(2):GetComponent("Text")
    self.shopBut = prefab.transform:GetChild(3).transform:GetChild(0):GetComponent("Button")
    self.shopBut.onClick:AddListener(function()
        _G.UImgr:ShowUI(UITypeEnum.shop)
    end)
    self.bagBut = prefab.transform:GetChild(3).transform:GetChild(1):GetComponent("Button")
    self.bagBut.onClick:AddListener(function()
        _G.UImgr:ShowUI(UITypeEnum.bag)
    end)
end
function mainView:Refresh(coin)
    self.coinText.text = coin
end
function mainView:OnEnable()

end

return mainView

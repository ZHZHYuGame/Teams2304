local shopView = BaseClass("shopView")
ShopItem = require("UI/shop/View/ShopItem")

function shopView:__init(prefab)
    
    UIMessageControll:AddListener(UIID.ShowShopUI, Bind(self,self.RefreshShow) )
    self.prefab = prefab
    self.prefab.gameObject:SetActive(false)
    -- 商品列表容器
    self.goodsListRoot = prefab.transform:Find("Scroll View/Viewport/Content", typeof(GameObject))

    self.CloseBut = prefab.transform:GetChild(3):GetComponent("Button")
    self.CloseBut.onClick:AddListener(function()
        _G.UImgr:CloseUI(UITypeEnum.shop, false)
    end)


    self.ShopItems = {}
    self.showNum = 0
    self.ShowData = nil 
    self.showIcon = prefab.transform:GetChild(1).transform:GetChild(0).transform:GetChild(0):GetComponent("Image")
    self.showName = prefab.transform:GetChild(1).transform:GetChild(1):GetComponent("Text")
    self.showDes = prefab.transform:GetChild(1).transform:GetChild(2).transform:GetChild(0):GetComponent("Text")
    self.showInputText = prefab.transform:GetChild(1).transform:GetChild(3):GetComponent("InputField")
    self.showAddBut = prefab.transform:GetChild(1).transform:GetChild(4):GetComponent("Button")
    self.showReduceBut = prefab.transform:GetChild(1).transform:GetChild(5):GetComponent("Button")
    self.showBuyBut = prefab.transform:GetChild(1).transform:GetChild(6):GetComponent("Button")
    self.showPriceText = prefab.transform:GetChild(1).transform:GetChild(6).transform:GetChild(1):GetComponent("Text")
    
end
-- 刷新商城UI
function shopView:RefreshShopUI(goodsList)

    
    -- -- 清空旧Item
    -- for i = self.goodsListRoot.childCount, 1, -1 do
    --     GameObject.Destroy(self.goodsListRoot:GetChild(i - 1).gameObject)
    -- end
    for i = 0, goodsList.Count - 1, 1 do
        local item = ShopItem.New(self.goodsListRoot, goodsList[i])
        table.insert(self.ShopItems, item)
    end
    self:FirstRefreshShow(goodsList[0])
    
end
function shopView:RefreshShow(data)
    print(data[1])
    if data[1] == nil then
        return
    end

    self.ShowData = data[1]
    self.showIcon.sprite = ABManager.GetInstance():LoadAsset_Sprite(self.ShowData.Icon)
    self.showName.text = self.ShowData.Name
    self.showDes.text = self.ShowData.Des
    self.showInputText.text = self.showNum
    self.showPriceText.text = self.ShowData.Sale
end
function shopView:FirstRefreshShow(data)
    print(data)
    if data == nil then
        return
    end

    self.ShowData = data
    self.showIcon.sprite = ABManager.GetInstance():LoadAsset_Sprite(self.ShowData.Icon)
    self.showName.text = self.ShowData.Name
    self.showDes.text = self.ShowData.Des
    self.showInputText.text = self.showNum
    self.showPriceText.text = self.ShowData.Sale
end
function shopView:OnEnable()

end

return shopView

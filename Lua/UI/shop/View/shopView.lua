local shopView = BaseClass("shopView")
ShopItem = require("UI/shop/View/ShopItem")

function shopView:__init(prefab)
    UIMessageControll:AddListener(UIID.ShowShopUI, Bind(self, self.RefreshShow))
    self.prefab = prefab
    self.prefab.gameObject:SetActive(false)
    -- 商品列表容器
    self.goodsListRoot = prefab.transform:Find("Scroll View/Viewport/Content", typeof(GameObject))

    self.CloseBut = prefab.transform:GetChild(3):GetComponent("Button")
    self.CloseBut.onClick:AddListener(function()
        _G.UImgr:CloseUI(UITypeEnum.shop, false)
    end)
    self.ShopItems = {}
    self.showNum = 1
    self.ShowData = nil
    self.showIcon = prefab.transform:GetChild(1).transform:GetChild(0).transform:GetChild(0):GetComponent("Image")
    self.showName = prefab.transform:GetChild(1).transform:GetChild(1):GetComponent("Text")
    self.showDes = prefab.transform:GetChild(1).transform:GetChild(2).transform:GetChild(0):GetComponent("Text")
    self.showInputText = prefab.transform:GetChild(1).transform:GetChild(3):GetComponent("InputField")
    self.showAddBut = prefab.transform:GetChild(1).transform:GetChild(4):GetComponent("Button")
    self.showReduceBut = prefab.transform:GetChild(1).transform:GetChild(5):GetComponent("Button")
    self.showBuyBut = prefab.transform:GetChild(1).transform:GetChild(6):GetComponent("Button")
    self.showPriceText = prefab.transform:GetChild(1).transform:GetChild(6).transform:GetChild(1):GetComponent("Text")

    self.showInputText.onValueChanged:AddListener(function(text)
        local inputNum = tonumber(text)
        if not inputNum then
            self.showInputText.text = "1"
            return
        end
        if inputNum > 9999 then
            self.showInputText.text = "9999"
        end
    end)
    self.showAddBut.onClick:AddListener(function()
        self.showNum = self.showNum + 1
        self.showInputText.text = self.showNum
    end)
    self.showReduceBut.onClick:AddListener(function()
        self.showNum = self.showNum - 1
        self.showInputText.text = self.showNum
        if self.showNum <= 1 then
            self.showNum = 1
            self.showInputText.text = self.showNum
        elseif self.showNum >= 9999 then
            self.showNum = 9999
        end
    end)
    self.showBuyBut.onClick:AddListener(function()
        local BuyGoodsData = MyGame.C_To_S_BuyGood()
        BuyGoodsData.Id = self.ShowData.Id
        self.showNum = tonumber(self.showInputText.text)
        BuyGoodsData.Num = self.showNum
        CS.NetManager.GetInstance():SendMessage(CS.NetID.C_To_S_BuyGood, Protobuf.ToByteArray(BuyGoodsData))
        self.showNum = 1
        self.showInputText.text = self.showNum
    end)


    self.All = prefab.transform:GetChild(2).transform:GetChild(0):GetComponent("Toggle")
    self.Equipment = prefab.transform:GetChild(2).transform:GetChild(1):GetComponent("Toggle")
    self.Medicine = prefab.transform:GetChild(2).transform:GetChild(2):GetComponent("Toggle")
    self.other = prefab.transform:GetChild(2).transform:GetChild(3):GetComponent("Toggle")
    self.All.onValueChanged:AddListener(function(isOn)
        if isOn then
            for key, item in pairs(self.ShopItems) do
                item:ShowUI()
            end
        end
    end)
    self.Equipment.onValueChanged:AddListener(function(isOn)
        if isOn then
            for key, item in pairs(self.ShopItems) do
                if item.Data.InventoryType == "装备" then
                    item:ShowUI()
                else
                    item:CloseUI()
                end
            end
        end
    end)
    self.Medicine.onValueChanged:AddListener(function(isOn)
        if isOn then
            for key, item in pairs(self.ShopItems) do
                if item.Data.InventoryType == "药品" then
                    item:ShowUI()
                else
                    item:CloseUI()
                end
            end
        end
    end)
    self.other.onValueChanged:AddListener(function(isOn)
        if isOn then
            for key, item in pairs(self.ShopItems) do
                if item.Data.InventoryType == "宝箱" or item.Data.InventoryType == "经验书" then
                    item:ShowUI()
                else
                    item:CloseUI()
                end
            end
        end
    end)
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

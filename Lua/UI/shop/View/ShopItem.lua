ShopItem = BaseClass("ShopItem")
function ShopItem:__init(goodsListRoot, Data)
    self.Data = Data
    self.goodsListRoot = goodsListRoot
    -- 商品列表容器
   
    self.itemObj = GameObject.Instantiate(ABManager.GetInstance():LoadAsset_GameObject(string.lower("ShopItem")), goodsListRoot)
 
    local itemTrans = self.itemObj.transform
    --赋值UI
    itemTrans:Find("Icon"):GetComponent("Image").sprite = ABManager.GetInstance():LoadAsset_Sprite(self.Data.Icon)
    self.Tool = CS.Tool.AddTool(self.itemObj)
    self.Tool.Action_OnPointerDown = function(eventData)
        print(1111)
        UIMessageControll:Dispatch(UIID.ShowShopUI, self.Data)
    end
end
function ShopItem:ShowUI()
    self.itemObj.gameObject:SetActive(true)
end
function ShopItem:CloseUI()
    self.itemObj.gameObject:SetActive(false)
end
return ShopItem
local shopItem = BaseClass("ShopItem")

function shopItem:__init(data, parent)
    self.item = GameObject.Instantiate(Resources.Load("ShopItem", typeof(CS.UnityEngine.GameObject)), parent.transform)
    self.data = data
    self.Icon = self.item.transform:Find("Icon"):GetComponent("Image")
    self.Icon.sprite = Resources.Load("icon/" .. self.data.Icon, typeof(Sprite))
    self.Price = self.item.transform:Find("Price"):GetComponent("Text")
    self.Price.text = self.data.Sale
    self.BuyBtn = self.item.transform:Find("BuyBtn"):GetComponent("Button")
    self.BuyBtn.onClick:AddListener(function()
        NetCSManager.Instance():SendMessage(NetID.C_To_S_BuyGoods_Msg)
    end)
end
return shopItem

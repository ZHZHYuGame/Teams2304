local shopItem = BaseClass("ShopItem")

function shopItem:__init(shopData, trans)
    self.gameObject = GameObject.Instantiate(Resources.Load("shopItem"),trans)
    --查找预制件
    self.name_txt = self.gameObject.transform:Find("name"):GetComponent(typeof(Text))
    self.limit_txt = self.gameObject.transform:Find("num"):GetComponent(typeof(Text))
    self.price_txt = self.gameObject.transform:Find("price"):GetComponent(typeof(Text))
    self.icon_img = self.gameObject.transform:Find("Icon"):GetComponent(typeof(Image))

    self:RefreshItem(shopData)
end


function shopItem:RefreshItem(shopData)
    self.name_txt.text = shopData.config.name
    self.icon_img.sprite = Resources.Load("icon/" .. shopData.config.icon,typeof(Sprite))
    self.limit_txt.text = shopData.limit
    self.price_txt.text = shopData.config.sale
end






return shopItem
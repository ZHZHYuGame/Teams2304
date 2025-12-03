local shopItem = BaseClass("ShopItem")

function shopItem:__init(shopData, trans)
    self.gameObject = GameObject.Instantiate(Resources.Load("shopItem"),trans)
    --查找预制件
    self.name_txt = self.gameObject.transform:Find("name"):GetComponent(typeof(Text))
    self.limit_txt = self.gameObject.transform:Find("limitRoot/limitText"):GetComponent(typeof(Text))
    self.price_txt = self.gameObject.transform:Find("price/price"):GetComponent(typeof(Text))
    self.icon_img = self.gameObject.transform:Find("uiItem/icon"):GetComponent(typeof(Image))
    self.click_buy = self.gameObject.transform:Find("uiItem").gameObject
    self.click_Tool = CS.OnPointClickTool.GetTool(self.click_buy)

    self.click_Tool.act_click = function()
        --发送购买消息
        local c_msg = MyGame.C_To_S_BuyShop_Msg()
        c_msg.Id = shopData.config.id
        c_msg.Price = shopData.config.sale
        c_msg.Count = 1
        NetManager.GetInstance():SendMessage(NetID.C_To_S_BuyShop_msg,Protobuf.ToByteArray(c_msg))
    end

    self:RefreshData(shopData)
end


function shopItem:RefreshData(shopData)
    self.name_txt.text = shopData.config.name
    self.icon_img.sprite = Resources.Load("icon/" .. shopData.config.icon,typeof(Sprite))
    self.limit_txt.text = shopData.limit
    self.price_txt.text = shopData.config.sale
end






return shopItem
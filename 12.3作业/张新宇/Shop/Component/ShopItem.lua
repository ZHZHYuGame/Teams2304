local ShopItem=BaseClass("ShopItem")
function ShopItem:__init(shopdata,pran)
    self.data=shopdata
    if shopdata~=nil then
        self.item=GameObject.Instantiate(Resources.Load("ShopItem"),pran);
        self.name=self.item.transform:Find("Name"):GetComponent("Text")
        self.Sale=self.item.transform:Find("Sale"):GetComponent("Text")
        self.icon=self.item.transform:Find("Icon"):GetComponent("Image")
        self.Buy=self.item.transform:Find("Buy_Btn"):GetComponent("Button")
        
    end
    self:SetData(shopdata)
end
function ShopItem:SetData(shopdata)
    if shopdata==nil then
        return
    end
    print(self.name)
    self.name.text=shopdata.name
    self.Sale.text=shopdata.price
    self.icon.sprite=Resources.Load("icon/"..shopdata.icon,typeof(Sprite))
end

return ShopItem
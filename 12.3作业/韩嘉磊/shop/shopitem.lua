local shopitem = BaseClass("shopitem")

function shopitem:__init(data, tran)
    self.data = data
    self.item = GameObject.Instantiate(Resources.Load("item", typeof(GameObject)), tran)
    self.icon = self.item.transform:Find("icon"):GetComponent("Image")
    self.name = self.item.transform:Find("name"):GetComponent("Text")
    self.coin = self.item.transform:Find("coin"):GetComponent("Text")

    if self.data then
        self.icon.sprite = Resources.Load("icon/" .. self.data.Icon, typeof(Sprite))
        self.name.text = self.data.Name
        self.coin.text = self.data.Sale
    end
end

return shopitem

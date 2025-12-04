local bagitem = BaseClass("BagItem")

function bagitem:__init(data, parent)
    self.data = data
    self.item = GameObject.Instantiate(Resources.Load("BagItem", typeof(CS.UnityEngine.GameObject)), parent.transform)
    self.Icon = self.item.transform:Find("Icon"):GetComponent("Image")
    self.Num = self.item.transform:Find("Num"):GetComponent("Text")
end

function bagitem:RefreshView(data)
    self.data = data
    if self.data ~= nil then
        self.Icon.sprite = Resources.Load("icon/" .. data.Icon, typeof(Sprite))
        self.Num.text = "1"
    end
end

return bagitem

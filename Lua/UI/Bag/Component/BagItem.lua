local bagItem = BaseClass("BagItem")
function bagItem:__init(index, data, parent)
    self.index = index
    self.data = data
    self.num = 0
    self.bagitem = GameObject.Instantiate(ABManager.GetInstance():LoadAsset_GameObject(string.lower("BagItem")),
        parent.transform)
    self.icon = self.bagitem.transform:GetChild(0):GetComponent("Image")
    self.numText = self.bagitem.transform:GetChild(1):GetComponent("Text")
    if data ~= nil then
        self.icon.gameObject:SetActive(true)
        self.numText.gameObject:SetActive(true)
        if self.data.Icon == nil then
            self.icon.gameObject:SetActive(false)
            self.numText.gameObject:SetActive(false)
        else
            self.icon.sprite = ABManager.GetInstance():LoadAsset_Sprite(self.data.Icon)
        end
        self.numText.text = self.data.Num
    else
        self.icon.gameObject:SetActive(false)
        self.numText.gameObject:SetActive(false)
    end
end

return bagItem

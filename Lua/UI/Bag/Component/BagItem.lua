local bagItem = BaseClass("BagItem")
function bagItem:__init(index, data, parent)
    self.index = index
    self.data = data.Data
    self.num = 0
    self.bagitem = GameObject.Instantiate(ABManager.GetInstance():LoadAsset_GameObject(string.lower("BagItem")),
        parent.transform)
    self.icon = self.bagitem.transform:GetChild(0):GetComponent("Image")
    self.numText = self.bagitem.transform:GetChild(1):GetComponent("Text")
    if self.data ~= nil then
        self.icon.gameObject:SetActive(true)
        self.numText.gameObject:SetActive(true)
        self.icon.sprite = ABManager.GetInstance():LoadAsset_Sprite(self.data.Icon)
        self.numText.text = data.Num
    else
        self.icon.gameObject:SetActive(false)
        self.numText.gameObject:SetActive(false)
    end

    self.Tool = CS.Tool.AddTool(self.bagitem)
    self.Tool.Action_OnPointerDown = function(eventData)
        UIMessageControll:Dispatch(UIID.ShowBagUI, self.data)
    end
end


function bagItem:RefreshBagItem(data)
    if data.Data ~= nil then
        self.data = data.Data
        self.icon.gameObject:SetActive(true)
        self.numText.gameObject:SetActive(true)
        self.icon.sprite = ABManager.GetInstance():LoadAsset_Sprite(self.data.Icon)
        self.numText.text = data.Num
    else
        self.icon.gameObject:SetActive(false)
        self.numText.gameObject:SetActive(false)
    end
end
function bagItem:ShowUI(data)
    self.bagitem.gameObject:SetActive(true)
end
function bagItem:CloseUI()
    self.bagitem.gameObject:SetActive(false)
end
return bagItem

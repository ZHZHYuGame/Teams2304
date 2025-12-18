local bagitem = BaseClass("BagItem")

function bagitem:__init(parent, id)
    self.id = id
    self.gameObject = GameObject.Instantiate(_G.ResMgr:LoadAsset("bagitem"), parent)
    --查找组件
    self.Icon = self.gameObject.transform:Find("icon"):GetComponent("Image")
    self.CountText = self.gameObject.transform:Find("Count"):GetComponent("Text")

    self.pointEvent = CS.UIEventListener.AddEvent_To_Obj(self.gameObject)
    self.pointEvent:AddListener(EventTriggerType.PointerClick, function(eventData)
        UIMessageControll:Dispatch(ClientID.Tips_BagData, self.id)
    end)
end

function bagitem:Refresh(bagdata)
    self.gameObject:SetActive(true)
    self.Icon.sprite = _G.ResMgr:LoadAtlasAsset(bagdata.config.Path, bagdata.config.IconName)
    self.CountText.text = bagdata.UseCount
end

return bagitem

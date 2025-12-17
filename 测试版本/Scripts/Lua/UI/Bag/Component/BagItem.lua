local bagitem = BaseClass("BagItem")

function bagitem:__init(parent)
    self.gameObject = GameObject.Instantiate(Resources.Load("BagItem", typeof(GameObject)), parent)
    --查找组件
    self.Icon = self.gameObject.transform:Find("icon"):GetComponent("Image")
    self.CountText = self.gameObject.transform:Find("Count"):GetComponent("Text")
    self.ItemScript = CS.UIEventListener.AddEvent_To_Obj(self.gameObject)
    self.ItemScript.onclick = function(eventData)
        UIMessageControll:Dispatch(ClientID.Tips_BagData, self.data)
    end
end

function bagitem:Refresh(bagdata)
    self.data = bagdata
    self.gameObject:SetActive(true)
    self.Icon.sprite = _G.ResMgr:LoadAtlasAsset(bagdata.config.Path, bagdata.config.IconName)
    self.CountText.text = bagdata.UseCount
end

return bagitem

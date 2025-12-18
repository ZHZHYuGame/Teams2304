local BagView = BaseClass("BagView")
local BagItem = require("UI/Bag/Component/BagItem")
function BagView:__init(prefab)
    UIMessageControll:AddListener(UIID.ShowBagUI, Bind(self, self.RefreshShow))
    self.prefab = prefab
    self.prefab.gameObject:SetActive(false)
    self.itemContent = prefab.transform:Find("BagPanel/Scroll View/Viewport/Content")
    self.BagCell = {}
    self.CloseBut = prefab.transform:GetChild(0).transform:GetChild(3):GetComponent("Button")
    self.CloseBut.onClick:AddListener(function()
        _G.UImgr:CloseUI(UITypeEnum.bag, false)
    end)
    self.ShowData = nil
    self.showIcon = prefab.transform:GetChild(0).transform:GetChild(2).transform:GetChild(0):GetComponent("Image")
    self.showName = prefab.transform:GetChild(0).transform:GetChild(2).transform:GetChild(1):GetComponent("Text")
    self.showDes = prefab.transform:GetChild(0).transform:GetChild(2).transform:GetChild(2).transform:GetChild(0)
    :GetComponent("Text")


    self.All = prefab.transform:GetChild(0).transform:GetChild(1).transform:GetChild(0):GetComponent("Toggle")
    self.Equipment = prefab.transform:GetChild(0).transform:GetChild(1).transform:GetChild(1):GetComponent("Toggle")
    self.Medicine = prefab.transform:GetChild(0).transform:GetChild(1).transform:GetChild(2):GetComponent("Toggle")
    self.other = prefab.transform:GetChild(0).transform:GetChild(1).transform:GetChild(3):GetComponent("Toggle")
    self.All.onValueChanged:AddListener(function(isOn)
        if isOn then
            for key, value in pairs(self.BagCell) do
               
                value:ShowUI(self.bagdata[key])

            end
        end
    end)
    self.Equipment.onValueChanged:AddListener(function(isOn)
        if isOn then
            for key, value in pairs(self.BagCell) do
                if value.data ~= nil then
                    if value.data.InventoryType == "装备" then
                        value:ShowUI(self.bagdata[key])
                    else
                        value:CloseUI()
                    end
                else
                    value:CloseUI()
                end
            end
        end
    end)
    self.Medicine.onValueChanged:AddListener(function(isOn)
        if isOn then
            for key, value in pairs(self.BagCell) do
                if value.data ~= nil then
                    if value.data.InventoryType == "药品" then
                        value:ShowUI(self.bagdata[key])
                    else
                        value:CloseUI()
                    end
                else
                    value:CloseUI()
                end
            end
        end
    end)
    self.other.onValueChanged:AddListener(function(isOn)
        if isOn then
            for key, value in pairs(self.BagCell) do
                if value.data ~= nil then
                    if value.data.InventoryType == "宝箱" or value.data.InventoryType == "经验书" then
                        value:ShowUI(self.bagdata[key])
                    else
                        value:CloseUI()
                    end
                else
                    value:CloseUI()
                end
            end
        end
    end)
end

function BagView:UpdateBagUI(data)
    self.bagdata = data
    for i = 0, self.bagdata.Count - 1 do
        local BagCell
        BagCell = BagItem.New(i, self.bagdata[i], self.itemContent)
        self.BagCell[i] = BagCell
    end
    self:FirstRefreshShow(self.bagdata[0])
end

function BagView:RefreshBagItemUI(data)
    self.bagdata = data

    for i = 0, self.bagdata.Count - 1 do
        if self.bagdata[i].Data ~= nil then
            self.BagCell[i]:RefreshBagItem(self.bagdata[i])
            self:FirstRefreshShow(self.bagdata[0])
        end
    end
end

function BagView:RefreshShow(data)
    if data[1] == nil then
        return
    end
    self.ShowData = data[1]
    self.showIcon.sprite = ABManager.GetInstance():LoadAsset_Sprite(self.ShowData.Icon)
    self.showName.text = self.ShowData.Name
    self.showDes.text = self.ShowData.Des
end

function BagView:FirstRefreshShow(data)
    if data.Data == nil then
        return
    end
    self.ShowData = data.Data
    self.showIcon.sprite = ABManager.GetInstance():LoadAsset_Sprite(self.ShowData.Icon)
    self.showName.text = self.ShowData.Name
    self.showDes.text = self.ShowData.Des
end

function BagView:OnEnable()

end

return BagView

-- 背包视图类（继承自自定义基类BaseClass）
local BagView = BaseClass("BagView")
-- 构造函数（初始化）
function BagView:__init(prefabObj)
    -- 1. 基础引用初始化（确保传入的是GameObject）
    self.gameObject = prefabObj
    self.transform = self.gameObject.transform
    self.BagItemParent = self.gameObject.transform:Find("BagItemPanel/Scroll View/Viewport/Content").transform
    self.BagItem = require("UI/Bag/Component/BagItem")
    -- 2.预制件item表
    self.BagItemList = {}
    self.StartNum = 1
    self.NowBagData = nil
    self:InitUIData()
    self:BindUIEvent()
end

function BagView:InitUIData()
    --查找ui组件
    self.closeBtn = self.transform:Find("CloseBtn"):GetComponent(typeof(Button))
    self.resTog = self.transform:Find("TypePanel/ResourceToggle"):GetComponent(typeof(Toggle))
    self.resTog.isOn = true
    self.boostTog = self.transform:Find("TypePanel/BoostToggle"):GetComponent(typeof(Toggle))
    self.buffTog = self.transform:Find("TypePanel/BuffToggle"):GetComponent(typeof(Toggle))
    self.outfitTog = self.transform:Find("TypePanel/OutfitToggle"):GetComponent(typeof(Toggle))
    self.otherTog = self.transform:Find("TypePanel/OtherToggle"):GetComponent(typeof(Toggle))
    self.DataIcon = self.transform:Find("TipsPanel/DataPanel/DataIcon"):GetComponent(typeof(Image))
    self.DataName = self.transform:Find("TipsPanel/DataName"):GetComponent(typeof(Text))
    self.DesText = self.transform:Find("TipsPanel/DesPanel/Text"):GetComponent(typeof(Text))
    self.Jian = self.transform:Find("TipsPanel/CountPanel/Jian"):GetComponent(typeof(Button))
    self.Add = self.transform:Find("TipsPanel/CountPanel/Add"):GetComponent(typeof(Button))
    self.NumText = self.transform:Find("TipsPanel/CountPanel/Text"):GetComponent(typeof(Text))
    self.MaxBtn = self.transform:Find("TipsPanel/CountPanel/MaxBtn"):GetComponent(typeof(Button))
    self.HasNum = self.transform:Find("TipsPanel/HasNum"):GetComponent(typeof(Text))
    self.UseBtn = self.transform:Find("TipsPanel/UseBtn"):GetComponent(typeof(Button))
    self.CountPanel = self.transform:Find("TipsPanel/CountPanel").gameObject
end

function BagView:TipsInitData(bagData)

    self.DataIcon.sprite = _G.ResMgr:LoadAtlasAsset(bagData.config.Path, bagData.config.IconName)
    self.DataName.text = bagData.config.Name
    self.DesText.text = bagData.config.Des

    if bagData.config.Type == "资源" or bagData.config.Type == "增益" then
        self.UseBtn.gameObject:SetActive(true)
        self.CountPanel.gameObject:SetActive(true)
    else
        self.UseBtn.gameObject:SetActive(false)
        self.CountPanel.gameObject:SetActive(false)
    end
    self.HasNum.text = "拥有数量:" .. bagData.count
    self.NumText.text = "1"
end

function BagView:BindUIEvent()
    self.closeBtn.onClick:AddListener(function()
        _G.UImgr:CloseUI(UITypeEnum.bag)
    end)
end

function BagView:InitData(bagDataDict)
    local itemCount = #self.BagItemList
    local dataCount = _G.tool:getTableTotalLength(bagDataDict)
    if dataCount > itemCount then
        for i = 1, dataCount - itemCount, 1 do
            local item = self.BagItem.New(self.BagItemParent, itemCount + i) 
            table.insert(self.BagItemList, item)
        end
    else
        if dataCount < itemCount then
            for i = dataCount + 1, itemCount, 1 do
                self.BagItemList[i].gameObject:SetActive(false)
            end
        end
    end

    for key, value in ipairs(bagDataDict) do
        self.BagItemList[key]:Refresh(value)
    end
end

-- 视图激活（补充逻辑）
function BagView:OnEnable()

end

-- 视图隐藏
function BagView:OnDisable()

end

-- 销毁/清理（核心：移除事件监听，避免内存泄漏）
function BagView:Dispose()

end

return BagView

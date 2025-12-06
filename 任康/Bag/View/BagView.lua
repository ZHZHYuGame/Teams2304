local bagView = BaseClass("BagView")

function bagView:__init(uiPre)
    self.gameObject = uiPre

    --查找预制件
    self.bagContent = self.gameObject.transform:Find("BG/Scroll View/Viewport/Content")
    self.close_btn = self.gameObject.transform:Find("BG/close"):GetComponent(typeof(Button))

    self.close_btn.onClick:AddListener(function()
        _G.UImgr:CloseUI(UITypeEnum.bag)
    end)
    self.bagItem = require("UI/Bag/Component/BagItem")
    self.bagItemTable = {}

    self.total_btn = self.gameObject.transform:Find("BG/btnGroup/total_btn"):GetComponent(typeof(Button))
    self.equip_btn = self.gameObject.transform:Find("BG/btnGroup/equip_btn"):GetComponent(typeof(Button))
    self.box_btn = self.gameObject.transform:Find("BG/btnGroup/box_btn"):GetComponent(typeof(Button))
    self.medicine_btn = self.gameObject.transform:Find("BG/btnGroup/medicine_btn"):GetComponent(typeof(Button))

    --查找红点预制件
    self.total_btn_redPoint = self.total_btn.transform:Find("redPoint").gameObject
    self.equip_btn_redPoint = self.equip_btn.transform:Find("redPoint").gameObject
    self.box_btn_redPoint = self.box_btn.transform:Find("redPoint").gameObject
    self.medicine_btn_redPoint = self.medicine_btn.transform:Find("redPoint").gameObject

    -- 注册红点数据
    self:RegisterRedPointData()
end

-- 注册红点数据
function bagView:RegisterRedPointData()
    
    _G.redPointManager:Bind_UI_To_RedPoint
    (UIRedPointType.UI_BagIcon, UIRedPointType.UI_Bag_AllBtnIcon,
        function(isShow)
            self.total_btn_redPoint:SetActive(isShow)
        end
    )

    _G.redPointManager:Bind_UI_To_RedPoint
    (UIRedPointType.UI_BagIcon, UIRedPointType.UI_Bag_EquipIcon,
        function(isShow)
            self.equip_btn_redPoint:SetActive(isShow)
        end
    )
    _G.redPointManager:Bind_UI_To_RedPoint
    (UIRedPointType.UI_BagIcon, UIRedPointType.UI_Bag_BoxIcon,
        function(isShow)
            self.box_btn_redPoint:SetActive(isShow)
        end
    )
    _G.redPointManager:Bind_UI_To_RedPoint
    (UIRedPointType.UI_BagIcon, UIRedPointType.UI_Bag_MedicineIcon,
        function(isShow)
            self.medicine_btn_redPoint:SetActive(isShow)
        end
    )
end

function bagView:OnInitView(table)
    for index, value in ipairs(table) do
        local bagItem = self.bagItem.New(value, self.bagContent)
        self.bagItemTable[index] = bagItem

    end
end

function bagView:RefreshItem(bagData)
    --查找商品
    if self.bagItemTable[bagData.id] ~= nil then
        self.bagItemTable[bagData.id]:RefreshData(bagData)
    end
end

return bagView
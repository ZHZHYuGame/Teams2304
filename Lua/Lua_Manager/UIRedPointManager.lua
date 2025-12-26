local redPointManager = BaseClass("RedPointManager")

--初始化红点规则
function redPointManager:__init()
    self.redPointIconDict = {}
    self.redPoint_Bind_UI_Dict = {}
    --主控与背包Icon的关系
    self:Bind_UI_Icon_RedPoints(UIRedPointType.UI_MainIcon, UIRedPointType.UI_BagIcon)
    --背包与面板面签按钮Icon的关系
    self:Bind_UI_Icon_RedPoints(UIRedPointType.UI_BagIcon, UIRedPointType.UI_Bag_AllIcon)
    self:Bind_UI_Icon_RedPoints(UIRedPointType.UI_BagIcon, UIRedPointType.UI_Bag_EquipIcon)
    self:Bind_UI_Icon_RedPoints(UIRedPointType.UI_BagIcon, UIRedPointType.UI_Bag_YaoIcon)
    self:Bind_UI_Icon_RedPoints(UIRedPointType.UI_BagIcon, UIRedPointType.UI_Bag_BaoIcon)
end

--绑定UI Icon之间的红点关系--注册
--parentType 父节点
--childrenType 子节点
function redPointManager:Bind_UI_Icon_RedPoints(parentType, childrenType)
    if self.redPointIconDict[parentType] == nil then
        --创建父节点
        local parentNode = require("Data/RedPointNode").New(parentType, false)
        self.redPointIconDict[parentType] = parentNode
    end

    if self.redPointIconDict[childrenType] == nil then
        --创建子节点
        local childreNode = require("Data/RedPointNode").New(childrenType, false)
        self.redPointIconDict[childrenType] = childreNode
    end
    --前缀关系的创建
    if self.redPointIconDict[parentType].childrenRedPointList[childrenType] == nil then
        --父节点的子节点列表添加
        self.redPointIconDict[parentType].childrenRedPointList[childrenType] = self.redPointIconDict[childrenType]
    end

    if self.redPointIconDict[childrenType].parentRedPointList[parentType] == nil then
        --子节点的父节点列表添加
        self.redPointIconDict[childrenType].parentRedPointList[parentType] = self.redPointIconDict[parentType]
    end
end

--刷新UI的红点状态（单个红点的刷新）--触发
function redPointManager:Update_UI_RedPoint_State(nodeType, redPointData)
    local redpoint = self.redPoint_Bind_UI_Dict[nodeType]
    if redpoint ~= nil then
        self.redPoint_Bind_UI_Dict[nodeType](nodeType, redPointData.redState)
    end
    --刷新子级节点的当前显示状态
    self:Update_UI_Children_RedPoint_State(nodeType, redPointData)
    --刷新父级节点的当前显示状态
    self:Update_UI_Parent_RedPoint_State(nodeType)
end

--刷新父级节点的当前显示状态
--nodeType 节点的枚举类型
function redPointManager:Update_UI_Parent_RedPoint_State(nodeType)
    --找到具体的UI的红点节点（最深度的那个节点）
    local uiRedPointNode = self.redPointIconDict[nodeType]
    print(#uiRedPointNode.parentRedPointList)
    for key, value in pairs(uiRedPointNode.parentRedPointList) do
        --父级节点
        local parentUIRenPointNode = value
        --显示状态
        if uiRedPointNode.redState == true then
            if uiRedPointNode.redState == parentUIRenPointNode.redState then
                --停止逻辑
            else
                parentUIRenPointNode.redState = true
                -- self.redPoint_Bind_UI_Dict[parentUIRenPointNode.redType](parentUIRenPointNode.redType,parentUIRenPointNode.redState)
                --通知父级状态变化
                self:Update_UI_RedPoint_State(parentUIRenPointNode.redType, parentUIRenPointNode)
            end
        else
            --隐藏状态
            if uiRedPointNode.redState == parentUIRenPointNode.redState then
                --停止逻辑
            else
                parentUIRenPointNode.redState = false
                -- self.redPoint_Bind_UI_Dict[nodeType](nodeType,parentUIRenPointNode.redState)
                --通知父级状态变化
                self:Update_UI_RedPoint_State(parentUIRenPointNode.redType, parentUIRenPointNode)
            end
        end
        --当前节点与父节点状态不一样的情况下，通知父节点发生变化，并刷新父节点
        -- if uiRedPointNode.redState ~= parentUIRenPointNode.redState then
        --    parentUIRenPointNode.redState = uiRedPointNode.redState
        --    self:Update_UI_RedPoint_State(parentUIRenPointNode.redType)
        -- end
    end
end

--刷新子级节点的当前显示状态
--nodeType 节点的枚举类型
function redPointManager:Update_UI_Children_RedPoint_State(nodeType, redPointData)
    --找到具体的UI的红点节点（最深度的那个节点）
    local uiRedPointNode = self.redPointIconDict[nodeType]
    --当前节点设置服务器最新的红点状态
    uiRedPointNode.redState = redPointData.redState
    --判断子级是否状态变化（看当前节点是否发生状态变化）
    for key, value in pairs(uiRedPointNode.childrenRedPointList) do
        local childrenUIRenPointNode = value
        --当前节点的子节点有一个为true，并且与当前状态一致主的话，终止逻辑，在运行没有意义
        if uiRedPointNode.redState == childrenUIRenPointNode.redState then
            --没有状态变化
            -- self.redPoint_Bind_UI_Dict[nodeType](nodeType,uiRedPointNode.redState)
            return
        end
        --当前节点为false,如果有一个子节点为true的话，当前节点状态就发生变化，改状态，终止逻辑
        if uiRedPointNode.redState ~= childrenUIRenPointNode.redState then
            if childrenUIRenPointNode.redState == true then
                uiRedPointNode.redState = true
                --通知UI进行红点状态刷新
                -- self.redPoint_Bind_UI_Dict[nodeType](redPointData)
                self.redPoint_Bind_UI_Dict[nodeType](nodeType, uiRedPointNode.redState)
                return
            end
        end
    end
    --当前节点的状态因为子节点的变化发生改变
    if #uiRedPointNode.childrenRedPointList > 0 then
        uiRedPointNode.redState = false
    end
    --通知UI进行红点状态刷新
    -- self.redPoint_Bind_UI_Dict[nodeType](redPointData)
    self.redPoint_Bind_UI_Dict[nodeType](nodeType, uiRedPointNode.redState)
end

--刷新UI的红点状态（全部红点的刷新，一般是第1点进入游戏的操作）
function redPointManager:Update_UI_RedPoint_All_State()
end

--红点数据与UI绑定处理
function redPointManager:Bind_RedPiontData_To_UI_Handle(type, handle)
    if self.redPoint_Bind_UI_Dict[type] == nil then
        self.redPoint_Bind_UI_Dict[type] = handle
    end
end

function redPointManager:Update_UI_RedPoint(type, bool)
    if self.redPoint_Bind_UI_Dict[type] ~= nil and self.redPointIconDict[type] ~= nil then
        self.redPointIconDict[type].redState = bool
        self:Update_UI_RedPoint_State(type, self.redPointIconDict[type])
    end
end

return redPointManager

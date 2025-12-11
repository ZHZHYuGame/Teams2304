local redPointManager = BaseClass("RedPointManager")


function redPointManager:Init()
    self:AddListener()
    self.redPointIconDIct = {}
    self.redPointActionDict = {}
end

-- 绑定UI图标之间的红点关系
function redPointManager:Bind_UI_Icon_RedPoints(parentType, childrenType)
    if self.redPointIconDIct[parentType] == nil then
        --创建父节点
        self.redPointIconDIct[parentType] = require("Data/RedPointNode").New(parentType, false)
    end

    if self.redPointIconDIct[childrenType] == nil then
        --创建子节点
        self.redPointIconDIct[childrenType] = require("Data/RedPointNode").New(childrenType, false)

    end

    if self.redPointIconDIct[parentType].childrenRedPointList[childrenType] == nil then
        --添加子节点
        self.redPointIconDIct[parentType].childrenRedPointList[childrenType] = self.redPointIconDIct[childrenType]
    end

    if self.redPointIconDIct[childrenType].parentRedPointList[parentType] == nil then
        --添加父节点
        self.redPointIconDIct[childrenType].parentRedPointList[parentType] = self.redPointIconDIct[parentType]
    end
end


-- 侦听网络红点刷新消息
function redPointManager:AddListener()
    NetMessageControll:AddListener(NetID.S_To_C_RedPoint_Update_msg,
        Bind(self, self.S_To_C_RedPoint_Update_msg_Handle))
end
function redPointManager:S_To_C_RedPoint_Update_msg_Handle(byteData)

    local s_msg = MyGame.S_To_C_RedPoint_Update_Msg.Parser:ParseFrom(byteData);
    for i = 0, s_msg.RedPointDataList.Count - 1, 1 do
        local redPointData = s_msg.RedPointDataList[i]
        UIMessageControll:Dispatch(redPointData.Type, redPointData)
    end
end

-- 刷新红点
function redPointManager:Update_RedPoint(nodeType, redPointData)
    
    --找到具体的节点
    local redPointNode = self.redPointIconDIct[nodeType]
    redPointNode.nodeState = redPointData.State
    self.redPointActionDict[redPointNode.nodeType](redPointNode.nodeState)

    if #redPointNode.childrenRedPointList > 0 then
        self:Update_Children_RedPoint(nodeType)
    end
    
    self:Update_Parent_RedPoint(nodeType)
end

function redPointManager:Update_Parent_RedPoint(nodeType)
    
    --找到具体的节点  
    local redPointNode = self.redPointIconDIct[nodeType]

    --遍历父节点
    for key, value in pairs(redPointNode.parentRedPointList) do
        -- 当前节点显示，父节点隐藏，则父节点显示，并继续往上遍历父节点
        if redPointNode.nodeState == true then
            if value.nodeState == false then
                value.nodeState = true
                -- 调用红点刷新回调
                if self.redPointActionDict[value.nodeType] ~= nil then
                    self.redPointActionDict[value.nodeType](value.nodeState)
                end
                self:Update_Parent_RedPoint(value.nodeType)
            end
        end

        -- 当前节点隐藏，父节点显示，先判断父节点下的其他子节点是否隐藏
        if redPointNode.nodeState == false then
            if value.nodeState == true then
                
                self:Update_RedPoint(value.nodeType)
            end
        end


    end
end
function redPointManager:Update_Children_RedPoint(nodeType)
    
    --找到具体的节点  
    local redPointNode = self.redPointIconDIct[nodeType]

    --遍历子节点
    for key, value in pairs(redPointNode.childrenRedPointList) do
        -- --当前节点显示时有子节点显示，直接返回，不做处理
        -- if redPointNode.nodeState == true then
        --     if value.nodeState == true then
        --         return  
        --     end
        -- end
        -- --当前节点隐藏时，有子节点显示，则显示父节点，并返回
        -- if redPointNode.nodeState == false then
        --     if value.nodeState == true then

        --         redPointNode.nodeState = true
        --         return
        --     end
        -- encountered
        --简化
        if value.nodeState == true then
            if redPointNode.nodeState == true then
                return
            else
                redPointNode.nodeState = true
                -- 调用红点刷新回调
                self.redPointActionDict[redPointNode.nodeType](redPointNode.nodeState)
                return
            end
        end
    end

    --到这一步说明当前节点没有子节点显示，则该节点隐藏
    redPointNode.nodeState = false
    -- 调用红点刷新回调
    self.redPointActionDict[redPointNode.nodeType](redPointNode.nodeState)

end

-- 绑定UI与红点关系
function redPointManager:Bind_UI_To_RedPoint(parentType, childrenType, handle)
    self:Bind_UI_Icon_RedPoints(parentType, childrenType)
    if self.redPointActionDict[childrenType] == nil and handle ~= nil then
       

        self.redPointActionDict[childrenType] = handle
    end
end


return redPointManager
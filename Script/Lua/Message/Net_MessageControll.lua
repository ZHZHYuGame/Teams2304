NetMessageControll = {}

--注册UI事件
--id 事件ID
--fun 事件回调
function NetMessageControll:AddListener(id, fun)
    if self.m_Dict == nil then
        self.m_Dict = {}
    end
    local tab = {}
    --第一次注册
    if self.m_Dict[id] == nil then
        table.insert(tab, fun)
        self.m_Dict[id] = tab
    else
        --代表多播委托
        tab = self.m_Dict[id]
        table.insert(tab, fun)
        self.m_Dict[id] = tab
    end
end

function NetMessageControll:Remove(id)
end
--派发执行ID对应回调
--id 事件ID
--... 不定参数
function NetMessageControll:Dispatch(id, ...)
    if self.m_Dict[id] ~= nil then
        for index, value in pairs(self.m_Dict[id]) do
            if value ~= nil then
                value(...)
            end
            
        end
    end
end

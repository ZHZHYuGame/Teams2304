UIMessageControll={}

--注册UI事件
--id 事件ID
--fun 时间回调
function UIMessageControll:AddListener(id,fun)
    if self.m_Dict==nil then
        self.m_Dict={}
    end
    local tab={}
    --第一次注册
    if self.m_Dict[id]==nil then
        table.insert(tab,fun)
        self.m_Dict[id]=tab
    else
        --代表多播委托
        tab=self.m_Dict[id]
        table.insert(tab,fun)
        self.m_Dict[id]=tab
    end
end
function UIMessageControll:Remove(id)
    
end
function UIMessageControll:Dispatch(id,...)
    local tab={...}
    if self.m_Dict==nil then
        self.m_Dict={}
    end
    if self.m_Dict[id]~=nil then
        for index, value in ipairs(self.m_Dict[id]) do
            if value~=nil then
                value(tab)
            end
        end
    end
    
end
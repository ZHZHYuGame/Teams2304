TimeTool = {}
TimeTool.TimeInvokeTable = {}
TimeTool.TimeInvokeRepreating = {}

-- 时间工具方法(延时时间执行，例如1秒后执行)，单次
-- delayTime 延时时间
-- func 回调方法
function TimeTool:Invoke(delayTime, func)
    local timeData = {StartTimes=DateTime.Now.Ticks, EndTimes=delayTime, CallBack=func}
    table.insert(self.TimeInvokeTable, timeData) 
end

-- 时间工具方法(延时循环执行)
-- delayTime 延时时间
-- func 回调方法
function TimeTool:InvokeRepeating(delayTime, func)
    local timeData ={StartTimes=DateTime.Now.Ticks, EndTimes=delayTime, CallBack=func}
    table.insert(self.TimeInvokeRepreating, timeData) 
end

-- 停止循环时间函数
-- func 
function TimeTool:StopInvokeRepeating(func)
    for index, value in ipairs(self.TimeInvokeRepreating) do
        if value.CallBack == func then
            table.remove(self.TimeInvokeRepreating, index)
        end
    end
end


function TimeTool:Update()

    for index, value in ipairs(TimeTool.TimeInvokeTable) do
        -- 当前正在运行时间 - 起始触发的时间 除于 单位 纳秒
        local currTimes = (DateTime.Now.Ticks - value.StartTimes)/10000000
        -- 间距时间 与 延时时间 判断 
        if currTimes - value.EndTimes >= 0 then
            value.CallBack()
            table.remove(TimeTool.TimeInvokeTable, index) 
        end
    end

    for index, value in ipairs(self.TimeInvokeRepreating) do
        -- 当前正在运行时间 - 起始触发的时间 除于 单位 纳秒
        local currTimes = (DateTime.Now.Ticks - value.StartTimes)/10000000
        -- 间距时间 与 延时时间 判断 
        if currTimes - value.EndTimes >= 0 then
            value.CallBack()
            -- 执行方法的时候就是下一次执行的开始时间
            value.StartTimes = DateTime.Now.Ticks
        end
    end

end



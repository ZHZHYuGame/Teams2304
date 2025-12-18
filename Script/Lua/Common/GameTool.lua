local GameTool = {}
function GameTool:getTableTotalLength(t)
    local len = 0
    -- pairs 遍历所有键（整数/字符串/布尔等，除了 nil）
    for k, v in pairs(t) do
        len = len + 1
    end
    return len
end


return GameTool
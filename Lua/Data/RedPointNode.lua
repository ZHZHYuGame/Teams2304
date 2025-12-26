local redPointNode = BaseClass("redPointNode")
--红点的节点数据
--redType 红点类型
--redState 红点状态
--parentRedPointList 父级红点节点数
--childrenRedPointList 子级红点节点数
--children_Count 当前结点的状态数量
function redPointNode:__init(...)
    local tab = {...}
    --红点类型
    self.redType = tab[1]
    --红点状态
    self.redState = tab[2]
    --父级红点节点数
    self.parentRedPointList = {}
    --子级红点节点数
    self.childrenRedPointList = {}
    --当前结点红点状态（记录其下属子节点的为ture的数量），可以减少过多状态的逻辑计算，也可以进行当前节点下的红点引导数量的显示
    self.children_Count = 0
end

return redPointNode
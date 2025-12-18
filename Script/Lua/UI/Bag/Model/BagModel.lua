local BagModel = BaseClass("BagModel")

function BagModel:__init()
    self.bagDataDict = {}

    if self.bagDataDict[0] == nil then
        self.bagDataDict[0] = {}
    end
    if self.bagDataDict[1] == nil then
        self.bagDataDict[1] = {}
    end
    if self.bagDataDict[2] == nil then
        self.bagDataDict[2] = {}
    end
    if self.bagDataDict[3] == nil then
        self.bagDataDict[3] = {}
    end
    if self.bagDataDict[4] == nil then
        self.bagDataDict[4] = {}
    end
    self:AddListener()
end

function BagModel:AddListener()
    UIMessageControll:AddListener(ClientID.Init_BagData, Bind(self, self.Init_BagData_Msg))
    
end

function BagModel:Update_BagData_Msg(table)
    local type = table[1]

    
end

function BagModel:Init_BagData_Msg(table)
    local baglist = table[1]
    self:InitData(baglist)
end

function BagModel:InitData(baglist)
    for i = 0, baglist.Count - 1, 1 do
        local data = ConfigManager:GetItemData(baglist[i].ItemId)
        local bagData =
        {
            id = baglist[i].Id,
            itemId = baglist[i].ItemId,
            itemIndex =  baglist[i].ItemIndex,
            config = data,
            count = baglist[i].Count
        }
        if data.Type == "资源" then
            self.bagDataDict[0][bagData.itemIndex] = bagData
        end
        if data.Type == "加速" then
            self.bagDataDict[1][bagData.itemIndex] = bagData
        end
        if data.Type == "增益" then
            self.bagDataDict[2][bagData.itemIndex] = bagData
        end

        if data.Type == "装备" then
            self.bagDataDict[3][bagData.itemIndex] = bagData
        end
        if data.Type == "其他" then
            self.bagDataDict[4][bagData.itemIndex] = bagData
        end
    end
end


function BagModel:RemoveListener()

end

return BagModel

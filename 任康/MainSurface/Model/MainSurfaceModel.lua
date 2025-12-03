local MainSurfaceModel = BaseClass("MainSurfaceModel")

function MainSurfaceModel:__init()

    self.playerDataTable = {}
end
function MainSurfaceModel:OnInitData(table)
    self.playerDataTable = table 
end

function MainSurfaceModel:RefreshGold(gold)
    self.playerDataTable.gold = gold
end

return MainSurfaceModel

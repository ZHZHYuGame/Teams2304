local mainModel = BaseClass("mainModel")

function mainModel:__init()
    self.coin = 0
end
function mainModel:UpdateMainData(coin)
    self.coin = coin
end
function mainModel:GetMainData()
    return self.coin
end
return mainModel

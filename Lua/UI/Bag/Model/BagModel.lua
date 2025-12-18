local BagModel = BaseClass("BagModel")

function BagModel:__init()
    self.bagDatas = {}
end

function BagModel:SetDatas(datas)
    self.bagDatas = datas
end

--获取商品数据(给view提供数据)
function BagModel:GetBagDatas()
    return self.bagDatas
end

return BagModel

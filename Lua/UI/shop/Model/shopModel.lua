local shopModel = BaseClass("shopModel")

function shopModel:__init()
    self.shopDataList = {} -- 存储商品数据
end
function shopModel:UpdateShopData(dataList)
    self.shopDataList = dataList
end
function shopModel:GetShopData()
    return self.shopDataList
end
return shopModel

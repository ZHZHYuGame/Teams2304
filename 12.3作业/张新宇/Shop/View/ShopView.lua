local ShopView = BaseClass("ShopView")
local shopitem=require("UI/Shop/Component/ShopItem")
local Content
local shopdata
function ShopView:__init(prefab)
    self.gameobj=prefab
    shopdata=Json.decode(Resources.Load("Good",typeof(TextAsset)).text)
    Content=prefab.transform: Find("Scroll View/Viewport/Content",typeof(GameObject));
    
end
function ShopView:ShopShowItem(datas)
print(datas.Count)
    for i = 0, datas.Count-1, 1 do
        shopitem.New(ShopView:GetJsonData(datas[i]),Content)
    end
end
function  ShopView:GetJsonData(data)
    local Shop_Item_Data=nil
    for key, value in pairs(shopdata) do
        local id=tostring(data.Id)
        if value.id==id then
            Shop_Item_Data=value
            break
        end
    end
    if Shop_Item_Data.price==nil then
        Shop_Item_Data.price=data.Price
    end
    if Shop_Item_Data.num==nil then
        Shop_Item_Data.num=data.Num
    end
    return Shop_Item_Data
end
function ShopView:OnEnable()
    self.gameobj.gameObject:SetActive(true)
end

return ShopView

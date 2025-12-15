local BagView = BaseClass("BagView")
local BagItem=require("UI/Bag/Component/BagItem")
function BagView:__init(prefab)
    self.uiPrefab=prefab
    self.itemContent=prefab.transform:Find("BagPanel/Scroll View/Viewport/Content")
    self.BagCell={}
    self.bagItemList={}
end
function BagView:UpdateBagUI(data)
    self.bagdata=data
    for i=1, 36,1 do
        local BagCell
        if i<self.bagdata.Count then
            BagCell=BagItem.New(i,self.bagdata[i],self.itemContent)
        else
            BagCell=BagItem.New(i,nil,self.itemContent)
        end
        self.BagCell[i]=BagCell
        self.bagItemList=self.BagCell[i]
    end
end
function BagView:OnEnable()

end

return BagView

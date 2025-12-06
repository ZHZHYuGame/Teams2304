local bagItem = BaseClass("BagItem")

function bagItem:__init(bagData, trans)
    self.gameObject = GameObject.Instantiate(Resources.Load("bagItem"),trans)
    self.name = "bagItem" .. bagData.id
    --查找预制件
    self.icon_img = self.gameObject.transform:Find("icon"):GetComponent(typeof(Image))
    self.num_txt = self.gameObject.transform:Find("num"):GetComponent(typeof(Text))

    --查找红点预制件
    self.cell_redPoint = self.gameObject.transform:Find("redPoint").gameObject

    swi
    self:RegisterRedPointData()


    self:RefreshData(bagData)

    
end

function bagItem:RegisterRedPointData(type)
    
    _G.redPointManager:Bind_UI_To_RedPoint
    (type, self.name,
        function(isShow)
            self.cell_redPoint:SetActive(isShow)
        end
    )

end


function bagItem:RefreshData(bagData)
    if bagData.num == 0 then
        self.icon_img.gameObject:SetActive(false)
        self.num_txt.gameObject:SetActive(false)
    else
        self.icon_img.gameObject:SetActive(true)
        self.num_txt.gameObject:SetActive(true)
        self.icon_img.sprite = Resources.Load("icon/" .. bagData.config.icon,typeof(Sprite))
        self.num_txt.text = bagData.num
    end    

end

return bagItem




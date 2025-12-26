local guidemaskView = BaseClass("guidemaskView")

function guidemaskView:__init(prefab)
    self.prefab = prefab
    GuideMaskManager = CS.GuideMask.AddGuideMask(prefab)
    -- self.GuideMaskManager=prefab:GetComponent("GuideMask")
    GuideMaskManager:Init()
    -- UIMessageControll:Dispatch(UIID.GuideShopUI)
end

function guidemaskView:OnEnable()

end

return guidemaskView

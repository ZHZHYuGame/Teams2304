local MainsurfaceView = BaseClass("MainsurfaceView")

function MainsurfaceView:__init(prefab)
	self.gameObject = prefab
	self:InitUIData()
	self:BindUIEvent()
	
end

function MainsurfaceView:InitUIData()

end
function MainsurfaceView:BindUIEvent()

end
function MainsurfaceView:OnEnable()

end

return MainsurfaceView

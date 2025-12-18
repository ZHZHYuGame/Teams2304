local MainsurfaceView = BaseClass("MainsurfaceView")

function MainsurfaceView:__init(prefab)
	self.gameObject = prefab
	self:InitUIData()
	self:BindUIEvent()
end

function MainsurfaceView:InitUIData()
	--查找ui组件
	self.bagbtn = self.gameObject.transform:Find("pl_Menu/背景/背包"):GetComponent("Button")
	self.FoodText = self.gameObject.transform:Find("pl_resources/食物/num"):GetComponent(typeof(Text))
	self.WoodText = self.gameObject.transform:Find("pl_resources/木材/num"):GetComponent(typeof(Text))
	self.DiaMondText = self.gameObject.transform:Find("pl_resources/宝石/num"):GetComponent(typeof(Text))
end

function MainsurfaceView:InitUIView(playerData)
	self.FoodText.text = playerData.food
	self.WoodText.text = playerData.wood
	self.DiaMondText.text = playerData.diamond
end

function MainsurfaceView:BindUIEvent()
	self.bagbtn.onClick:AddListener(function()
		_G.UImgr:ShowUI(UITypeEnum.bag)
	end)
end

function MainsurfaceView:OnEnable()

end

return MainsurfaceView

local MainsurfaceModel = BaseClass("MainsurfaceModel")

function MainsurfaceModel:__init()
	self:AddListener()
end

function MainsurfaceModel:InitData(playerData)
	self.playerData =
	{
		wood = playerData.Wood,
		food = playerData.Food,
		diamond = playerData.Diamond
	}
end

function MainsurfaceModel:AddListener()

end

function MainsurfaceModel:RemoveListener()

end

return MainsurfaceModel

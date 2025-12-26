local guidemaskControll = BaseClass("guidemaskControll")

function guidemaskControll:__init()
	self:AddListener()
end

function guidemaskControll:PlayGuide(id)
	self.data = ConfigMessageControll:GetType_Id_To_Data(ConfigType.Guide, id[1])
	local btnObj = GameObject.Find(self.data.targetPath)
	btnObj:GetComponent("Button").onClick:AddListener(function()
		if self.data.nextStepCondition == -1 then
			if GuideMaskManager ~= nil then
				GuideMaskManager:Close()
				GuideMaskManager = nil
				_G.UImgr:CloseUI(UITypeEnum.guide,true)
			end
		else
			local t = {}
			t[1] = self.data.nextStepCondition
			self:PlayGuide(t)
		end
	end)
	if GuideMaskManager ~= nil then
		GuideMaskManager:Play(btnObj.gameObject)
	end
end

function guidemaskControll:AddListener()
	UIMessageControll:AddListener(UIID.GuideShop, Bind(self, self.PlayGuide))
end

function guidemaskControll:RemoveListener()

end

return guidemaskControll

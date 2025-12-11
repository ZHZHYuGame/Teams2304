local modelManager = {}


function modelManager:Init()
    self.modelDict = {}

    self:RegisterAllModel()
    
end
function modelManager:RegisterAllModel()
    self:RegisterModel(UITypeEnum.mainSurface)
end


function modelManager:RegisterModel(name)
    
    if self.modelDict[name] == nil then
        local model = UIConfigMgr[name].code_Model.New()
        self.modelDict[name] = model
    end
end


function modelManager:RemoveModel(name)
    if self.modelDict[name] ~= nil then
        self.modelDict[name] = nil
    end
end

function modelManager:GetModel(name)
    if self.modelDict[name] ~= nil then
        return self.modelDict[name] 
    end

    return nil
end



return modelManager
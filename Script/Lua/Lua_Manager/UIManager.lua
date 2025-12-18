local uiManager = {}


function uiManager:Init()
    self.UIBackGroundLayer = GameObject.Find("UIBackGroundLayer")
    self.UIWindowLayer = GameObject.Find("UIWindowLayer")
    self.UITipsLayer = GameObject.Find("UITipsLayer")
    self.UISystemOpenLayer = GameObject.Find("UISystemOpenLayer")
    self.UIGuideLayer = GameObject.Find("UIGuideLayer")
    self.UILoadingLayer = GameObject.Find("UILoadingLayer")
    self.UIHttpLayer = GameObject.Find("UIHttpLayer")

    if self.uiDict == nil then
        self.uiDict = {}
    end
end

function uiManager:ShowUI(uiType)
    --第一次加载ui
    if self.uiDict[uiType] == nil then
        local uiConfigData = UIConfigMgr[uiType]
        
        local layer = uiConfigData.layer

        --创建蒙版

        --加载预制件
        local uiPre = GameObject.Instantiate(_G.ResMgr:LoadAsset(uiConfigData.prefabName), layer.transform)

        --初始化MVC层

        local mono_uiPre = uiConfigData.code_View.New(uiPre)

        local model = _G.modelManager:GetModel(uiType)

        uiConfigData.code_Controll.New(mono_uiPre, model)
        
        --注册C层中的MV层
        self.uiDict[uiType] = mono_uiPre

        
    else
        self:GetUI(uiType)
    end

end

function uiManager:CloseUI(uiType)
    self.uiDict[uiType].gameObject:SetActive(false)
end

function uiManager:GetUI(uiType)
    self.uiDict[uiType].gameObject:SetActive(true)
end

return uiManager

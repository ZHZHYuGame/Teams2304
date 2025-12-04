
local uiManager = {}


function uiManager:Init()
    self.UIBackGroundLayer = GameObject.Find("UIBackGroundLayer")
    self.UIWindowLayer = GameObject.Find("UIWindowLayer")
    self.UITipsLayer = GameObject.Find("UITipsLayer")
    self.UISystemOpenLayer = GameObject.Find("UISystemOpenLayer")
    self.UIGuideLayer = GameObject.Find("UIGuideLayer")
    self.UILoadingLayer = GameObject.Find("UILoadingLayer")
    self.UIHttpLayer = GameObject.Find("UIHttpLayer")
end

function uiManager:ShowUI(uiType)
    if self.uiDict == nil then
        self.uiDict = {}
    end
    --第一次加载ui
    if self.uiDict[uiType] == nil then
       
        local uiConfigData = UIConfigMgr[uiType]
        --加载预制件
        local layer = uiConfigData.layer
        local uiPre = GameObject.Instantiate(Resources.Load(uiConfigData.prefabName), layer.transform)
        --初始化MVC层
        uiConfigData.code_Model.New();
        uiConfigData.code_Controll.New();
        local mono_uiPre = uiConfigData.code_View.New(uiPre);

        --注册M层中的VC层
        uiConfigData.code_Controll.model = uiConfigData.code_Model;
        uiConfigData.code_Controll.view = mono_uiPre;

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
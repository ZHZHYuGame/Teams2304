local uiManager = {}

function uiManager:Init()
    self.UIBackGroudLayer = GameObject.Find("UIBackGroudLayer")
    self.UIWindowLayer = GameObject.Find("UIWindowLayer")
    self.UITipsLayer = GameObject.Find("UITipsLayer")
    self.UISystemOpenLayer = GameObject.Find("UISystemOpenLayer")
    self.UIGuideLayer = GameObject.Find("UIGuideLayer")
    self.UILoadingLayer = GameObject.Find("UILoadingLayer")
    self.UIHttpLayer = GameObject.Find("UIHttpLayer")

    if self.uiDict == nil then
        --UI缓存字典
        self.uiDict = {}
    end
end

--打开UI功能面板
--uiType UI类型
function uiManager:ShowUI(uiType)
    --第一次打开面板
    if self.uiDict[uiType] == nil then
        --打开规则
        local uiConfigData = UIConfigMgr[uiType]
        print("prefabName = ",uiConfigData)
        --预制件信息,所属层集
        local uiPre = GameObject.Instantiate(Resources.Load(uiConfigData.prefabName), UILayer.window.transform) 
        --Code代码，MVC层代码初始化
        uiConfigData.code_Controll.New()
        uiConfigData.code_Model.New()
        local mono_UICode = uiConfigData.code_View.New(uiPre)
        --逻辑层持有数据与显示层
        uiConfigData.code_Controll.model = uiConfigData.code_Model
        uiConfigData.code_Controll.view = mono_UICode
        
        self.uiDict[uiType] = mono_UICode
    else
        self:GetUI(uiType)
        --数据赋值

    end
end
--关闭面板 
--uiType 面板ID
function uiManager:CloseUI(uiType)
end
--获取面板
--uiType 面板ID
function uiManager:GetUI(uiType)
    self.uiDict[uiType].prefab.gameObject:SetActive(true)
end

--层集操作区域

--Tips操作区域

return uiManager

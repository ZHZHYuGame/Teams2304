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
       
        --预制件信息,所属层集
        local uiPre = GameObject.Instantiate(ABManager.GetInstance():LoadAsset_GameObject(string.lower(uiConfigData.prefabName)), UILayer.window.transform)
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
--- 关闭UI面板
--- @param uiType number|string 面板唯一标识ID（如UIType.Bag、"ShopWindow"等）
--- @param isDestroy boolean 可选参数（默认false）：
---                        false - 仅隐藏面板（保留预制体和缓存，便于后续复用）
---                        true  - 彻底销毁面板（删除预制体、清空MVC引用、移除缓存，释放内存）
function uiManager:CloseUI(uiType, isDestroy)
    -- 容错校验：防止空参数/未加载的UI类型导致逻辑异常
    -- 校验项：1.uiType参数是否有效 2.缓存字典是否初始化 3.目标UI是否在缓存中
    if not uiType or not self.uiDict or not self.uiDict[uiType] then
        print(string.format("CloseUI error: UI类型[%s]不存在或未加载", tostring(uiType)))
        return -- 校验失败直接退出，避免后续空指针错误
    end

    -- 获取缓存中目标UI的视图层实例（mono_UICode）
    local targetUI = self.uiDict[uiType]
    -- 安全获取UI预制体对应的GameObject（避免prefab为空时的异常）
    local targetGameObject = targetUI.prefab and targetUI.prefab.gameObject

    -- 1. 基础关闭逻辑：隐藏UI对象（最常用场景，保留对象用于复用）
    if targetGameObject and targetGameObject.activeSelf then -- 仅当对象激活时执行隐藏
        targetGameObject:SetActive(false)
    end

    -- 2. 彻底销毁逻辑：适用于临时UI（如一次性提示框、活动弹窗）
    if isDestroy then
        -- 销毁预制体GameObject（释放Unity侧的内存资源）
        if targetGameObject then
            GameObject.Destroy(targetGameObject)
        end

        -- 清理MVC层引用：避免空引用和内存泄漏（Lua侧的引用管理）
        local uiConfigData = UIConfigMgr[uiType] -- 获取该UI的配置数据
        if uiConfigData then
            -- 清空控制器层对模型/视图的引用
            if uiConfigData.code_Controll then
                uiConfigData.code_Controll.model = nil
                uiConfigData.code_Controll.view = nil
                uiConfigData.code_Controll = nil -- 释放控制器实例引用
            end
            uiConfigData.code_Model = nil        -- 释放模型层实例引用
            uiConfigData.code_View = nil         -- 释放视图层实例引用
        end

        -- 移除缓存字典中的记录：保证下次打开该UI时重新加载初始化
        self.uiDict[uiType] = nil
        print(string.format("UI[%s]已彻底销毁并移除缓存", tostring(uiType)))
    else
        -- 仅隐藏时的扩展点：可在此添加面板状态重置逻辑
        -- 示例：清空输入框、重置滚动条位置、停止动画等
        -- targetUI:ResetState()  -- 假设视图层有状态重置方法
        print(string.format("UI[%s]已隐藏", tostring(uiType)))
    end
end
--获取面板
--uiType 面板ID
function uiManager:GetUI(uiType)
    self.uiDict[uiType].prefab.gameObject:SetActive(true)
end

--层集操作区域

--Tips操作区域

return uiManager

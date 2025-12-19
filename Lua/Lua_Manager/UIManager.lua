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
    -- 1. 从UI配置管理器中获取对应UI类型的配置数据
    -- UIConfigMgr是全局的UI配置表，存储了每个UI的预制件名、脚本类等信息
    local uiConfigData = UIConfigMgr[uiType]

    -- 安全校验：配置不存在则报错并终止执行
    if not uiConfigData then
        error(string.format("ShowUI Error: UI配置不存在，类型=%s", tostring(uiType)))
        return
    end

    -- 2. 检查该UI是否已经创建过（通过uiDict字典缓存已创建的UI）
    if self.uiDict[uiType] == nil then
        -- 2.1 未创建过，则执行UI的创建流程
        -- 预制件名转小写（规范：资源名统一小写，避免大小写问题）
        local prefabName = string.lower(uiConfigData.prefabName)

        -- 2.2 从资源管理器加载UI预制件（ABManager是AssetBundle资源管理器）
        local uiPre = ABManager.GetInstance():LoadAsset_GameObject(prefabName)

        -- 安全校验：预制件加载失败则报错并终止
        if not uiPre then
            error(string.format("ShowUI Error: 预制件加载失败，名称=%s", prefabName))
            return
        end

        -- 2.3 获取UI窗口的父节点（所有UI都挂在这个节点下，保证层级正确）
        local windowLayer = self.UIWindowLayer

        -- 安全校验：父节点不存在则报错并终止
        if not windowLayer or not windowLayer.transform then
            error("ShowUI Error: UI窗口层节点不存在")
            return
        end

        -- 2.4 实例化UI预制件（挂到窗口层下）
        local uiInstance = GameObject.Instantiate(uiPre, windowLayer.transform)

        -- 重置实例名称（去掉Instantiate自动加的"(Clone)"后缀）
        uiInstance.name = uiConfigData.prefabName

        -- 2.5 初始化MVC架构的核心对象
        -- 创建控制器实例（code_Controll是配置中指定的控制器类）
        local controller = uiConfigData.code_Controll.New()
        -- 创建数据模型实例（code_Model是配置中指定的数据模型类）
        local model = uiConfigData.code_Model.New()
        -- 创建视图实例（code_View是配置中指定的视图类，传入UI实例）
        local mono_UICode = uiConfigData.code_View.New(uiInstance)

        -- 2.6 关联MVC三者的引用（互相持有，方便调用）
        controller.model = model    -- 控制器持有模型
        controller.view = mono_UICode -- 控制器持有视图

        -- 2.7 将创建好的UI信息缓存到字典中（方便后续复用）
        self.uiDict[uiType] = {
            view = mono_UICode,      -- 视图实例
            controller = controller, -- 控制器实例
            model = model,           -- 数据模型实例
            gameObject = uiInstance  -- UI游戏物体实例
        }
    else
        -- 3. 该UI已创建过，执行"显示"逻辑
        local uiInfo = self.uiDict[uiType]

        -- 安全校验：缓存的UI信息有效
        if uiInfo and uiInfo.gameObject then
            -- 3.1 激活UI物体（显示UI）
            uiInfo.gameObject:SetActive(true)

            -- 3.2 如果控制器有Refresh方法，则调用刷新（更新UI数据）
            if uiInfo.controller and uiInfo.controller.Refresh then
                uiInfo.controller:Refresh()
            end
        else
            -- 异常处理：缓存的UI信息无效（比如物体被销毁）
            -- 清除无效缓存
            self.uiDict[uiType] = nil
            -- 重新调用ShowUI创建新的UI实例
            self:ShowUI(uiType)
            return
        end
    end

    -- 4. 最终校验：确认UI实例创建/显示成功
    local targetUI = self:GetUI(uiType)
    if not targetUI then
        -- 仅警告（不终止）：获取UI实例失败（日志提示，方便调试）
        warn(string.format("ShowUI Warning: 获取UI实例失败，类型=%s", tostring(uiType)))
        return
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
    -- 改用缓存中实际存在的gameObject字段
    local targetGameObject = targetUI.gameObject

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
        --print(string.format("UI[%s]已彻底销毁并移除缓存", tostring(uiType)))
    else
        -- 仅隐藏时的扩展点：可在此添加面板状态重置逻辑
        -- 示例：清空输入框、重置滚动条位置、停止动画等
        -- targetUI:ResetState()  -- 假设视图层有状态重置方法
        --print(string.format("UI[%s]已隐藏", tostring(uiType)))
    end
end

--获取面板
--uiType 面板ID
function uiManager:GetUI(uiType)
    -- 1. 空值校验：避免uiType/nil导致崩溃
    if not uiType or not self.uiDict then
        return nil
    end
    -- 2. 从缓存字典取UI信息
    local uiInfo = self.uiDict[uiType]
    -- 3. 安全返回视图实例（有则返回，无则返回nil）
    return uiInfo and uiInfo.view or nil
end

--层集操作区域

--Tips操作区域

function uiManager:ShowGoodsTips(tipsType, goodsData, worldPos, offset)
    -- ===================== 第一步：安全校验（防止传错参数导致崩溃） =====================
    -- 1. 检查必填参数：Tips类型、商品数据、坐标缺一不可
    if not tipsType then
        print("显示Tips失败：没传Tips面板类型")
        return
    end
    if not goodsData then
        print("显示Tips失败：没传商品数据")
        return
    end
    if not worldPos then
        print("显示Tips失败：没传显示位置")
        return
    end

    -- 初始化偏移量：如果没传偏移，默认向右移20像素、向上移0像素
    local tipsOffset = offset or Vector2(20, 0)
    -- 取出Tips缓存（上面InitTipsCache初始化的）
    local tipsCache = self.tipsCache

    -- ===================== 第二步：创建/复用Tips面板（核心复用逻辑） =====================
    -- 如果Tips面板还没创建过（第一次鼠标移到商品上）
    if not tipsCache.gameObject then
        -- 1. 从全局配置表找Tips面板的预制件/脚本信息
        local uiConfigData = UIConfigMgr[tipsType]
        if not uiConfigData then
            print("显示Tips失败：配置表里没有这个Tips类型 → " .. tostring(tipsType))
            return
        end

        -- 2. 加载Tips预制件（从AB包加载，资源名转小写避免大小写问题）
        local prefabName = string.lower(uiConfigData.prefabName)
        local tipsPre = ABManager.GetInstance():LoadAsset_GameObject(prefabName)
        if not tipsPre then
            print("显示Tips失败：预制件加载不出来 → " .. prefabName)
            return
        end

        -- 3. 找到Tips专用层级（确保Tips显示在最上层，不被其他UI挡住）
        local tipsLayer = self.UITipsLayer
        if not tipsLayer or not tipsLayer.transform then
            print("显示Tips失败：场景里找不到UITipsLayer节点")
            return
        end

        -- 4. 创建Tips面板实例（挂到Tips层下）
        local tipsInstance = GameObject.Instantiate(tipsPre, tipsLayer.transform)
        -- 去掉Unity自动加的"(Clone)"后缀，方便调试
        tipsInstance.name = uiConfigData.prefabName
        -- 先隐藏，等数据初始化完再显示，避免闪屏
        tipsInstance:SetActive(false)

        -- 5. 初始化Tips的MVC（和你原有UI的MVC逻辑一致）
        -- 创建控制器（处理数据逻辑）
        local controller = uiConfigData.code_Controll.New()
        -- 创建数据模型（存储数据）
        local model = uiConfigData.code_Model.New()
        -- 创建视图（控制UI显示）
        local mono_UICode = uiConfigData.code_View.New(tipsInstance)

        -- 6. 关联MVC（让控制器/视图/模型互相能调用）
        controller.model = model
        controller.view = mono_UICode
        mono_UICode.controller = controller
        mono_UICode.model = model

        -- 7. 把创建好的Tips存到缓存里（下次直接用）
        tipsCache.gameObject = tipsInstance       -- 存游戏物体
        tipsCache.targetUI = {                    -- 存MVC实例
            view = mono_UICode,
            controller = controller,
            model = model
        }
        tipsCache.tipsType = tipsType             -- 存Tips类型
    end

    -- ===================== 第三步：防高频刷新（性能优化） =====================
    -- 获取当前时间（Unity的Time.time，单位秒）
    local curTime = Time.time
    -- 如果两次更新间隔小于50ms，直接返回（避免鼠标快速移动时疯狂刷新）
    if curTime - tipsCache.lastUpdateTime < tipsCache.updateInterval then
        return
    end
    -- 更新最后一次更新时间（下次判断用）
    tipsCache.lastUpdateTime = curTime

    -- ===================== 第四步：更新Tips数据和位置（核心显示逻辑） =====================
    -- 从缓存取出Tips的MVC和游戏物体
    local tipsUI = tipsCache.targetUI
    local tipsObj = tipsCache.gameObject

    -- 安全校验：缓存的Tips实例有效
    if tipsUI and tipsObj then
        -- 1. 设置Tips显示位置（把鼠标/格子的世界坐标转成UI坐标）
        -- Camera.main：主相机（确保是UI相机，否则坐标会错）
        local screenPos = Camera.main:WorldToScreenPoint(worldPos)
        -- 获取Tips面板的RectTransform（UI的位置组件）
        local rectTransform = tipsObj:GetComponent(typeof(RectTransform))
        if rectTransform then
            -- 适配Canvas缩放（不同分辨率下位置不变）
            local canvasScaler = self.UITipsLayer:GetComponent(typeof(UnityEngine.UI.CanvasScaler))
            local scaleFactor = canvasScaler and canvasScaler.scaleFactor or 1
            -- 设置最终位置（加上偏移量）
            rectTransform.anchoredPosition = Vector2(
                screenPos.x / scaleFactor + tipsOffset.x,  -- X轴：屏幕X坐标 + 偏移
                screenPos.y / scaleFactor + tipsOffset.y   -- Y轴：屏幕Y坐标 + 偏移
            )
        end

        -- 2. 更新商品数据（核心：把当前商品的名称/图片/描述显示到Tips上）
        -- 要求Tips控制器必须实现UpdateGoodsData方法（下面会给示例）
        if tipsUI.controller and tipsUI.controller.UpdateGoodsData then
            tipsUI.controller:UpdateGoodsData(goodsData)
        else
            print("Tips控制器没实现UpdateGoodsData方法，无法更新商品数据")
        end

        -- 3. 显示Tips面板（终于激活了！）
        tipsObj:SetActive(true)
        tipsCache.isShowing = true  -- 标记为显示状态
    end
end

--- 隐藏商品Tips提示框（鼠标移出商品格子时调用）
--- 通俗解释：鼠标离开商品格子时，隐藏Tips面板（不销毁，下次复用）
--- @param isDestroy boolean 可选参数（默认false）：
---                        false（默认）：仅隐藏，保留面板（下次直接用）
---                        true：彻底销毁（仅切换场景/退出游戏时用）
function uiManager:HideGoodsTips(isDestroy)
    -- 取出Tips缓存
    local tipsCache = self.tipsCache
    -- 如果Tips面板都没创建过，直接返回
    if not tipsCache or not tipsCache.gameObject then
        return
    end

    -- 1. 基础操作：隐藏Tips面板
    if tipsCache.gameObject.activeSelf then  -- 只有面板是显示状态时才隐藏
        tipsCache.gameObject:SetActive(false)
        tipsCache.isShowing = false          -- 标记为隐藏状态
    end

    -- 2. 彻底销毁（极少用，比如切换场景时释放内存）
    if isDestroy then
        -- 销毁Unity侧的游戏物体
        GameObject.Destroy(tipsCache.gameObject)
        -- 清空缓存（下次需要重新创建）
        self.tipsCache = {
            isShowing = false,
            targetUI = nil,
            gameObject = nil,
            lastUpdateTime = 0,
            updateInterval = 0.05
        }
    end
end

--- 检查Tips是否正在显示（外部逻辑判断用）
--- 通俗解释：比如想判断"当前有没有显示商品Tips"，就调这个方法
--- @return boolean true=显示中，false=没显示
function uiManager:IsTipsShowing()
    -- 安全校验：缓存存在且标记为显示状态
    return self.tipsCache and self.tipsCache.isShowing or false
end
return uiManager

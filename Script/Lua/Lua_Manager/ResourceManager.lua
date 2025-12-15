local resourcesManager = {}
--初始化资源包
function resourcesManager:Init()
    ABManager.GetInstance():OnInit()
end
--查找AB包图集中的精灵
function resourcesManager:LoadAtlasAsset(Path, name)
    local obj = ABManager.GetInstance():LoadAssetArrayLua("png/" .. Path, typeof(Sprite))
    print(obj.Length)
    for i = 0, obj.Length - 1 do
        if name == obj[i].name then
            return obj[i]
        end
    end
    print("查找的精灵不存在")
end
--查找AB包中的预制体
function resourcesManager:LoadAsset(name)
    local obj = ABManager.GetInstance():LoadAsset("prefab/" .. name, typeof(GameObject))
    return obj
end
--查找AB包图片
function resourcesManager:LoadPictureAsset(name)
    local obj = ABManager.GetInstance():LoadAsset("jpg/" .. name, typeof(GameObject))
    return obj
end

return resourcesManager

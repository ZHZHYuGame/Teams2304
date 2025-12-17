ConfigManager = BaseClass("ConfigManager") --配置表管理

function ConfigManager:Init()
    self.itemConfig = json.decode(Resources.Load("BagData", typeof(TextAsset)).text);
end

function ConfigManager:GetItemData(id)
    for key, value in pairs(self.itemConfig) do
        if value.id == tostring(id) then
            return value
        end
    end
    print("查找不到这个物品 ====" .. id)
    return nil
end


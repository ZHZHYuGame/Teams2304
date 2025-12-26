ConfigMessageControll = {}
local ConfigDic = {}
function ConfigMessageControll:Init()
    ConfigDic[ConfigType.Guide] = {
        {
            id = 1,
            stepName = "商店引导",
            description = "点击商店按钮",
            targetPath = "UIWindowLayer/UI_Window_main(Clone)/BtnTran/Shop",
            nextStepCondition = 2,
            
        },
        {
            id = 2,
            stepName = "商店引导",
            description = "点击购买按钮购买道具",
            targetPath = "UIWindowLayer/UI_Window_Shop(Clone)/Purchase Section/BuyBut",
            nextStepCondition = 3,
            
        },
        {
            id = 3,
            stepName = "商店引导",
            description = "点击商店关闭按钮",
            targetPath = "UIWindowLayer/UI_Window_Shop(Clone)/CloseBtn",
            nextStepCondition = -1,
            
        }
    }
    ConfigDic[ConfigType.Role] = Json.decode(Resources.Load("Role", typeof(TextAsset)).text)
    -- ConfigDic[ConfigType.Good] = Json.decode(Resources.Load("Good", typeof("TextAsset")).text)
    -- ConfigDic[ConfigType.Guide] = Json.decode(Resources.Load("Guide", typeof("TextAsset")).text)
end

function ConfigMessageControll:GetData(Type)
    local t = {}
    if ConfigDic[Type] ~= nil then
        t = ConfigDic[Type]
    end
    return t
end

function ConfigMessageControll:GetType_Id_To_Data(Type, Id)
    local t=self:GetData(Type)
    local data=nil
    if t ~= nil then
        for key, value in pairs(t) do
            if Id==value.id then
                data=value
                break
            end
        end
    end
    return data
end

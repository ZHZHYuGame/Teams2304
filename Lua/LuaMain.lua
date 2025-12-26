require("Lipus/BaseClass")
require("Lipus/head")
require("Lipus/TimeTool")
Json = require("Lipus/json")
require("Net/NetManager")
require("Lipus/LuaUtil")
require("Net/NetID")
require("UI/UIID")
require("GameEnum/ConfigType")
require("Message/UI_MessageControll")
require("Message/Net_MessageControll")
require("Message/ConfigMessageControll")
require("GameEnum/UIRedPointType")
NetManager:Init()
_G.UImgr = require("Lua_Manager/UIManager")
_G.UImgr:Init()
require("GameEnum/UILayer")
require("GameEnum/UITypeEnum")
require("UI/UIConfigMgr")
Red_Mgr = require("Lua_Manager/UIRedPointManager").New()
-- require("Net/NetID")


local btn
local image
function LuaStart()
    ConfigMessageControll:Init()
    _G.UImgr:ShowUI(UITypeEnum.log)
    _G.UImgr:ShowUI(UITypeEnum.guide)
end

function LuaUpdata()
    TimeTool:Update()
end

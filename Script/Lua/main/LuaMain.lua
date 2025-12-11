
require("Common/BaseClass")
require("Common/LuaUtil")
json = require("Common/json")
require("Common/head")
require("Config/ConfigManager")
ConfigManager:Init()

require("GameEnum/UIRedPointType")

require("Net/NetID")
require("Net/ClientID")
require("Message/Net_MessageControll")
require("Message/UI_MessageControll")
require("Net/NetManager")
Lua_NetManager:Init()


_G.UImgr = require("Lua_Manager/UIManager")
_G.UImgr:Init()
require("GameEnum/UILayer")
require("GameEnum/UITypeEnum")
require("UI/UIConfigMgr")

_G.modelManager = require("Lua_Manager/ModelManager")
_G.modelManager:Init()

function Lua_Start()
    _G.UImgr:ShowUI(UITypeEnum.mainSurface)
end

function Lua_Update()
    
end


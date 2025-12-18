require("Common/BaseClass")
require("Common/LuaUtil")
_G.tool = require("Common/GameTool")
json = require("Common/json")
require("Common/head")
require("Config/ConfigManager")
ConfigManager:Init()

require("GameEnum/UIRedPointType")
require("GameEnum/UIMaskType")

require("Net/NetID")
require("Net/ClientID")
require("Message/Net_MessageControll")
require("Message/UI_MessageControll")
require("Net/NetManager")
Lua_NetManager:Init()

_G.UImgr = require("Lua_Manager/UIManager")
_G.UImgr:Init()
_G.ResMgr = require("Lua_Manager/ResourceManager")
_G.ResMgr:Init()
require("GameEnum/UILayer")
require("GameEnum/UITypeEnum")
require("UI/UIConfigMgr")
_G.modelManager = require("Lua_Manager/ModelManager")
_G.modelManager:Init()
function Lua_Start()
    _G.UImgr:ShowUI(UITypeEnum.mainSurface)
    local c_msg = MyGame.C_To_S_GetPlayerData_Msg()
    NetManager.GetInstance():SendMessage(NetID.C_To_S_GetPlayerData_Msg,Protobuf.ToByteArray(c_msg))
    
end
function Lua_Update()

end

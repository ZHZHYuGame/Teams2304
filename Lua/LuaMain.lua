require("Lipus/BaseClass")
require("Lipus/head")
require("Net/NetManager")
require("Lipus/LuaUtil")
require("Net/NetID")
require("UI/UIID")
require("Message/UI_MessageControll")
require("Message/Net_MessageControll")
NetManager:Init()
_G.UImgr = require("Lua_Manager/UIManager")
_G.UImgr:Init()
require("GameEnum/UILayer")
require("GameEnum/UITypeEnum")
require("UI/UIConfigMgr")
-- require("Net/NetID")


local bagbtn
function LuaStart()
    _G.UImgr:ShowUI(UITypeEnum.main)
    _G.UImgr:ShowUI(UITypeEnum.shop)
    --_G.UImgr:ShowUI(UITypeEnum.bag)
end

function LuaUpdata()

end

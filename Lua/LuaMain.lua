require("Lipus/BaseClass")
require("Lipus/head")
-- require("Message/Net_MessageControll")
-- require("Net/NetManager")
require("Lipus/LuaUtil")
-- NetManager:Init()
print("baseclass = ", _G.BaseClass)
_G.UImgr = require("Lua_Manager/UIManager")
_G.UImgr:Init()
require("GameEnum/UILayer")
require("GameEnum/UITypeEnum")
require("UI/UIConfigMgr")
-- require("Net/NetID")


local bagbtn
function LuaStart()
    -- print("going lua..............")

    -- _G.UImgr:ShowUI(UITypeEnum.shop)
    --  _G.UImgr:ShowUI(UITypeEnum.bag)
   
    -- bagbtn = GameObject.Find("bagbtn"):GetComponent("Button")
    -- bagbtn.onClick:AddListener(function()
       
    --      _G.UImgr:ShowUI(UITypeEnum.bag)
      
    -- end)
    -- local msg = MyGame.C_To_S_Chat_Msg()
    -- netManager.GetInstance():SendMessage(NetID.C_To_S_WorldChat_Msg, Protobuf.ToByteArray(msg))
end

function LuaUpdata()
-- print("LuaUpdata..............")
end

--local c_Chat_Msg = MyGame.C_To_S_Chat_Msg()

--print("chat msg = ", c_Chat_Msg)

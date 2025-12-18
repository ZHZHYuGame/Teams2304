<<<<<<< HEAD:Script/Lua/UI/Bag/BagConfig.lua
local BagConfig = {
	type = UITypeEnum.bag,
	prefabName = "UI_Window_bag",
	layer = UILayer.window,
	mask = UIMaskType.Full_None,
=======
local UIBagConfig = {
	type = UITypeEnum.Bag,
	prefabName = "UI_Window_Bag",
	layer = UILayer.window,
>>>>>>> 7d63ed1a3cbee6a85ef9add2994b3b6d14e406ea:测试版本/Scripts/Lua/UI/Bag/BagConfig.lua
	code_Model = require("UI/Bag/Model/BagModel"),
	code_View = require("UI/Bag/View/BagView"),
	code_Controll = require("UI/Bag/Controll/BagControll"),
}
<<<<<<< HEAD:Script/Lua/UI/Bag/BagConfig.lua
return BagConfig
=======
return UIBagConfig
>>>>>>> 7d63ed1a3cbee6a85ef9add2994b3b6d14e406ea:测试版本/Scripts/Lua/UI/Bag/BagConfig.lua

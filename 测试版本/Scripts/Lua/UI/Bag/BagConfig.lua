local UIBagConfig = {
	type = UITypeEnum.Bag,
	prefabName = "UI_Window_Bag",
	layer = UILayer.window,
	code_Model = require("UI/Bag/Model/BagModel"),
	code_View = require("UI/Bag/View/BagView"),
	code_Controll = require("UI/Bag/Controll/BagControll"),
}
return UIBagConfig

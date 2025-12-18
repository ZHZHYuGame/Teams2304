local BagConfig = {
	type = UITypeEnum.bag,
	prefabName = "UI_Window_bag",
	layer = UILayer.window,
	mask = UIMaskType.Full_None,
	code_Model = require("UI/Bag/Model/BagModel"),
	code_View = require("UI/Bag/View/BagView"),
	code_Controll = require("UI/Bag/Controll/BagControll"),
}
return BagConfig
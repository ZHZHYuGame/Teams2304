local UIshopConfig = {
	type = UITypeEnum.shop,
	prefabName = "UI_Window_shop",
	layer = UILayer.window,
	code_Model = require("UI/shop/Model/shopModel"),
	code_View = require("UI/shop/View/shopView"),
	code_Controll = require("UI/shop/Controll/shopControll"),
}
return UIshopConfig

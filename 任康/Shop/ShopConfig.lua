local UIShopConfig = {
	type = UITypeEnum.shop,
	prefabName = "UI_Window_Shop",
	layer = UILayer.window,
	code_Model = require("UI/Shop/Model/ShopModel"),
	code_View = require("UI/Shop/View/ShopView"),
	code_Controll = require("UI/Shop/Controll/ShopControll"),
}
return UIShopConfig

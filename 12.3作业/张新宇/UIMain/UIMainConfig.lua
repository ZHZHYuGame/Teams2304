local UIUIMainConfig = {
	type = UITypeEnum.UIMain,
	prefabName = "UI_Window_UIMain",
	layer = UILayer.window,
	code_Model = require("UI/UIMain/Model/UIMainModel"),
	code_View = require("UI/UIMain/View/UIMainView"),
	code_Controll = require("UI/UIMain/Controll/UIMainControll"),
}
return UIUIMainConfig

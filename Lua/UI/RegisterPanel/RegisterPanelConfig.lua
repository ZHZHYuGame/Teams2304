local UIRegisterPanelConfig = {
	type = UITypeEnum.RegisterPanel,
	prefabName = "UI_Window_Register",
	layer = UILayer.window,
	code_Model = require("UI/RegisterPanel/Model/RegisterPanelModel"),
	code_View = require("UI/RegisterPanel/View/RegisterPanelView"),
	code_Controll = require("UI/RegisterPanel/Controll/RegisterPanelControll"),
}
return UIRegisterPanelConfig

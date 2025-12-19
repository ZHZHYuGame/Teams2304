local UILogPanelConfig = {
	type = UITypeEnum.LogPanel,
	prefabName = "UI_Window_Login",
	layer = UILayer.window,
	code_Model = require("UI/LogPanel/Model/LogPanelModel"),
	code_View = require("UI/LogPanel/View/LogPanelView"),
	code_Controll = require("UI/LogPanel/Controll/LogPanelControll"),
}
return UILogPanelConfig

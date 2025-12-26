local UIguidemaskConfig = {
	type = UITypeEnum.guidemask,
	prefabName = "UI_Window_guidemask",
	layer = UILayer.guide,
	code_Model = require("UI/guidemask/Model/guidemaskModel"),
	code_View = require("UI/guidemask/View/guidemaskView"),
	code_Controll = require("UI/guidemask/Controll/guidemaskControll"),
}
return UIguidemaskConfig

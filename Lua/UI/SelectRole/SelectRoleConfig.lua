local UISelectRoleConfig = {
	type = UITypeEnum.SelectRole,
	prefabName = "UI_Window_SelectRole",
	layer = UILayer.window,
	code_Model = require("UI/SelectRole/Model/SelectRoleModel"),
	code_View = require("UI/SelectRole/View/SelectRoleView"),
	code_Controll = require("UI/SelectRole/Controll/SelectRoleControll"),
}
return UISelectRoleConfig

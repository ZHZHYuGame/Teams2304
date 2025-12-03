local UIMainSurfaceConfig = {
	type = UITypeEnum.MainSurface,
	prefabName = "UI_Window_MainSurface",
	layer = UILayer.window,
	code_Model = require("UI/MainSurface/Model/MainSurfaceModel"),
	code_View = require("UI/MainSurface/View/MainSurfaceView"),
	code_Controll = require("UI/MainSurface/Controll/MainSurfaceControll"),
}
return UIMainSurfaceConfig

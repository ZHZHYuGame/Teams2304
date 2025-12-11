local UIMainsurfaceConfig = {
	type = UITypeEnum.Mainsurface,
	prefabName = "UI_Window_Mainsurface",
	layer = UILayer.window,
	code_Model = require("UI/Mainsurface/Model/MainsurfaceModel"),
	code_View = require("UI/Mainsurface/View/MainsurfaceView"),
	code_Controll = require("UI/Mainsurface/Controll/MainsurfaceControll"),
}
return UIMainsurfaceConfig

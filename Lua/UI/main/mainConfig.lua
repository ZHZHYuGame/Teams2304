local UImainConfig = {
    type = UITypeEnum.main,
    prefabName = "UI_Window_main",
    layer = UILayer.window,
    code_Model = require("UI/main/Model/mainModel"),
    code_View = require("UI/main/View/mainView"),
    code_Controll = require("UI/main/Controll/mainControll"),
}
return UImainConfig

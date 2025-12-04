local BagConfig = 
{
    prefabName = "UI_Window_Bag",
    layer = UILayer.window,
    code_Model = require("UI/Bag/Model/BagModel"),
    code_Controll = require("UI/Bag/Controll/BagControll"),
    code_View = require("UI/Bag/View/BagView")
}

return BagConfig
local LogPanelView = BaseClass("LogPanelView")

function LogPanelView:__init(prefab)
    self.prefab = prefab
    self.logNameIf = self.prefab.transform.GetChild(1):GetComponent("InputField")
    self.logPassIf = self.prefab.transform.GetChild(3):GetComponent("InputField")
    self.logbtn = self.prefab.transform.GetChild(4):GetComponent("Button")
    self.registerbtn = self.prefab.transform.GetChild(5):GetComponent("Button")
    self.logbtn.onClick:AddListener(function()
        local logData = MyGame.C_To_S_Log()
        logData.UserName = self.logNameIf.text
        logData.PassWord = self.logPassIf.text
        CS.NetMessageControll.GetInstance():SendMessage(CS.NetID.C_To_S_Log, Protobuf.ToByteArray(logData))
    end)
    self.registerbtn.onClick:AddListener(function()
        self.prefab:SetActive(false)
        _G.UImgr:ShowUI(UITypeEnum.register)
    end)
end

function LogPanelView:OnEnable()

end

return LogPanelView

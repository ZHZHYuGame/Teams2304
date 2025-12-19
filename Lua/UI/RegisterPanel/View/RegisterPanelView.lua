local RegisterPanelView = BaseClass("RegisterPanelView")

function RegisterPanelView:__init(prefab)
    self.prefab = prefab
    self.registerNameIf = self.prefab.transform:GetChild(1):GetComponent("InputField")
    self.registerPassIf = self.prefab.transform:GetChild(3):GetComponent("InputField")
    self.yesbtn = self.prefab.transform:GetChild(4):GetComponent("Button")
    self.returnbtn = self.prefab.transform:GetChild(5):GetComponent("Button")
    self.yesbtn.onClick:AddListener(function()
        local logData = MyGame.C_To_S_Register()
        logData.UserName = self.registerNameIf.text
        logData.PassWord = self.registerPassIf.text
        CS.NetManager.GetInstance():SendMessage(CS.NetID.C_To_S_Register, Protobuf.ToByteArray(logData))
    end)
    self.returnbtn.onClick:AddListener(function()
        self.prefab:SetActive(false)
        _G.UImgr:ShowUI(UITypeEnum.log)
    end)
end

function RegisterPanelView:OnEnable()

end

return RegisterPanelView

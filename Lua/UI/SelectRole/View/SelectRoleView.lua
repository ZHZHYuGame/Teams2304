local SelectRoleView = BaseClass("SelectRoleView")
function SelectRoleView:__init(prefab)
    self.Roles = ConfigMessageControll:GetData(ConfigType.Role)
    self.roleid = 0
    self.ui_Camera = GameObject.Find("UI_Camera").gameObject:GetComponent("Camera")
    self.prefab = prefab
    self.roles = {}
    self.grey_Tog = prefab.transform:GetChild(0):GetComponent("Toggle")
    self.red_Tog = prefab.transform:GetChild(1):GetComponent("Toggle")
    self.green_Tog = prefab.transform:GetChild(2):GetComponent("Toggle")
    self.blue_Tog = prefab.transform:GetChild(3):GetComponent("Toggle")
    self.play_Btn = prefab.transform:GetChild(4):GetComponent("Button");
    self.roles["red"] = GameObject.Instantiate(ABManager.GetInstance():LoadAsset_GameObject("hero_fire"));
    self:ShowRole("red")
    -- self.roles["red"].transform:Rotate(Vector3.up, 180)
    self.role = "hero_fire"
    self.grey_Tog.onValueChanged:AddListener(function(isOn)
        if isOn then
            if self.roles["grey"] ~= nil then
                self:ShowRole("grey")
            else
                self.roles["grey"] = GameObject.Instantiate(ABManager.GetInstance():LoadAsset_GameObject("hero_rock"));
                self:ShowRole("grey")
                -- self.roles["grey"].transform:Rotate(Vector3.up, 180)
            end
            self.role = "hero_rock"
        end
    end)
    self.red_Tog.onValueChanged:AddListener(function(isOn)
        if isOn then
            if self.roles["red"] ~= nil then
                self:ShowRole("red")
            else
                self.roles["red"] = GameObject.Instantiate(ABManager.GetInstance():LoadAsset_GameObject("hero_fire"));
                self:ShowRole("red")
                -- self.roles["red"].transform:Rotate(Vector3.up, 180)
            end
            self.role = "hero_fire"
        end
    end)
    self.green_Tog.onValueChanged:AddListener(function(isOn)
        if isOn then
            if self.roles["green"] ~= nil then
                self:ShowRole("green")
            else
                self.roles["green"] = GameObject.Instantiate(ABManager.GetInstance():LoadAsset_GameObject("hero_nature"));
                self:ShowRole("green")
                -- self.roles["green"].transform:Rotate(Vector3.up, 180)
            end
            self.role = "hero_nature"
        end
    end)
    self.blue_Tog.onValueChanged:AddListener(function(isOn)
        if isOn then
            if self.roles["blue"] ~= nil then
                self:ShowRole("blue")
            else
                self.roles["blue"] = GameObject.Instantiate(ABManager.GetInstance():LoadAsset_GameObject("hero_ice"));
                self:ShowRole("blue")
                -- self.roles["blue"].transform:Rotate(Vector3.up, 180)
            end
            self.role = "hero_ice"
        end
    end)

    self.play_Btn.onClick:AddListener(function()
        for key, value in pairs(self.Roles) do
            if value.prefab == self.role then
                self.roleid = value.id
                break
            end
        end
        print("进入场景")
        local c_Msg = MyGame.C_To_S_RoleID()
        c_Msg.Id = self.roleid
        CS.NetManager.GetInstance():SendMessage(CS.NetID.C_To_S_RoleID, Protobuf.ToByteArray(c_Msg))
        SceneManager.LoadScene("Game");
        self.ui_Camera.nearClipPlane = 99
        self.ui_Camera.transform.position = Vector3(78, 1, -98)
        _G.UImgr:ShowUI(UITypeEnum.main)
        _G.UImgr:ShowUI(UITypeEnum.shop)
        _G.UImgr:ShowUI(UITypeEnum.bag)
        _G.UImgr:CloseUI(UITypeEnum.log, true)
        _G.UImgr:CloseUI(UITypeEnum.selectrole, true)
        TimeTool:Invoke(0.2, function()
            CS.Map.GetInstance():Create(self.role)
        end)
    end)
end

function SelectRoleView:ShowRole(rolename)
    if self.roles[rolename] ~= nil then
        for key, value in pairs(self.roles) do
            if key == rolename then
                value.transform.position = Vector3(0, 0, 0)
            else
                value.transform.position = Vector3(34, 0, 0)
            end
        end
    end
end

function SelectRoleView:OnEnable()

end

return SelectRoleView

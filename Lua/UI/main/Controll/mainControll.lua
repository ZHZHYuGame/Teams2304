local mainControll = BaseClass("mainControll")

function mainControll:__init()
    self.model = nil 
    self.view = nil
    local getMainData = MyGame.C_To_S_Main()
    CS.NetManager.GetInstance():SendMessage(CS.NetID.C_To_S_Main, Protobuf.ToByteArray(getMainData))
    self:AddListener()
    
end

function mainControll:AddListener()
    NetMessageControll:AddListener(NetID.S_To_C_Main, Bind(self, self.GetMainDataRefreshed))
end
function mainControll:GetMainDataRefreshed(mainDataList)
    self.data = mainDataList[1].Money
    self.model:UpdateMainData(self.data)
    if self.view then
        self.view:Refresh(self.model:GetMainData())
    end
end
function mainControll:RemoveListener()

end

return mainControll

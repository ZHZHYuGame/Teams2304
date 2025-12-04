local bagControll = BaseClass("bagControll")
function bagControll:__init()
    self:AddListener()
end

function bagControll:AddListener()
    NetMessageControll:AddListener(NetID.S_To_C_BagInit_Msg, Bind(self, self.S_To_C_BagInit_Msg_Handle))
    NetMessageControll:AddListener(NetID.S_To_C_BagAddData_Msg, Bind(self, self.S_To_C_BagAddData_Msg_Handle))
end

function bagControll:S_To_C_BagInit_Msg_Handle(bagitemlist)
    self.bagitemlist = bagitemlist[1]
    self.view:ShowBagItem(self.bagitemlist)
end

function bagControll:S_To_C_BagAddData_Msg_Handle(bagdata)
    self.view:ShowNewBagItem(bagdata[1])
end

function bagControll:RemoveListener()

end

function bagControll:OnEnable()

end

return bagControll

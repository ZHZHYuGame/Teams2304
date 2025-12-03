local MainSurfaceView = BaseClass("MainSurfaceView")


function MainSurfaceView:OpenShopUI()

    _G.UImgr:ShowUI(UITypeEnum.shop)
    if self.isOneOpenShop == false then
        self.isOneOpenShop = true
        local c_msg = MyGame.C_To_S_GetShopInfos_Msg()
        NetManager.GetInstance():SendMessage(NetID.C_To_S_GetShopInfos_msg,Protobuf.ToByteArray(c_msg))
    end
end

function MainSurfaceView:__init(prefab)

    self.gameObject = prefab
    --查找预制件
    self.gold_txt = self.gameObject.transform:Find("goldRoot/goldTxt"):GetComponent(typeof(Text))
    self.shopBtn = self.gameObject.transform:Find("shopBtn"):GetComponent(typeof(Button))
    self.isOneOpenShop = false
    self.shopBtn.onClick:AddListener(function()
        self:OpenShopUI()
    end)
end  

function MainSurfaceView:OnInitView(table)
    self.gold_txt.text = table.gold;
end

function MainSurfaceView:RefreshGold(gold)
    self.gold_txt.text = gold;
end

function MainSurfaceView:OnEnable()

end

return MainSurfaceView

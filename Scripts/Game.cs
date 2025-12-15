using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : Singleton<Game>
{
    public Image image;
    // Start is called before the first frame update
    public void Start()
    {
        //NetManager.GetInstance().Start();
        //LuaMain.GetInstance().Start();
        ABManager.GetInstance().Start();
        //Instantiate(ABManager.GetInstance().LoadAsset("cube"));
        //image.sprite = ABManager.GetInstance().LoadAsset<Sprite>("40");
        GameObject.Instantiate(ABManager.GetInstance().LoadAsset_GameObject("ui_window_shop"),GameObject.Find("Canvas").transform);
        //string path = Application.streamingAssetsPath + "/ABs/0.u3d";
        //通过资源包的路径找到这一个资源包
        //AssetBundle aBundle = AssetBundle.LoadFromFile(path);
        //Sprite[] sprites = aBundle.LoadAssetWithSubAssets<Sprite>("0");
        //通过资源包找到里面存在的物体
        //Sprite sprite = System.Array.Find(sprites,item=>item.name=="0_4");
        //image.sprite = sprite;
    }

    // Update is called once per frame
    public void Update()
    {
        //NetManager.GetInstance().Update();
        //Tool_Time_Manager.GetInstance().Update();
        //LuaMain.GetInstance().Update();
    }
}

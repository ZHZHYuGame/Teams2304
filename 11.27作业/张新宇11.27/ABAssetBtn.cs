using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ABAssetBtn : MonoBehaviour
{
    public Button btn_Confirm;
    public Button btn_Cancel;
    // Start is called before the first frame update
    void Start()
    {
        btn_Confirm.onClick.AddListener(() =>
        {
            MessageControll.GetInstance().Dispach(Client_Const_Event.Hotfix_Confirm_Event);
        });
        btn_Cancel.onClick.AddListener(() =>
        {

        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

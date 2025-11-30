using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button yes, no;
    public Image hotfixImage;
    // Start is called before the first frame update
    void Start()
    {
        yes.onClick.AddListener(() =>
        {
            MessageControll.Ins.Dispach(NetID.Hotfix_Confirm_Event, null);
            hotfixImage.gameObject.SetActive(false);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

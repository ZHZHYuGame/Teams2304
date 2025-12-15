using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Google.Protobuf;
using Google;
using MyGame;

public class CreatPlayer : MonoBehaviour
{
    public static CreatPlayer instance;
    Dictionary<uint,GameObject>  PlayerDic = new Dictionary<uint, GameObject>();
    public GameObject player;
    public static uint PlayerID ; 
    public CinemachineFreeLook playerCamera;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        //PlayerID = 156;
        MessageControll.GetInstance().AddListener(NetID.S_To_C_PlayerOperation,RushPlayer);
        MessageControll.GetInstance().AddListener(NetID.S_To_C_Disconnect,S_To_C_DisconnectHandle);
        
        
    }
    /// <summary>
    /// 断开链接
    /// </summary>
    /// <param name="obj"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void S_To_C_DisconnectHandle(object obj)
    {
        object[] objs = obj as object[];
        Byte[] bytes = objs[0] as Byte[];
        S_To_C_Disconnect msg = S_To_C_Disconnect.Parser.ParseFrom(bytes);
        
        if (PlayerDic.ContainsKey(msg.PlayerId))
        {
            Destroy(PlayerDic[msg.PlayerId]);
            PlayerDic.Remove(msg.PlayerId);
        }
    }

    void Start()
    {
        C_To_S_PlayerOperation msg=new C_To_S_PlayerOperation();
        msg.PlayerId = PlayerID;
        player = Instantiate(Resources.Load<GameObject>("Role/1"));
        NetManager.GetInstance().SendMessage(NetID.C_To_S_PlayerOperation,msg.ToByteArray());
        
        if (playerCamera == null)
        {
            playerCamera = Instantiate(Resources.Load<CinemachineFreeLook>("FreeLook Camera"));
            
            playerCamera.Follow = player.transform;
            playerCamera.LookAt = player.transform;
        }
    }

    private void RushPlayer(object obj)
    {
        object[] par = obj as object[];
        byte[] data=par[0] as byte[];
        
        S_To_C_PlayerOperation msg= S_To_C_PlayerOperation.Parser.ParseFrom(data);
        if (msg.PlayerId == PlayerID)
        {
            return;
        }
        if (!PlayerDic.ContainsKey(msg.PlayerId))
        {
            PlayerDic[msg.PlayerId] = Instantiate(Resources.Load<GameObject>("Role/1"));
        }
        else
        {
            PlayerDic[msg.PlayerId].transform.position=new Vector3(msg.X,msg.Y,msg.Z);
            PlayerDic[msg.PlayerId].transform.rotation=Quaternion.Euler(0,msg.RoundY,0);
            
        }
        
    }

    private float timer;
    private float verticalAngle = 0f;
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        //移动
        Vector3 mainForward = Camera.main.transform.forward;
        mainForward.y = 0;
        Vector3 mainRight = Camera.main.transform.right;
        mainRight.y = 0;
        Vector3 moveDir = (mainForward*v+mainRight*h).normalized;
        player.transform.Translate(moveDir*Time.deltaTime*5,Space.World);
        player.transform.LookAt(player.transform.position+moveDir);
        

        LockMouse();
        
        //每秒30次更新位置
        timer += Time.deltaTime;
        if (timer>=0.033)
        {
            timer=0.0f;
            C_To_S_PlayerOperation msg=new C_To_S_PlayerOperation();
            msg.PlayerId = PlayerID;
            msg.X=player.transform.position.x;
            msg.Y=player.transform.position.y;
            msg.Z=player.transform.position.z;
            msg.RoundY=player.transform.rotation.eulerAngles.y;
            NetManager.GetInstance().SendMessage(NetID.C_To_S_PlayerOperation,msg.ToByteArray());
        }
    }
    private bool ishide;
    private void LockMouse()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
                Debug.Log("隐藏鼠标图标");
                ishide=true;
                Cursor.visible = false; // 隐藏鼠标图标
                Cursor.lockState = CursorLockMode.Locked; // 锁定鼠标到屏幕中心，且鼠标移动仅输出增量（无实际位置移动）
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("显示鼠标图标");
            ishide = false;
            Cursor.visible = true; // 显示鼠标图标
            Cursor.lockState = CursorLockMode.None; // 解除锁定，鼠标可自由移动
        }
        
    }

    private void LateUpdate()
    {
        //Camera.main.transform.position = player.transform.position+new Vector3(0,1,-5);
    }

    void OnApplicationQuit()
    {
        Debug.Log("你已断开连接");
        
        C_To_S_Disconnect msg = new C_To_S_Disconnect();
        msg.PlayerId = PlayerID;
        NetManager.GetInstance().SendMessage(NetID.C_To_S_Disconnect,msg.ToByteArray());
    }
}

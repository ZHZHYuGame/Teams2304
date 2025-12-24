using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using MyGame;
using Google.Protobuf;
using Google;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private Transform tou;
    public PlayerType Nowtype;
    private Rigidbody rig;
    public uint playerId;

    private Animator ani;
    private Collider collder;
    private float nowY = 0;
    private float deadTime = 3f;
    private bool is3Or1 = true;//控制切换第一和第三人称
    private bool isJu = true;
    
    public CinemachineVirtualCamera playerCamera1;
    
    void Start()
    { 
        rig = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        collder = GetComponent<Collider>();
        tou = transform.Find("tou");
        playerCamera1 = Instantiate(Resources.Load<CinemachineVirtualCamera>("Virtual Camera"),tou);
        playerCamera1.Follow = tou;
        playerCamera1.Priority = 5;
        //transform.AddComponent<CameraTest>();
    }

    private float h;
    private float v;
    //全自动的射击间隔
    private float fireTime = 0.2f;

    private bool aniOne = true;
    public void Init(uint id)
    {
        playerId = id;
        nowY = transform.position.y;
    }
    private float timer;
    public void Update()
    {
        if (Nowtype == PlayerType.Dead)
        {
            //3秒后复活
            deadTime -= Time.deltaTime;
            if (deadTime <= 0)
            {
                PlayerAnimatorMgr.GetInstance().SendAnimator(playerId,"Dead",false,PlayerAniType.BoolType);
                PlayerNetMgr.GetInstance().PlayerAliveNet(playerId);
                Nowtype  = PlayerType.None;
                transform.position = CreatPlayer.instance.RefreshPlayerPos();
                deadTime = 3f;
            }
            return;
        }
        //移动
        h=Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        //跳跃
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (PlayerJump(transform))
            {
                rig.AddForce(Vector3.up*5, ForceMode.Impulse);
            }
        }
        //更新位置
        if (h!=0||v!=0)
        {
            timer += Time.deltaTime;
            if (timer>=0.033)
            {
                PlayerNetMgr.GetInstance().RefreshPlayerPos(transform, playerId);
                timer=0.0f;
            }
            if (aniOne)
            {
                PlayerAnimatorMgr.GetInstance().SendAnimator(playerId,"Move",true,PlayerAniType.BoolType);
                aniOne = false;
            }
        }
        else
        {
            if (!aniOne)
            {
                PlayerAnimatorMgr.GetInstance().SendAnimator(playerId,"Move",false,PlayerAniType.BoolType);
                aniOne = true;
            }
        }
        //发射子弹
        if (Input.GetMouseButton(0))
        {
            fireTime-= Time.deltaTime;
            if (fireTime < 0)
            {
                transform.rotation=Quaternion.Euler(0,Camera.main.transform.rotation.eulerAngles.y,0);
                BulletMgr.GetInstance().InitBullet(playerId,tou);
                fireTime =0.2f;
            }
        }
        //右键进入狙击状态
        if (Input.GetMouseButtonDown(1))
        {
            if (isJu)
            {
                playerCamera1.m_Lens.FieldOfView= 15;
            }
            else
            {
                playerCamera1.m_Lens.FieldOfView= 60;
            }
            isJu = !isJu;
        }
        //更新y轴位置
        if (nowY != transform.position.y)
        {
            PlayerNetMgr.GetInstance().RefreshPlayerPos(transform, playerId);
            nowY = transform.position.y;
        }
        //第一人称与第三人称的切换
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            is3Or1 = !is3Or1;
            //第一人称
            if (!is3Or1)
            {
                playerCamera1.Priority = 11;
            }
            //第三人称
            else
            {
                playerCamera1.Priority = 5;
            }
            
        }

        if (!is3Or1)
        {
            Camera1();
            Move1();
        }
        else
        {
            Move3();
        }
    }
    float mouseX, mouseY;

    private void Move3()
    {
        Vector3 mainForward = Camera.main.transform.forward;
        mainForward.y = 0;
        Vector3 mainRight = Camera.main.transform.right;
        mainRight.y = 0;
        Vector3 moveDir = (mainForward*v+mainRight*h).normalized;
        transform.Translate(moveDir*Time.deltaTime*5,Space.World);
        transform.LookAt(transform.position+moveDir);
    }

    private void Move1()
    {
        transform.Translate(new Vector3(h*Time.deltaTime*6,0,v*Time.deltaTime*6),Space.Self);
    }
    private void Camera1 ()
    {
        //***第一人称视角代码****
        mouseX  = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        if (mouseX != 0 || mouseY != 0)
        {
            // 1. 获取当前对象的欧拉角（旋转角度）
            Vector3 currentEuler = tou.localEulerAngles;

            // 2. 处理欧拉角的“360°循环问题”（比如350°实际是-10°）
            float clampedX = currentEuler.x;
            if (clampedX > 180f) // 把0~360°的角度转换为-180~180°
            {
                clampedX -= 360f;
            }

            // 3. 核心：用Clamp限制X轴角度在设定范围内
            clampedX = Mathf.Clamp(clampedX, -80, 80);
            // 4. 把限制后的角度赋值回对象，仅修改X轴，保留Y/Z轴
            tou.localEulerAngles = new Vector3(clampedX, currentEuler.y, currentEuler.z);

            transform.Rotate(0,mouseX*Time.deltaTime*160,0);
            tou.Rotate(-mouseY*Time.deltaTime*220,0,0);
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Bullet EnemyBullet = other.gameObject.GetComponent<Bullet>();
            C_To_S_PlayerAtk msg = new C_To_S_PlayerAtk();
            EnemyBullet.Remove();
            msg.AtkPlayerId = EnemyBullet.playerID;
            msg.BAtkPlayerId = playerId;
            msg.BulletID=EnemyBullet.bulletID;
            NetManager.GetInstance().SendMessage(NetID.C_To_S_PlayerAtk,msg.ToByteArray());
            PlayerAnimatorMgr.GetInstance().SendAnimator(playerId,"Hurt",true,PlayerAniType.IsTriggerType);
        }
    }

    public void SetAnimator(string aniName,bool aniType,PlayerAniType type)
    {
        switch (type)
        {
            case PlayerAniType.BoolType:
                ani.SetBool(aniName, aniType);
                break;
            case PlayerAniType.IntType:
                //ani.SetInteger(aniName,);
                break;
            case PlayerAniType.IsTriggerType:
                ani.SetTrigger(aniName);
                break;
        }
        
        if (aniName=="Dead")
        {
            GetComponent<Collider>().enabled = !aniType;
            rig.useGravity = !aniType;
        }
    }
    
    /// <summary>
    /// 跳跃检测
    /// </summary>
    /// <param name="pos"></param>
    public bool PlayerJump(Transform tran)
    {
        List<Vector3> list = new List<Vector3>();
        Vector3 forward = transform.position+new Vector3(0,0,0.5f);
        Vector3 left = transform.position+new Vector3(-0.5f,0,0);
        Vector3 right = transform.position+new Vector3(0.5f,0,0);
        Vector3 back = transform.position+new Vector3(0,0,-0.5f);
        
        list.Add(forward);
        list.Add(left);
        list.Add(right);
        list.Add(back);
        foreach (Vector3 item in list)
        {
            if (Physics.Raycast(item,Vector3.down, out RaycastHit hit,0.3f))
            {
                if (hit.collider.gameObject.tag!="Player")
                {
                    return true;
                }
            }
        }
        return false;
    }
}

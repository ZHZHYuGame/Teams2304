using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame;
using Google.Protobuf;
using Google;

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

    public PlayerType Nowtype;
    private Rigidbody rig;
    public uint playerId;
    void Start()
    { 
        rig = GetComponent<Rigidbody>();
    }

    private float nowY = 0;
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
            return;
        }
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        //移动
        Vector3 mainForward = Camera.main.transform.forward;
        mainForward.y = 0;
        Vector3 mainRight = Camera.main.transform.right;
        mainRight.y = 0;
        Vector3 moveDir = (mainForward*v+mainRight*h).normalized;
        transform.Translate(moveDir*Time.deltaTime*5,Space.World);
        transform.LookAt(transform.position+moveDir);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rig.AddForce(Vector3.up*5, ForceMode.Impulse);
        }

        if (h!=0||v!=0)
        {
            timer += Time.deltaTime;
            if (timer>=0.033)
            {
            
                PlayerNetMgr.GetInstance().RefreshPlayerPos(transform, playerId);
                timer=0.0f;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            BulletMgr.GetInstance().InitBullet(playerId,transform);
        }

        if (nowY != transform.position.y)
        {
            PlayerNetMgr.GetInstance().RefreshPlayerPos(transform, playerId);
            nowY = transform.position.y;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("被攻击");
            Bullet EnemyBullet = other.gameObject.GetComponent<Bullet>();
            C_To_S_PlayerAtk msg = new C_To_S_PlayerAtk();
            EnemyBullet.Remove();
            msg.AtkPlayerId = EnemyBullet.playerID;
            msg.BAtkPlayerId = playerId;
            msg.BulletID=EnemyBullet.bulletID;
            NetManager.GetInstance().SendMessage(NetID.C_To_S_PlayerAtk,msg.ToByteArray());
        }
    }

    public void SetAnimator(string anim)
    {
        Debug.Log("动画");
    }
}

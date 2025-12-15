using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using MyGame;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private uint bulletID;
    private uint playerID;//这个子弹是谁打的
    private float timer;
    void Update()
    {
        transform.transform.Translate(transform.forward * Time.deltaTime*20,Space.World);
        timer  += Time.deltaTime;
        if (timer >=0.033f)
        {
            C_To_S_Bullet  msg = new C_To_S_Bullet();
            msg.BulletID = bulletID;
            msg.PlayerId = CreatPlayer.PlayerID;
            msg.X =transform.position.x;
            msg.Y = transform.position.y;
            msg.Z = transform.position.z;
            NetManager.GetInstance().SendMessage(NetID.C_To_S_Bullet,msg.ToByteArray());
            timer = 0;
        }
        
    }

    public void Init(uint playerID, uint bulletID)
    {
        this.playerID = playerID;
        this.bulletID =  bulletID;
        Invoke("Remove",5);
    }

    void Remove()
    {
        PlayerNetMgr.GetInstance().RemoveBullet(bulletID);
        Destroy(gameObject);
    }
}

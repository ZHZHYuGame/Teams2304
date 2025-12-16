using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using MyGame;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public uint bulletID;
    public uint playerID;//这个子弹是谁打的
    private float timer;
    void Update()
    {
        transform.transform.Translate(transform.forward * Time.deltaTime*20,Space.World);
    }

    public void Init(uint playerID, uint bulletID)
    {
        this.playerID = playerID;
        this.bulletID =  bulletID;
        if (playerID == CreatPlayer.PlayerID)
        {
            Invoke("Remove",5);
        }
        else
        {
            tag="Bullet";
        }
    }

    public void Remove()
    {
        PlayerNetMgr.GetInstance().RemoveBullet(bulletID);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
}

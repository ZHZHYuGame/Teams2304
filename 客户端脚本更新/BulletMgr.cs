using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using MyGame;
using UnityEngine;

public class BulletMgr : Singleton<BulletMgr>
{
    Dictionary<uint,GameObject>  biuDic = new Dictionary<uint, GameObject>();
    private uint bulletID;
    public void Init()
    {
        MessageControll.GetInstance().AddListener(NetID.S_To_C_Bullet,RefreshBullet);
    }

    private void RefreshBullet(object obj)
    {
        object[] objs = obj as object[];
        byte[] bytes = objs[0] as byte[];
        
        S_To_C_Bullet scMsg = S_To_C_Bullet.Parser.ParseFrom(bytes);
        if (biuDic.ContainsKey(scMsg.BulletID))
        {
            biuDic[scMsg.BulletID].transform.position = new Vector3(scMsg.X,scMsg.Y,scMsg.Z);
        }
        else
        {
            biuDic[scMsg.BulletID] = GameObject.Instantiate(Resources.Load<GameObject>("Bullet"));
            biuDic[scMsg.BulletID].transform.position = new Vector3(scMsg.X,scMsg.Y,scMsg.Z);
        }
    }
    float timer=0f;
    // Update is called once per frame
    public void BiuUpdate()
    {
        foreach (var item in biuDic)
        {
            if (item.Value.name == CreatPlayer.PlayerID.ToString())
            {   
                item.Value.transform.Translate(item.Value.transform.forward * Time.deltaTime*20, Space.World);
            }
        }
        timer +=Time.deltaTime;
        if (timer >= 0.033f)
        {
            C_To_S_Bullet msg = new C_To_S_Bullet();
            foreach (var item in biuDic)
            {
                if (item.Value.name == CreatPlayer.PlayerID.ToString())
                {
                    
                    msg.BulletID = item.Key;
                    msg.PlayerId = CreatPlayer.PlayerID;
                    msg.X = item.Value.transform.position.x;
                    msg.Y = item.Value.transform.position.y;
                    msg.Z = item.Value.transform.position.z;
                    NetManager.GetInstance().SendMessage(NetID.C_To_S_Bullet,msg.ToByteArray());
                }
            }
            timer=0.0f;
        }
    }

    public void InitBullet(uint playerID,Transform player)
    {
        if (biuDic.Count == 0)
        {
            bulletID = 0;
        }
        GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>("Bullet"));
        obj.transform.position = player.transform.position+player.transform.forward;
        obj.transform.rotation = player.transform.rotation;
        obj.name = playerID.ToString();
        
        if (!biuDic.ContainsKey(bulletID))
        {
            biuDic[bulletID] = obj;
            bulletID++;
        }
    }
    
    public void RemoveBullet(uint bulletID)
    {
        if (biuDic.ContainsKey(bulletID))
        {
            GameObject.Destroy(biuDic[bulletID]);
            biuDic.Remove(bulletID);
        }
    }
}

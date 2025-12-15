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
        MessageControll.GetInstance().AddListener(NetID.S_To_C_ReomveBullet,RemoveBulletHandle);
    }

    private void RemoveBulletHandle(object obj)
    {
        object[] objs = obj as object[];
        byte[] bytes = objs[0] as byte[];
        S_To_C_RemoveBullet scMsg = S_To_C_RemoveBullet.Parser.ParseFrom(bytes);
        RemoveBullet(scMsg.BulletID);
        Debug.Log("删除成功");
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

    public void InitBullet(uint playerID,Transform player)
    {
        GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>("Bullet"));
        obj.transform.position = player.transform.position+player.transform.forward;
        obj.transform.rotation = player.transform.rotation;
        obj.name = playerID.ToString();
        obj.AddComponent<Bullet>().Init(playerID,bulletID);
        bulletID++;
    }
    
    public void RemoveBullet(uint bulletID)
    {
        if (biuDic.ContainsKey(bulletID))
        {
            C_To_S_RemoveBullet csMsg = new C_To_S_RemoveBullet();
            csMsg.BulletID = bulletID;
            NetManager.GetInstance().SendMessage(NetID.C_To_S_ReomveBullet,csMsg.ToByteArray());
            GameObject.Destroy(biuDic[bulletID]);
            biuDic.Remove(bulletID);
        }
    }
}

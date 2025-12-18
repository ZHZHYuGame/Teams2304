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
        
    }

    private void RefreshBullet(object obj)
    {
        object[] objs = obj as object[];
        byte[] bytes = objs[0] as byte[];
        
        S_To_C_Bullet scMsg = S_To_C_Bullet.Parser.ParseFrom(bytes);
        if (!biuDic.ContainsKey(scMsg.BulletID))
        {
            Transform atkPlayer = CreatPlayer.instance.GetPlayer(scMsg.PlayerId);
            InitBullet(scMsg.PlayerId, atkPlayer,scMsg.BulletID,scMsg.Rotation);
        }
    }

    public void InitBullet(uint playerID,Transform player,uint bulletID,RotationData rotation)
    {
        GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>("Bullet"));
        obj.transform.position = player.transform.position+player.transform.forward+Vector3.up;
        obj.transform.rotation = player.transform.rotation;
        obj.transform.rotation=Quaternion.Euler(rotation.X,rotation.Y,rotation.Z);
        obj.name = playerID.ToString();
        obj.AddComponent<Bullet>().Init(playerID,bulletID);
        biuDic[bulletID]=obj;
    }
    public void InitBullet(uint playerID,Transform player)
    {
        GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>("Bullet"));
        obj.transform.position = player.transform.position+player.transform.forward+Vector3.up;
        obj.transform.rotation = Camera.main.transform.rotation;
        obj.name = playerID.ToString();
        obj.AddComponent<Bullet>().Init(playerID,bulletID);
        C_To_S_Bullet  msg = new C_To_S_Bullet();
        msg.BulletID = bulletID;
        msg.PlayerId = CreatPlayer.PlayerID;
        msg.Rotation.X=Camera.main.transform.rotation.eulerAngles.x;
        msg.Rotation.Y=Camera.main.transform.rotation.eulerAngles.y;
        msg.Rotation.Z=Camera.main.transform.rotation.eulerAngles.z;
        NetManager.GetInstance().SendMessage(NetID.C_To_S_Bullet,msg.ToByteArray());
        bulletID++;
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

using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Map : Singleton<Map>
{
    CinemachineVirtualCamera role_Camera;
    GameObject player;
    public  void Create(string Rolename)
    {
        role_Camera = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        GameObject.Instantiate(ABManager.GetInstance().LoadAsset_GameObject("map"));
        player = GameObject.Instantiate(ABManager.GetInstance().LoadAsset_GameObject(Rolename));
        player.transform.Rotate(Vector3.up,180);
        role_Camera.Follow = player.transform;
        role_Camera.LookAt = player.transform;
        player.AddComponent<Player>();
    }
}

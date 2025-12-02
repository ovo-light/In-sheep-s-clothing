using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Entity : MonoBehaviourPun
{
    private void Start()
    {
        // 获取当前实体的所有者（生成这个羊的玩家）
        Player owner = photonView.Owner;
        // 将“玩家-羊实体”注册到管理类
        PlayerEntityManager.Instance.RegisterPlayerEntity(owner, gameObject);
    }
}
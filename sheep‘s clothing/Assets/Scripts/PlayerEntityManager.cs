using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

// 管理所有玩家的游戏实体（羊/牧羊人）
public class PlayerEntityManager : MonoBehaviourPunCallbacks
{
    public static PlayerEntityManager Instance;

    // 字典：键=Photon玩家对象，值=对应的游戏实体（羊/牧羊人）
    private Dictionary<Player, GameObject> playerToEntity = new Dictionary<Player, GameObject>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 注册玩家和对应的实体
    public void RegisterPlayerEntity(Player player, GameObject entity)
    {
        if (!playerToEntity.ContainsKey(player))
        {
            playerToEntity.Add(player, entity);
        }
    }

    // 根据玩家获取对应的实体
    public GameObject GetPlayerEntity(Player player)
    {
        playerToEntity.TryGetValue(player, out GameObject entity);
        return entity;
    }

    // 移除玩家的实体（销毁后调用）
    public void RemovePlayerEntity(Player player)
    {
        if (playerToEntity.ContainsKey(player))
        {
            playerToEntity.Remove(player);
        }
    }
}

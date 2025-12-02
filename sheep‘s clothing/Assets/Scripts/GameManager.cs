using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using System.Linq;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;
    public PhotonView photonView1;

    [Header("游戏设置")]
    public float shepherdWaitTime = 5f; // 牧羊人等待时间（羊躲藏时间）

    private List<Player> allPlayers = new List<Player>();
    private bool isGameStarted = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 开始角色分配（在RPC_StartGame中调用）
    public void StartRoleAssignment()
    {
        if (!PhotonNetwork.IsMasterClient) return; // 仅房主执行分配
        if (isGameStarted) return;

        // 获取房间内所有玩家
        allPlayers = PhotonNetwork.CurrentRoom.Players.Values.ToList();

        // 随机选择1个玩家作为牧羊人
        int randomIndex = Random.Range(0, allPlayers.Count);
        Player shepherdPlayer = allPlayers[randomIndex];

        // 给每个玩家设置角色属性（Sheep/Shepherd）
        foreach (var player in allPlayers)
        {
            PlayerRole role = (player == shepherdPlayer) ? PlayerRole.Shepherd : PlayerRole.Sheep;
            player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Role", role } });
        }

        // 通知所有玩家角色分配完成
        photonView.RPC("RPC_OnRolesAssigned", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_OnRolesAssigned()
    {
        isGameStarted = true;
        // 每个玩家初始化自己的角色逻辑
        Player localPlayer = PhotonNetwork.LocalPlayer;
        if (localPlayer.CustomProperties.TryGetValue("Role", out object roleObj))
        {
            PlayerRole role = (PlayerRole)roleObj;
            InitializePlayerRole(role);
        }
    }

    // 初始化玩家角色（羊/牧羊人）
    private void InitializePlayerRole(PlayerRole role)
    {
        if (role == PlayerRole.Shepherd)
        {
            // 1. 获取当前玩家对应的羊实体
            Player localPlayer = PhotonNetwork.LocalPlayer;
            GameObject sheepEntity = PlayerEntityManager.Instance.GetPlayerEntity(localPlayer);

            if (sheepEntity != null)
            {
                // 2. 网络同步销毁羊实体（所有客户端都会销毁）
                PhotonNetwork.Destroy(sheepEntity);
                // 3. 从管理类中移除羊实体的记录
                PlayerEntityManager.Instance.RemovePlayerEntity(localPlayer);
            }

            PhotonNetwork.Instantiate("Shepherd", new Vector3(0, 0, 0), Quaternion.identity, 0);

        }
    }
}

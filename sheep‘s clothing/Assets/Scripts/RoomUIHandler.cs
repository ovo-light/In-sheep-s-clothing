using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.Demo.PunBasics;

public class RoomUIHandler : MonoBehaviourPunCallbacks
{
    [Header("UI引用")]
    public TMP_Text currentPlayersText;
    public Button startGameButton;
    public GameObject roomUIPanel;

    private bool isPanelVisible = false;
    public PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        // 初始禁用开始按钮
        startGameButton.interactable = false;
        startGameButton.onClick.AddListener(OnStartGameClicked);

        
        LockMouse(true); // 初始锁定鼠标

    }

    void Update()
    {
        // 监听~键（KeyCode.BackQuote），按下切换面板显示/隐藏
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            isPanelVisible = !isPanelVisible;
            roomUIPanel.SetActive(isPanelVisible); // 切换面板激活状态
            LockMouse(!isPanelVisible);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdateRoomUI();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateRoomUI();
    }

    public override void OnJoinedRoom()
    {
        UpdateRoomUI();
    }

    private void UpdateRoomUI()
    {
        if (PhotonNetwork.CurrentRoom == null) return;

        // 显示当前玩家数/最大玩家数
        currentPlayersText.text = $"当前玩家：{PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";

        // 仅房主且玩家数≥2时，启用开始按钮
        bool isHost = PhotonNetwork.IsMasterClient;
        bool canStart = isHost && PhotonNetwork.CurrentRoom.PlayerCount >= 2;
        startGameButton.interactable = canStart;
    }

    // 房主点击开始游戏
    private void OnStartGameClicked()
    {
        // 通知所有玩家开始游戏（通过RPC同步）
        photonView.RPC("RPC_StartGame", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_StartGame()
    {
        // 隐藏房间UI，进入游戏场景（如果需要切换场景，用PhotonNetwork.LoadLevel）
        roomUIPanel.SetActive(false);
        Debug.Log("成功开始游戏");
        // 触发角色分配逻辑（见需求2）
        GameManager.Instance.StartRoleAssignment();
    }

    // 鼠标锁定/解锁控制
    private void LockMouse(bool isLock)
    {
        if (isLock)
        {
            // 锁定鼠标（游戏状态）：隐藏指针+锁定到屏幕中心
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            // 解锁鼠标（UI状态）：显示指针+可自由移动
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    
    
}

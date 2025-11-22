using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;




public class NewBehaviourScript : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("连接成功");

        PhotonNetwork.JoinOrCreateRoom("testRoom", new RoomOptions() { MaxPlayers = 4 }, default);
        
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"加入房间失败！错误码：{returnCode}，原因：{message}");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("进入房间，开始实例化Sheep...");
        PhotonNetwork.Instantiate("Player", new Vector3(0, 0, 0), Quaternion.identity, 0);
    }

}

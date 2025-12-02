using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraController : MonoBehaviourPun
{
    private PhotonView playerPhotonView;
    //private AudioListener audioListener;


    void Start()
    {
        playerPhotonView = transform.root.GetComponent<PhotonView>();
        if (playerPhotonView != null)
        {
            gameObject.SetActive(photonView.IsMine);
        }

        //audioListener = GetComponent<AudioListener>();
        Camera mainCamera = GetComponent<Camera>(); // 获取摄像头组件

        if (!playerPhotonView.IsMine)
        {
            // 远程玩家：禁用摄像头和Audio Listener
            mainCamera.enabled = false;
            //audioListener.enabled = false;
        }
        else
        {
            // 本地玩家：确保摄像头和Audio Listener激活
            mainCamera.enabled = true;
            //audioListener.enabled = true;
        }
    }


}

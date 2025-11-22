using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraController : MonoBehaviourPun
{
    private PhotonView playerPhotonView;
    void Start()
    {
        playerPhotonView = transform.parent.GetComponent<PhotonView>();
        if (playerPhotonView != null)
        {
            gameObject.SetActive(photonView.IsMine);
        }
        

    }

}
